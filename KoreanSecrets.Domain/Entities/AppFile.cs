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

    public Guid ProductId { get; set; }

    public Product Product { get; set; }
}
