using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class Brand : BaseEntity
{
    public string Title { get; set; }

    public List<Product> Products { get; set; } = new();

    public Guid CategoryId { get; set; }

    public Category Category { get; set; }
}
