using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using api_rest.DTOs;
using api_rest.Interfaces;


namespace api_rest.Models
{
    public class MailJet : IEmail
    {
        public async Task Send(string emailAddress, string body, EmailOptionsDTO options)
        {
            var client = new SmtpClient();
            client.Host = options.Host;
            client.Credentials = new NetworkCredential(options.APIKey, options.APIKeySecret);
            client.Port = options.Port;

            var message = new MailMessage(options.SenderEmail, emailAddress);
            message.Body = body;
            message.IsBodyHtml = true;

            await client.SendMailAsync(message);
        }
    }
}