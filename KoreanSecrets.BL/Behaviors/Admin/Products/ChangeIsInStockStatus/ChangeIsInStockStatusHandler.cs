using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.BL.Services.Realizations;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Settings;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using KoreanSecrets.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.ChangeIsInStockStatus;

public class ChangeIsInStockStatusHandler : IRequestHandler<ChangeIsInStockStatusCommand>
{
    private readonly DataContext _context;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _config;
    private readonly IPhoneNumberService _phoneNumberService;
    private readonly TwillioSettings _twilioSettings;

    public ChangeIsInStockStatusHandler(DataContext context, IEmailService emailService, IConfiguration config, IPhoneNumberService phoneNumberService, TwillioSettings twillioSettings)
    {
        _context = context;
        _emailService = emailService;
        _config = config;
        _phoneNumberService = phoneNumberService;
        _twilioSettings = twillioSettings;
    }

    public async Task<Unit> Handle(ChangeIsInStockStatusCommand request, CancellationToken cancellationToken)
    {
        TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);

        var product = await _context.Products
            .Include(t => t.Volumes)
                .ThenInclude(t => t.UsersWaitingForStock)
                    .ThenInclude(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        product.IsInStock = !product.IsInStock;
        await _context.SaveChangesAsync(cancellationToken);

        foreach (var volume in product.Volumes)
        {
            try
            {
                foreach (var volumeUser in volume.UsersWaitingForStock)
                {
                    await SendMessage($"Secrets of care | Товар {volume.Product.Title} у наявності!",
                        volumeUser.User.PhoneNumber);
                    await SendMessage($"https://www.secretsofcare.com.ua/home/item/{volumeUser.Volume.ProductId}",
                        volumeUser.User.PhoneNumber);
                }

                volume.UsersWaitingForStock.Clear();

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

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
