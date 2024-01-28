using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Charts.GetYearMoneyStatistic;

public class GetYearMoneyStatisticQuery : IRequest<ChartDTO>
{
}
