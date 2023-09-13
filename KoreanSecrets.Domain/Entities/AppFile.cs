using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class AppFile : BaseEntity
{
    public string FileName { get; set; }

    public string FileExtension { get; set; }

    public string FilePath { get; set; }

    public Guid? ProductId { get; set; }

    public Product Product { get; set; }

    public Guid? ProductPhotoId { get; set; }

    public Product ProductPhoto { get; set; }

    public Guid? ProductMainPhotoId { get; set; }

    public Product ProductMainPhoto { get; set; }

    public Guid? BrandPhotoId { get; set; }

    public Brand BrandPhoto { get; set; }

    public Guid? BannerId { get; set; }

    public Banner Banner { get; set; }
}
