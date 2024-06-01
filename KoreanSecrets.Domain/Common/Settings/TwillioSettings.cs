using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Common.Settings;

public class TwillioSettings
{
    public string AccountSid { get; set; }

    public string AuthToken { get; set; }

    public string FromPhoneNumber { get; set; }
}
