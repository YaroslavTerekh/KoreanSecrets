using KoreanSecrets.Domain.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Demands.GetDemands;

public class GetDemandsQuery : IAuthorizedRequest<List<DemandDTO>>
{
}
