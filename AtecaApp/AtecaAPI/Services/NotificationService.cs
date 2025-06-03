using SendGrid.Helpers.Mail;
using SendGrid;

namespace AtecaAPI.Services
{


    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AtecaAPI.Services.INotificationService" />
    public class NotificationService : INotificationService
    {
        /// <summary>
        /// The API key
        /// </summary>
        private readonly string _apiKey;
        /// <summary>
        /// From email
        /// </summary>
        private readonly string _fromEmail;
        /// <summary>
        /// From name
        /// </summary>
        private readonly string _fromName;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public NotificationService(IConfiguration config)
        {
            _apiKey = config["SendGrid:ApiKey"];
            _fromEmail = config["SendGrid:FromEmail"];
            _fromName = config["SendGrid:FromName"];
        }

        /// <summary>
        /// Enviars the correo asynchronous.
        /// </summary>
        /// <param name="destinatario">The destinatario.</param>
        /// <param name="asunto">The asunto.</param>
        /// <param name="mensajeHtml">The mensaje HTML.</param>
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
