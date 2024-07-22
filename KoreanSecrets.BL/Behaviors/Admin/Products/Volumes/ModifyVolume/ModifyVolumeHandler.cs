using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.BL.Services.Realizations;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.Settings;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.ModifyVolume;

public class ModifyVolumeHandler : IRequestHandler<ModifyVolumeCommand>
{
    private readonly DataContext _context;
    private readonly IEmailService _emailService;
    private readonly IPhoneNumberService _phoneNumberService;
    private readonly TwillioSettings _twilioSettings;

    public ModifyVolumeHandler(DataContext context, IEmailService emailService, IPhoneNumberService phoneNumberService, TwillioSettings twillioSettings)
    {
        _context = context;
        _emailService = emailService;
        _phoneNumberService = phoneNumberService;
        _twilioSettings = twillioSettings;
    }

    public async Task<Unit> Handle(ModifyVolumeCommand request, CancellationToken cancellationToken)
    {
        TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);

        var volume = await _context.Volume.Include(volume => volume.UsersWaitingForStock)
            .ThenInclude(volumeUser => volumeUser.User)
            .Include(volume => volume.Product)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (volume is null) throw new Exception(ErrorMessages.ProductNotFound("Об'єкту об'єму для модифікації"));

        volume.Price = request.Price;
        volume.Unit = request.Unit;
        volume.Value = request.Value;
        
        if(volume.Quantity == 0 && request.Quantity > 0)
        {
            try
            {
                if (volume.IsInStock)
                {
                    var message = new Message(volume.UsersWaitingForStock.Select(t => t.User.Email).ToArray(), "Товар в наявності!", volume.Product.Title);

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
