using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.BL.Services.Realizations;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.Settings;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.ModifyVolumeQuantity;

public class ModifyVolumeQuantityHandler : IRequestHandler<ModifyVolumeQuantityCommand>
{
    private readonly DataContext _context;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _config;
    private readonly IPhoneNumberService _phoneNumberService;
    private readonly TwillioSettings _twilioSettings;

    public ModifyVolumeQuantityHandler(DataContext context, IEmailService emailService, IConfiguration config, IPhoneNumberService phoneNumberService, TwillioSettings twillioSettings)
    {
        _context = context;
        _emailService = emailService;
        _config = config;
        _twilioSettings = twillioSettings;
        _phoneNumberService = phoneNumberService;
    }

    public async Task<Unit> Handle(ModifyVolumeQuantityCommand request, CancellationToken cancellationToken)
    {
        TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);

        var volume = await _context.Volume.Include(volume => volume.UsersWaitingForStock)
            .ThenInclude(volumeUser => volumeUser.User).Include(volume => volume.Product)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (volume is null) throw new Exception(ErrorMessages.ProductNotFound("Об'єкту об'єму для модифікації"));

        if (volume.Quantity == 0 && request.Quantity > 0)
        {
            try
            {
                if (volume.IsInStock)
                {
                    var message = new Message(volume.UsersWaitingForStock.Select(t => t.User.Email).ToArray(),
                        "Товар в наявності!", String.Concat("Товар", volume.Product.Title, "з'явився у наявності!", _config.GetSection("HostSettings:FrontApplicationUrl"), "home/item/", volume.Product.Id));

                    await _emailService.SendEmailAsync(message, "Товар в наявності");

                    volume.UsersWaitingForStock.Clear();

                    await _context.SaveChangesAsync(cancellationToken);

                    foreach (var volumeUser in volume.UsersWaitingForStock)
                    {
                        await SendMessage($"Secrets of care | Товар {volume.Product.Title} у наявності!",
                            volumeUser.User.PhoneNumber);
                        await SendMessage($"https://www.secretsofcare.com.ua/home/item/{volumeUser.Volume.ProductId}",
                            volumeUser.User.PhoneNumber);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        volume.Quantity = request.Quantity;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task SendMessage(string text, string phone)
    {
        try
        {
            await MessageResource.CreateAsync(
            body: text,
                from: new PhoneNumber(_twilioSettings.FromPhoneNumber),
                to: new PhoneNumber(_phoneNumberService.FormatPhoneNumber(phone))
            );
        }
        catch (Exception e)
        {
        }
    }
}
