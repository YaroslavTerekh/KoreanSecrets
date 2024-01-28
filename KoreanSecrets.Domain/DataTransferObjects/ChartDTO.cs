using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class ChartDTO
{
    public string TotalResult { get; set; }

    public object ChartInfo { get; set; }
}
