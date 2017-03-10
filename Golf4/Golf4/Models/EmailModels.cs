using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Collections.Generic;

namespace Golf4.Models
{
    public abstract class EmailModels
    {
        public static void skickaEmail(string Sender, string Password, List<string> To, string Subject, string Message)
        {
            foreach (string to in To)
            {
                string Emailsmtp = "";

                if (to.Contains(".gmail.com"))
                {
                    Emailsmtp = "smtp.gmail.com";
                }

                else if (to.Contains(".yahoo.com"))
                {
                    Emailsmtp = "smtp.mail.yahoo.com";
                }

                else
                {
                    Emailsmtp = "smtp.live.com";
                }

                SmtpClient client = new SmtpClient(Emailsmtp, 587);
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(Sender,Password);

                MailMessage mail = new MailMessage(Sender, to, Subject, Message);
                mail.BodyEncoding = Encoding.UTF8;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                try
                {
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
        }
    }
}