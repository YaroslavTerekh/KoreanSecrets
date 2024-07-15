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
    public long? TemporaryCode { get; set; }

    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public Guid? AddressInfoId { get; set; }

    public AddressInfo? AddressInfo { get; set; }

    public Guid BucketId { get; set; }

    public Bucket Bucket { get; set; }

    public Guid? AdminBucketId { get; set; }

    public AdminBucket? AdminBucket { get; set; }

    public List<Feedback> Feedbacks { get; set; } = new();

    public List<FeedbackReply> FeedbackReplies { get; set; } = new();

    public List<ProductUser> Likes { get; set; } = new();
    
    public List<Product> ProductsWaitingForStock { get; set; } = new();

    public List<VolumeUser> VolumesWaitingForStock { get; set; } = new();

    public List<Purchase> Purchases { get; set; } = new();

    public List<Report> Reports { get; set; } = new();
}
