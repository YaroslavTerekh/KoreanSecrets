using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Settings;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        // ТУТ надіслати КОД, джоба шоб делітнути код за 10 хв
        
        // var user = await _context.Users.FirstOrDefaultAsync(t => t.Id == request.UserId, cancellationToken);
        // var checkPhoneNumberUser = await _context.Users.FirstOrDefaultAsync(t => t.PhoneNumber == request.PhoneNumber && t.Id != request.UserId, cancellationToken);
        //
        // if (checkPhoneNumberUser is not null)
        //     throw new Exception(ErrorMessages.UserWithSamePhoneExists);
        //
        // if (user is null)
        //     throw new NotFoundException(ErrorMessages.UserNotFound);
        //
        // if ((DateTime.UtcNow - user.CreatedTime).TotalMinutes > 10)
        // {
        //     throw new Exception(ErrorMessages.UserExpired);
        // }
        //
        // if (user.PhoneNumberConfirmed)
        //     throw new Exception(ErrorMessages.PhoneNumberAlreadyConfirmed);
        //
        // var code = new Random().Next(100000, 999999);
        // user.PhoneNumber = _phoneNumberService.FormatPhoneNumber(request.PhoneNumber);
        // user.TemporaryCode = code;
        //
        // await _context.SaveChangesAsync(cancellationToken);
        //
        return Unit.Value;
    }
}
