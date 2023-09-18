using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.Common.Settings;
using KoreanSecrets.Domain.DbConnection;
using LiqPay.SDK;
using LiqPay.SDK.Dto;
using LiqPay.SDK.Dto.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Services.Realizations;

public class LiqPayService : ILiqPayService
{
    private readonly DataContext _context;
    private readonly LiqPaySettings _liqPaySettings;

    public LiqPayService(DataContext context, LiqPaySettings liqPaySettings)
    {
        _context = context;
        _liqPaySettings = liqPaySettings;
    }

    public async Task<string> GenerateForm(Guid purchaseId, CancellationToken cancellationToken = default)
    {
        var purchase = await _context.Purchases.FirstOrDefaultAsync(t => t.Id == purchaseId, cancellationToken);

        if (purchase is null)
            throw new NotFoundException(ErrorMessages.PurchaseProductNotRelatedToUser);

        var invoiceRequest = new LiqPayRequest
        {
            PublicKey = _liqPaySettings.PublicKey,
            Version = 3,
            Amount = purchase.TotalPrice,
            Currency = "UAH",
            OrderId = purchase.Id.ToString(),
            Action = LiqPayRequestAction.Pay,
            Language = LiqPayRequestLanguage.UK,
            Description = $"Оплата замовлення №{purchase.PurchaseIdentifier} | Korean Secrets",
            ServerUrl = _liqPaySettings.ServerUrl,
        };

        var liqPayClient = new LiqPayClient(_liqPaySettings.PublicKey, _liqPaySettings.PrivateKey);
        return liqPayClient.CNBForm(invoiceRequest);
    }

    public async Task ProcessCallbackAsync(Dictionary<string, string> data, CancellationToken cancellationToken = default)
    {
        var response = DecodeResponse(data);
        var newStatus = response.Status switch
        {
            LiqPayResponseStatus.Success => PurchaseStatus.Success,
            LiqPayResponseStatus.Failure => PurchaseStatus.Failure,
            _ => PurchaseStatus.Failure
        };

        await ChangePurchaseStatusAsync(response, newStatus, cancellationToken);
    }

    private LiqPayResponse DecodeResponse(Dictionary<string, string> data)
    {
        byte[] bytes = Convert.FromBase64String(data["data"]);

        string jsonString = Encoding.UTF8.GetString(bytes);

        LiqPayResponse response = JsonConvert.DeserializeObject<LiqPayResponse>(jsonString);

        return response;
    }

    private async Task ChangePurchaseStatusAsync(LiqPayResponse response, PurchaseStatus status, CancellationToken cancellationToken = default)
    {
        var orderId = Guid.Parse(response.OrderId);
        var purchase = await _context.Purchases
            .FirstOrDefaultAsync(t => t.Id == orderId, cancellationToken);

        if (purchase is null)
            throw new NotFoundException(ErrorMessages.PurchaseProductNotRelatedToUser);

        purchase.PurchaseStatus = status;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
