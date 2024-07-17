using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.ChangeIsInStockStatus;

public class ChangeIsInStockStatusHandler : IRequestHandler<ChangeIsInStockStatusCommand>
{
    private readonly DataContext _context;
    private readonly IEmailService _emailService;

    public ChangeIsInStockStatusHandler(DataContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<Unit> Handle(ChangeIsInStockStatusCommand request, CancellationToken cancellationToken)
    {
        var volume = await _context.Volume
            .Include(t => t.UsersWaitingForStock)
                .ThenInclude(t => t.User)
            .Include(t => t.UsersWaitingForStock)
                .ThenInclude(t => t.Volume)
                    .ThenInclude(t => t.Product)
            .FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (volume is null)
            throw new NotFoundException(ErrorMessages.VolumeNotFound);

        volume.IsInStock = !volume.IsInStock;

        await _context.SaveChangesAsync(cancellationToken);
        
        try
        {
            if (volume.IsInStock)
            {
                var message = new Message(volume.UsersWaitingForStock.Select(t => t.User.Email).ToArray(), "Товар в наявності!", volume.Product.Title);
        
                await _emailService.SendEmailAsync(message, "Товар в наявності");
        
                volume.UsersWaitingForStock.Clear();
                
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return Unit.Value;
    }
}
