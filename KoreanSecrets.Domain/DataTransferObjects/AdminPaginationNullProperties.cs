using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class AdminPaginationNullProperties<T>
{
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }

    public int Total { get; set; }

    public List<T> Entities { get; set; }
}
