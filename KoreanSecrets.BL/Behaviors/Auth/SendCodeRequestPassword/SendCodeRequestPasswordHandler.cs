using Hangfire;
using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Settings;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace KoreanSecrets.BL.Behaviors.Auth.SendCodeRequestPassword;

public class SendCodeRequestPasswordHandler : IRequestHandler<SendCodeRequestPasswordCommand>
{
    private readonly DataContext _context;
    private readonly IPhoneNumberService _phoneNumberService;
    private readonly TwillioSettings _twilioSettings;


    public SendCodeRequestPasswordHandler(DataContext context, IPhoneNumberService phoneNumberService, TwillioSettings twillioSettings)
    {
        _context = context;
        _phoneNumberService = phoneNumberService;
        _twilioSettings = twillioSettings;
    }

    public async Task<Unit> Handle(SendCodeRequestPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(t => t.PhoneNumber == _phoneNumberService.FormatPhoneNumber(request.PhoneNumber), cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        var code = new Random().Next(100000, 999999);
        user.TemporaryCode = code;

        await _context.SaveChangesAsync(cancellationToken);

        BackgroundJob.Schedule(() => DeleteConfirmationCode(user.Id, cancellationToken), TimeSpan.FromMinutes(10));

        TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);

        var message = await MessageResource.CreateAsync(
            body: ValidationMessages.VerificationCodeInfo(user.TemporaryCode),
            from: new PhoneNumber(_twilioSettings.FromPhoneNumber),
            to: new PhoneNumber(_phoneNumberService.FormatPhoneNumber(request.PhoneNumber))
        );

        return Unit.Value;
    }

    public async Task DeleteConfirmationCode(Guid id, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        user.TemporaryCode = null;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
