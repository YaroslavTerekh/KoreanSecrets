using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class Banner : BaseEntity
{
    public string Text { get; set; }

    public Guid BannerPhotoId { get; set; }

    public AppFile BannerPhoto { get; set; }

    public Guid ProductId { get; set; }

    public Product Product { get; set; }
}
