using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Common.CustomExceptions;

public class DeleteException : Exception
{
    public string Description { get; set; }

    public DeleteException(string description = "Помилка видалення")
    {
        Description = description;
    }
}
