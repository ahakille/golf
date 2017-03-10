using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Golf4.Models
{
    public abstract class EmailModels
    {
        public static void skickaEmail(string Sender, string Password, string To, string Subject, string Message)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(Sender,Password);

            MailMessage mail = new MailMessage(Sender,To,Subject,Message);
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