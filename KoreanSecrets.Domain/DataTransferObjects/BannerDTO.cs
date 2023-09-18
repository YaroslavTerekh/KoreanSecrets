using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoreanSecrets.Domain.Entities;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class BannerDTO : BaseEntity
{
    public string Text { get; set; }

    public AppFileDTO BannerPhoto { get; set; }

    public Guid ProductId { get; set; }
}
