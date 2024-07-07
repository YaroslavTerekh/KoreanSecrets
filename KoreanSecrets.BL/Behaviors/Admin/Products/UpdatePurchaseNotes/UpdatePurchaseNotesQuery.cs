using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.Entities;
using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.UpdatePurchaseNotes;

public class UpdatePurchaseNotesQuery : IRequest
{
    public long Id { get; set; }

    public string Notes { get; set; }
}
