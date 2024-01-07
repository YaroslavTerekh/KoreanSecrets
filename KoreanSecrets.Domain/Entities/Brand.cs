using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class Brand : BaseEntity
{
    public string Title { get; set; }

    public Guid PhotoId { get; set; }

    public AppFile Photo { get; set; }

    public List<Product> Products { get; set; } = new();
}
