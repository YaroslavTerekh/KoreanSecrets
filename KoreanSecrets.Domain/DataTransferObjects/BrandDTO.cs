using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class BrandDTO : BaseEntity
{
    public string Title { get; set; }

    public AppFileDTO Photo { get; set; }
}