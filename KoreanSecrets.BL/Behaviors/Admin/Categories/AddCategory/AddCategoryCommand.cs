using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Categories.AddCategory;

public class AddCategoryCommand : IRequest
{
    public string Title { get; set; }
}
