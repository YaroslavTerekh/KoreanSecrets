using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Common.CustomExceptions;

public class NotFoundException : Exception
{
    public string Description { get; set; }

    public NotFoundException(string description = "Не знайдено")
    {
        Description = description;
    }
}

