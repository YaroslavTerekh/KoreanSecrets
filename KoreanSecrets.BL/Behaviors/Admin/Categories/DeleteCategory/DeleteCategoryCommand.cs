using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Categories.DeleteCategory;

public class DeleteCategoryCommand : IRequest
{
    public Guid CategoryId { get; set; }

    public DeleteCategoryCommand(Guid id) => CategoryId = id; 
}
