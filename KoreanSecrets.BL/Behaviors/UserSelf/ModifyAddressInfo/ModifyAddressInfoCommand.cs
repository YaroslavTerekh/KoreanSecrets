using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.ModifyAddressInfo;

public class ModifyAddressInfoCommand : IAuthorizedRequest
{
    public string City { get; set; }

    public string Warehouse { get; set; }
}
