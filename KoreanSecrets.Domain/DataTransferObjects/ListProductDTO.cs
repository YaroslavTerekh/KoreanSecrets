using KoreanSecrets.Domain.Entities;
using KoreanSecrets.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class ListProductDTO : BaseEntity
{
    public string Title { get; set; }

    public long Price { get; set; }

    public Guid MainPhotoId { get; set; }

    public AppFileDTO MainPhoto { get; set; }

    public Guid BrandId { get; set; }

    public BrandDTO Brand { get; set; }

    public ProductIcon Icon { get; set; }
}
