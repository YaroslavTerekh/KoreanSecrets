using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Settings;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace KoreanSecrets.BL.Behaviors.Admin.Users.SendSMS;

public class SendSMSHandler : IRequestHandler<SendSMSCommand>
{
    private readonly DataContext _context;
    private readonly IPhoneNumberService _phoneNumberService;
    private readonly TwillioSettings _twilioSettings;


    public SendSMSHandler(DataContext context, IPhoneNumberService phoneNumberService, TwillioSettings twillioSettings)
    {
        _context = context;
        _phoneNumberService = phoneNumberService;
        _twilioSettings = twillioSettings;
    }

    public async Task<Unit> Handle(SendSMSCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(t => t.PhoneNumber == request.PhoneNumber, cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);
        
        TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);

        await MessageResource.CreateAsync(
            body: "Secrets of care | " + request.Text,
            from: new PhoneNumber(_twilioSettings.FromPhoneNumber),
            to: new PhoneNumber(_phoneNumberService.FormatPhoneNumber(user.PhoneNumber))
        );

        return Unit.Value;
    }
}
