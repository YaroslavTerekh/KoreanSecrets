using KoreanSecrets.Domain.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public Guid? AddressInfoId { get; set; }

    public AddressInfo? AddressInfo { get; set; }

    public Guid BucketId { get; set; }

    public Bucket Bucket { get; set; }

    public List<Feedback> Feedbacks { get; set; } = new();

    public List<Product> Likes { get; set; } = new();

    public List<Product> ProductsWaitingForStock { get; set; } = new();

    public List<Purchase> Purchases { get; set; } = new();
}
