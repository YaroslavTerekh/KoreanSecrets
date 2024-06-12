using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.DeleteProductFeedback;

public class DeleteProductFeedbackCommand : IRequest
{
    public Guid FeedbackId { get; set; }

    public DeleteProductFeedbackCommand(Guid id) => FeedbackId = id;
}
