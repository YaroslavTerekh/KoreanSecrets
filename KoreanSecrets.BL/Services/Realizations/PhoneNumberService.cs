using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Services.Realizations;

public class PhoneNumberService : IPhoneNumberService
{
    public string FormatPhoneNumber(string phoneNumber)
    {
        if (phoneNumber.StartsWith('0'))
        {
            return String.Concat("+38", phoneNumber);
        } 
        else if (phoneNumber.StartsWith('3'))
        {
            return String.Concat("+", phoneNumber);
        } 
        else if (phoneNumber.StartsWith('+'))
        {
            return phoneNumber;
        }

        throw new Exception(ErrorMessages.WrongPhoneNumber);
    }
}
