﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Common.Settings;

public class LiqPaySettings
{
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
    public string ServerUrl { get; set; }
}
