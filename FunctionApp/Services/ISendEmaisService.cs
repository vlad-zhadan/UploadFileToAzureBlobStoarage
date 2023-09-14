using System;
using System.Threading.Tasks;

namespace FunctionApp.Services
{
    public interface ISendEmaisService
    {
        public void SendEmail(string receiverEmail, string uri);
    }
}