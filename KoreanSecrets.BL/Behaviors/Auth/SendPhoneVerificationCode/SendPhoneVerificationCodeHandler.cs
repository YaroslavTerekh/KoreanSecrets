using Hangfire;
using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.BL.Services.Realizations;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Settings;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace KoreanSecrets.BL.Behaviors.Auth.SendPhoneVerificationCode;

public class SendPhoneVerificationCodeHandler : IRequestHandler<SendPhoneVerificationCodeCommand>
{
    private readonly DataContext _context;
    private readonly IPhoneNumberService _phoneNumberService;
    private readonly TwillioSettings _twilioSettings;


    public SendPhoneVerificationCodeHandler(DataContext context, IPhoneNumberService phoneNumberService, TwillioSettings twillioSettings)
    {
        _context = context;
        _phoneNumberService = phoneNumberService;
        _twilioSettings = twillioSettings;
    }

    public async Task<Unit> Handle(SendPhoneVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(t => t.Id == request.UserId, cancellationToken);
        var checkPhoneNumberUser = await _context.Users.FirstOrDefaultAsync(t => t.PhoneNumber == request.PhoneNumber && t.Id != request.UserId, cancellationToken);

        if (checkPhoneNumberUser is not null)
            throw new Exception(ErrorMessages.UserWithSamePhoneExists);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        if ((DateTime.UtcNow - user.CreatedTime).TotalMinutes > 10)
        {
            throw new Exception(ErrorMessages.UserExpired);
        }

        if (user.PhoneNumberConfirmed)
            throw new Exception(ErrorMessages.PhoneNumberAlreadyConfirmed);

        var code = new Random().Next(100000, 999999);
        user.PhoneNumber = _phoneNumberService.FormatPhoneNumber(request.PhoneNumber);
        user.TemporaryCode = code;

        await _context.SaveChangesAsync(cancellationToken);

        TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);

        var message = await MessageResource.CreateAsync(
            body: ValidationMessages.VerificationCodeInfo(user.TemporaryCode),
            from: new PhoneNumber(_twilioSettings.FromPhoneNumber),
            to: new PhoneNumber(user.PhoneNumber)
        );

        return Unit.Value;
    }
}
