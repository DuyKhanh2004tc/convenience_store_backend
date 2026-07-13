using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace ConvenienceStore.Application.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");
        var host = emailSettings["SmtpHost"];
        var port = int.Parse(emailSettings["SmtpPort"] ?? "587");
        var user = emailSettings["SmtpUser"];
        var pass = emailSettings["SmtpPass"];
        var fromName = emailSettings["FromName"];

        using var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(user, pass),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(user!, fromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(to);

        await client.SendMailAsync(mailMessage);
    }
}
