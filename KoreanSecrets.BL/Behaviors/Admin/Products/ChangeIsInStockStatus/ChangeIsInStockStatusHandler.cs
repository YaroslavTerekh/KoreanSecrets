using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
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

namespace KoreanSecrets.BL.Behaviors.Admin.Products.ChangeIsInStockStatus;

public class ChangeIsInStockStatusHandler : IRequestHandler<ChangeIsInStockStatusCommand>
{
    private readonly DataContext _context;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _config;

    public ChangeIsInStockStatusHandler(DataContext context, IEmailService emailService, IConfiguration config)
    {
        _context = context;
        _emailService = emailService;
        _config = config;
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
                var message = new Message(volume.UsersWaitingForStock.Select(t => t.User.Email).ToArray(), 
                    "Товар в наявності!", String.Concat("Товар", volume.Product.Title, "з'явився у наявності!", _config.GetSection("HostSettings:FrontApplicationUrl"), "home/item/", volume.Product.Id));
        
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
