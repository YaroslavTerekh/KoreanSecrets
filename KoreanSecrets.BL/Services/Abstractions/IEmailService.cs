using KoreanSecrets.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Services.Abstractions;

public interface IEmailService
{
    public Task SendEmailAsync(Message message, string name);
}
