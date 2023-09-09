using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.SubCategories.DeleteSubCategory;

public class DeleteSubCategoryCommand : IRequest
{
    public Guid SubCategoryId { get; set; }

    public DeleteSubCategoryCommand(Guid id) => SubCategoryId = id; 
}
