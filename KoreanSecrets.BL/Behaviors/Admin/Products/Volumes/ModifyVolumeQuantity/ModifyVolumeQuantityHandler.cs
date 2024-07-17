using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.ModifyVolumeQuantity;

public class ModifyVolumeQuantityHandler : IRequestHandler<ModifyVolumeQuantityCommand>
{
    private readonly DataContext _context;
    private readonly IEmailService _emailService;

    public ModifyVolumeQuantityHandler(DataContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<Unit> Handle(ModifyVolumeQuantityCommand request, CancellationToken cancellationToken)
    {
        var volume = await _context.Volume
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (volume is null) throw new Exception(ErrorMessages.ProductNotFound("Об'єкту об'єму для модифікації"));
        
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
}
