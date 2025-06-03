namespace AtecaAPI.Services
{

    public interface INotificationService
    {
        Task EnviarCorreoAsync(string destinatario, string asunto, string mensajeHtml);
    }
}
