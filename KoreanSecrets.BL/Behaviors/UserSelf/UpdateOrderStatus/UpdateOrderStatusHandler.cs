using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Enums;
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
using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Settings;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace KoreanSecrets.BL.Behaviors.UserSelf.UpdateOrderStatus;

public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand>
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;
    private readonly TwillioSettings _twilioSettings;
    private readonly IPhoneNumberService _phoneNumberService;
    public UpdateOrderStatusHandler(DataContext context, UserManager<User> roleManager, TwillioSettings twilioSettings, IPhoneNumberService phoneNumberService)
    {
        _context = context;
        _userManager = roleManager;
        _twilioSettings = twilioSettings;
        _phoneNumberService = phoneNumberService;
    }

    public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Purchases
            .Include(t => t.User)
            .Include(t => t.Products)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        var currentUser = await _context.Users.FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        if (order is null) throw new NotFoundException(ErrorMessages.PurchaseNotFound);

        if (currentUser is null) throw new NotFoundException(ErrorMessages.UserNotFound);

        if (order.UserId != request.CurrentUserId && !await _userManager.IsInRoleAsync(currentUser, Roles.Admin)) 
            throw new Exception(ErrorMessages.PurchaseNotRelatedToUser);

        TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);


        switch (request.Status)
        {
            case PurchaseStatus.Waiting:
                await SendMessage($"Secrets of care | Замовлення '{order.PurchaseIdentifier}'. Оплата підтверджена, очікуйте на наступні повідолення", 
                    order.User.PhoneNumber, order.Phone);
                await SendMessage($"https://www.secretsofcare.com.ua/home/purchase/{order.PurchaseIdentifier}", 
                    order.User.PhoneNumber, order.Phone);
                break;
            case PurchaseStatus.New:
                await SendMessage($"Secrets of care | Замовлення '{order.PurchaseIdentifier}' зареєстовано, очікуйте на наступні повідолення.", 
                    order.User.PhoneNumber, order.Phone);
                await SendMessage($"https://www.secretsofcare.com.ua/home/purchase/{order.PurchaseIdentifier}", 
                    order.User.PhoneNumber, order.Phone);
                break;
            case PurchaseStatus.InProgress:
                await SendMessage($"Secrets of care | Ваше замовлення '{order.PurchaseIdentifier}' в обробці", 
                    order.User.PhoneNumber, order.Phone);
                await SendMessage($"https://www.secretsofcare.com.ua/home/purchase/{order.PurchaseIdentifier}", 
                    order.User.PhoneNumber, order.Phone);
                break;
            case PurchaseStatus.SendViaPost:
                await SendMessage($"Secrets of care | Ваше замовлення '{order.PurchaseIdentifier}' відправлено у відділення Нової пошти", 
                    order.User.PhoneNumber, order.Phone);
                await SendMessage($"https://www.secretsofcare.com.ua/home/purchase/{order.PurchaseIdentifier}", 
                    order.User.PhoneNumber, order.Phone);
                break;
            case PurchaseStatus.SendByAdmin:
                await SendMessage($"Secrets of care | Ваше замовлення '{order.PurchaseIdentifier}' в пункті самовивозу", 
                    order.User.PhoneNumber, order.Phone);
                await SendMessage($"https://www.secretsofcare.com.ua/home/purchase/{order.PurchaseIdentifier}", 
                    order.User.PhoneNumber, order.Phone);
                break;
            case PurchaseStatus.Success:
                await SendMessage($"Secrets of care | Дякуємо, за замовлення '{order.PurchaseIdentifier}'! Очікуєм на Ваш відгук!", 
                    order.User.PhoneNumber, order.Phone);
                await SendMessage($"https://www.secretsofcare.com.ua/home/purchase/{order.PurchaseIdentifier}", 
                    order.User.PhoneNumber, order.Phone);
                break;
            case PurchaseStatus.Failure:
                await SendMessage($"Secrets of care | Ваше замовлення '{order.PurchaseIdentifier}' скасовано!", 
                    order.User.PhoneNumber, order.Phone);
                await SendMessage($"https://www.secretsofcare.com.ua/home/purchase/{order.PurchaseIdentifier}", 
                    order.User.PhoneNumber, order.Phone);
                break;
        }
        
        if (request.Status == PurchaseStatus.Failure)
        {
            var productIds = order.Products.Select(t => Guid.Parse(t.ProductIdentify)).ToList();
            var products = await _context.Products.Where(t => productIds.Contains(t.Id)).ToListAsync(cancellationToken);

            foreach (var product in products)
            {
                foreach (var purchasedProduct in order.Products)
                {
                    if (Guid.Parse(purchasedProduct.ProductIdentify) == product.Id)
                    {
                        var volume = await _context.Volume.FirstOrDefaultAsync(t => t.Id == Guid.Parse(purchasedProduct.VolumeIdentify), cancellationToken);

                        if (volume is null)
                            throw new NotFoundException(ErrorMessages.VolumeNotFound);

                        volume.Quantity -= purchasedProduct.Amount;

                        break;
                    }
                }
            }
        }

        if (request.Status != PurchaseStatus.Failure && order.PurchaseStatus == PurchaseStatus.Failure)
        {
            var productIds = order.Products.Select(t => Guid.Parse(t.ProductIdentify)).ToList();
            var products = await _context.Products.Where(t => productIds.Contains(t.Id)).ToListAsync(cancellationToken);

            foreach (var product in products)
            {
                foreach (var purchasedProduct in order.Products)
                {
                    if (Guid.Parse(purchasedProduct.ProductIdentify) == product.Id)
                    {
                        var volume = await _context.Volume.FirstOrDefaultAsync(t => t.Id == Guid.Parse(purchasedProduct.VolumeIdentify), cancellationToken);

                        if (volume is null)
                            throw new NotFoundException(ErrorMessages.VolumeNotFound);

                        if (purchasedProduct.Amount! > volume.Quantity)
                        {
                            volume.Quantity -= purchasedProduct.Amount;
                            if (volume.Quantity <= 0) product.IsInStock = false;
                        }
                        break;
                    }
                }
            }
        }

        order.PurchaseStatus = request.Status;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task SendMessage(string text, string phone, string requestPhone)
    {
        try
        {
            if (phone.Contains(requestPhone))
            {
                await MessageResource.CreateAsync(
                    body: text,
                    from: new PhoneNumber(_twilioSettings.FromPhoneNumber),
                    to: new PhoneNumber(_phoneNumberService.FormatPhoneNumber(phone))
                );
            }
            else
            {
                await MessageResource.CreateAsync(
                    body: text,
                    from: new PhoneNumber(_twilioSettings.FromPhoneNumber),
                    to: new PhoneNumber(_phoneNumberService.FormatPhoneNumber(phone))
                );
                
                await MessageResource.CreateAsync(
                    body: text,
                    from: new PhoneNumber(_twilioSettings.FromPhoneNumber),
                    to: new PhoneNumber(_phoneNumberService.FormatPhoneNumber(requestPhone))
                );
            }
        }
        catch (Exception e)
        {
        }
    }
}
