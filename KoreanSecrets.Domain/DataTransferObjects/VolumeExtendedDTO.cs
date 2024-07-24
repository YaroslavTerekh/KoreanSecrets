using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class VolumeExtendedDTO
{
    public Guid Id { get; set; }
    public string Value { get; set; }
    public string Unit { get; set; }
    public ListProductDTO Product { get; set; }
    public decimal Price { get; set; }
    public decimal? PriceWithDiscount { get; set; }
    public int Quantity { get; set; }
    public List<AppFileDTO> Photos { get; set; }
    public List<VolumeUserDTO> UsersWaitingForStock { get; set; }
    public DateTime CreatedDate { get; set; }
}