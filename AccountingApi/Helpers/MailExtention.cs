using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EOfficeAPI.Helpers
{
    public static class MailExtention
    {

        public static void Send(string subject, string body, string email)
        {
            var message = new MailMessage();

            message.To.Add(new MailAddress(email)); // replace with valid value

            message.From = new MailAddress("elchin@codeveloper.az"); // replace with valid value

            message.Subject = subject;

            message.Body = body;

            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())

            {

                var credential = new NetworkCredential

                {

                    UserName = "elchin@codeveloper.az",
                    Password = "elchin1998"
                };

                smtp.Credentials = credential;

                smtp.Host = "smtp.yandex.com";

                //smtp.Host = "smtp.live.com";

                smtp.Port = 587;

                smtp.EnableSsl = true;

                smtp.Send(message);

                return;
            }

        }
    }

}