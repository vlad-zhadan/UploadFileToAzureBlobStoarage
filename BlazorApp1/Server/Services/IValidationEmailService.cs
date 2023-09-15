using GemBox.Email;

namespace BlazorApp1.Server.Services
{
    public interface IValidationEmailService
    {
        public bool IsValidateEmail(string email);

    }
}