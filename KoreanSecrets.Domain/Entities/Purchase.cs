using KoreanSecrets.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class Purchase : BaseEntity
{
    public long PurchaseIdentifier { get; set; }

    public string City { get; set; }

    public string Warehouse { get; set; }

    public Guid? UserId { get; set; }

    public User? User { get; set; }    

    public PayType PayType { get; set; }

    public PurchaseStatus PurchaseStatus { get; set; }

    public PurchaseGenerateBy GeneratedBy { get; set; }

    public string Comment { get; set; }

    public List<PurchasedProduct> Products { get; set; }

    public decimal TotalPrice { get; set; }

    public Guid? PromocodeId { get; set; }

    public Promocode Promocode { get; set; }

    public string UserInfo { get; set; }
    
    public string Phone { get; set; }
    
    public string? Email { get; set; }
    
    public string? AdminNotes { get; set; }
    
    public DateTime? PaidDate { get; set; }
}
