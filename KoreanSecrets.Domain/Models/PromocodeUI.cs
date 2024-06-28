using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Models;

public class PromocodeUI
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public decimal Discount { get; set; }

    public decimal Total { get; set; }
}
