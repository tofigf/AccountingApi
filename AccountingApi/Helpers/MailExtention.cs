using System;
using System.Collections.Generic;
using System.IO;
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

            message.From = new MailAddress("smb@eoffice.az"); // replace with valid value

            message.Subject = subject;

            message.Body = body;

            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())

            {

                var credential = new NetworkCredential

                {

                    UserName = "smb@eoffice.az",
                    Password = "8HVbedWq9B2HMXyP"
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
        public static void SendPasswordEmail(string email, string token)
        {
            //calling for creating the email body with html template   

            string body = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/Templates/PasswordChange.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{token}}", token); //replacing the required things  

            Send("eOffice xoşgəldiniz", body, email);
            return;
        }
    }

}