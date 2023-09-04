using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class AppFileDTO : BaseEntity
{
    public string FileName { get; set; }

    public string FileExtension { get; set; }

    public string FilePath { get; set; }
}
