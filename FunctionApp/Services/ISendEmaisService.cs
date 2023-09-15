using System;
using System.Threading.Tasks;

namespace FunctionApp.Services
{
    public interface ISendEmaisService
    {
        public ResponseEmailDto SendEmail(string receiverEmail, string uri);
    }
}