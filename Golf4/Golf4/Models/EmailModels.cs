using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Collections.Generic;
using Npgsql;
using System.Data;

namespace Golf4.Models
{
    public abstract class EmailModels
    {
        public static void SendEmail(string Sender, string Password, List<string> To, string Subject, string Message)
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

        public static List<string> GetEmail(DateTime Timestart, DateTime Timeend)
        {
            PostgresModels Database = new PostgresModels();
            DataTable table = Database.SqlQuery("SELECT email FROM balls INNER JOIN members ON members.id = balls.userid WHERE reservationid IN (SELECT id FROM reservations WHERE timestart BETWEEN @timestart AND @timeend)", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@timestart", Timestart),
                new NpgsqlParameter("@timeend", Timeend),
            });

            List<string> emails = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                emails.Add(row["email"].ToString());
            }

            return emails;
        }

    }
}