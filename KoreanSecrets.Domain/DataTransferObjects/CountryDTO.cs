using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class CountryDTO : BaseEntity
{
    public string Title { get; set; }

    public Guid CategoryId { get; set; }
}
