using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Services.Abstractions;

public interface IPhoneNumberService
{
    public string FormatPhoneNumber(string phoneNumber);
}
