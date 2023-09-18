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
        var product = await _context.Products
            .Include(t => t.UsersWaitingForStock)
            .FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        product.IsInStock = !product.IsInStock;

        if (product.IsInStock)
        {
            var message = new Message(product.UsersWaitingForStock.Select(t => t.Email).ToArray(), "Товар в наявності!", product.Title);

            await _emailService.SendEmailAsync(message, "Товар в наявності");

            product.UsersWaitingForStock.Clear();
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
