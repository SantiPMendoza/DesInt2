using SendGrid.Helpers.Mail;
using SendGrid;

namespace AtecaAPI.Services
{


    public class NotificationService : INotificationService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public NotificationService(IConfiguration config)
        {
            _apiKey = config["SendGrid:ApiKey"];
            _fromEmail = config["SendGrid:FromEmail"];
            _fromName = config["SendGrid:FromName"];
        }

        public async Task EnviarCorreoAsync(string destinatario, string asunto, string mensajeHtml)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(destinatario);
            var msg = MailHelper.CreateSingleEmail(from, to, asunto, plainTextContent: null, htmlContent: mensajeHtml);
            await client.SendEmailAsync(msg);
        }
    }
}
