using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Azure.Communication.Email;
using Azure.Communication.Email.Models;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;


namespace FunctionApp.Services
{
    public class SendEmaisService : ISendEmaisService
    {

        public ResponseEmailDto SendEmail(string receiverEmail, string uri)
        {
            var email = new MimeMessage();
            var responseDto = new ResponseEmailDto();

            if (receiverEmail is null)
            {
                // need 
                responseDto.IsSent = false;
                responseDto.EmailTo = receiverEmail;
            }

            email.From.Add(new MailboxAddress("File Upload Service", "xiaomiredmix4good@gmail.com"));
            email.To.Add(new MailboxAddress("Receiver Name", receiverEmail));

            email.Subject = "Testing out email sending";

            // Create the HTML body with the URI included
            string bodyHtml = $"<b>Hello, you successfully upload file to Azure Storage! </b><br><a href=\"{uri}\">Click here to access the resource</a>";

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = bodyHtml
            };

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.Connect("smtp-relay.brevo.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate("xiaomiredmix4good@gmail.com", "g5PnTIAWdDO1zYrJ");

                smtp.MessageSent += (sender, args) =>
                {
                    responseDto.IsSent = true;
                    responseDto.EmailTo = receiverEmail;
                };


                smtp.Send(email);
                smtp.Disconnect(true);
            }



            return responseDto;
           
        }
    }
}
