namespace ProductAPI.Models
{
    public interface IEMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
