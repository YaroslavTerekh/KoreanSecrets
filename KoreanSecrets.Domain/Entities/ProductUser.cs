using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class ProductUser
{
    public Guid LikesId { get; set; }

    public Product Likes { get; set; }

    public Guid LikesId1 { get; set; }

    public User Likes1 { get; set; }
}
