using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Settings;
using KoreanSecrets.Domain.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Services.Realizations;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfig;

    public EmailService(EmailConfiguration emailConfig)
    {
        _emailConfig = emailConfig;
    }
    public async Task SendEmailAsync(Message message, string name)
    {
        var emailMessage = CreateEmailMessage(message, name);
        await SendAsync(emailMessage);
    }

    private MimeMessage CreateEmailMessage(Message message, string name)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(name, _emailConfig.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
        return emailMessage;
    }
    private async Task SendAsync(MimeMessage mailMessage, CancellationToken cancellationToken = default)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true, cancellationToken);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password, cancellationToken);
                await client.SendAsync(mailMessage, cancellationToken);
            }
            catch
            {
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}