using Hangfire;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Promocodes.AddPromocode;

public class AddPromocodeHandler : IRequestHandler<AddPromocodeCommand>
{
    private readonly DataContext _context;

    public AddPromocodeHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddPromocodeCommand request, CancellationToken cancellationToken)
    {
        var promocode = new Promocode
        {
            Code = request.Title,
            Discount = request.Discount,
            StartDate = request.StartDate.AddHours(12),
            EndDate = request.EndDate.AddHours(12),
        };

        if (request.BrandId is not null)
        {
            promocode.BrandId = request.BrandId;
        }
        
        if (request.StartDate > request.EndDate)
            throw new Exception(ErrorMessages.DateNotMatch);

        if (request.EndDate < DateTime.UtcNow)
            throw new Exception(ErrorMessages.DateNotMatch);

        if(request.ProductIds is not null)
        {
            var productPromocodes = request.ProductIds.Select(t => new ProductPromocode 
            {
                ProductId = t,
                PromocodeId = promocode.Id
            }).ToList();

            await _context.ProductPromocode.AddRangeAsync(productPromocodes, cancellationToken);
        }

        await _context.Promocodes.AddAsync(promocode, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        BackgroundJob.Schedule(() => RemoveDiscountAsync(promocode.Id, request.EndDate, request.StartDate), request.EndDate - DateTime.UtcNow);

        return Unit.Value;
    }

    public async Task RemoveDiscountAsync(Guid id, DateTime endDate, DateTime startDate)
    {
        var promocode = await _context.Promocodes.FirstOrDefaultAsync(t => t.Id == id);

        if (promocode is null)
            return;

        if (endDate != promocode.EndDate)
            return;

        if (startDate != promocode.StartDate)
            return;

        _context.Promocodes.Remove(promocode);
        await _context.SaveChangesAsync();
    }


}
