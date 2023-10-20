using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class CategoryCountry
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public Category Category { get; set; }

    public Guid CountryId { get; set; }

    public Country Country { get; set; }
}
