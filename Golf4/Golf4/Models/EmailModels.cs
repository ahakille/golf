using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Collections.Generic;
using Npgsql;
using System.Data;
using System.Web;

namespace Golf4.Models
{
    public abstract class EmailModels
    {
        public static void SendEmail(string Sender, string Password, List<MemberModels.MembersViewModel> To, string Subject, string Message)
        {
            string Emailsmtp = "";

            if (Sender.Contains("@gmail.com"))
            {
                Emailsmtp = "smtp.gmail.com";
            }

            else if (Sender.Contains("@yahoo.com"))
            {
                Emailsmtp = "smtp.mail.yahoo.com";
            }

            else
            {
                Emailsmtp = "smtp.live.com";
            }

            foreach (MemberModels.MembersViewModel to in To)
            {
                SmtpClient client = new SmtpClient(Emailsmtp, 587);
                MailMessage mail = null;

                if (to.Email.Contains("@nppc.se") || to.Email.Contains("@outlook.com"))
                {                    
                    client.EnableSsl = true;
                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(Sender, Password);

                    var inlineLogo = new LinkedResource(HttpContext.Current.Server.MapPath("~/Picture/golf.png"));
                    inlineLogo.ContentId = Guid.NewGuid().ToString();

                    string body = string.Format(@"
                    <p>{0}</p>
                    <p>{1}</p>
                    <img src=""cid:{2}"" />
                    <p>Ha en trevlig dag</p>
                    <p>Med vänliga hälsningar Hålslaget GK</p> 
                    ", "Hej " + to.Firstname + " " + to.Lastname, to.TimestartTemp + " " + " " + Message, inlineLogo.ContentId);

                    mail = new MailMessage(Sender, to.Email, Subject, Message);
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                    view.LinkedResources.Add(inlineLogo);
                    mail.AlternateViews.Add(view);
                }

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

        public static List<MemberModels.MembersViewModel> GetEmail(int id)
        {
            PostgresModels Database = new PostgresModels();
            DataTable table = Database.SqlQuery("SELECT firstname, lastname, timestart, email FROM balls INNER JOIN members ON members.id = balls.userid INNER JOIN reservations ON reservations.id = balls.reservationid WHERE reservationid = @id", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@id", id),
            });

            List<MemberModels.MembersViewModel> members = new List<MemberModels.MembersViewModel>();

            foreach (DataRow row in table.Rows)
            {
                MemberModels.MembersViewModel member = new MemberModels.MembersViewModel();
                member.Firstname = (string)row["firstname"];
                member.Lastname = (string)row["lastname"];
                member.TimestartTemp = (DateTime)row["timestart"];
                member.Email = (string)row["email"];
                members.Add(member);
            }

            return members;
        }

        public static List<MemberModels.MembersViewModel> GetEmail(DateTime Timestart, DateTime Timeend)
        {
            PostgresModels Database = new PostgresModels();
            DataTable table = Database.SqlQuery("SELECT firstname, lastname, timestart, email FROM balls INNER JOIN members ON members.id = balls.userid INNER JOIN reservations ON reservations.id = balls.reservationid WHERE timestart BETWEEN @timestart AND @timeend", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@timestart", Timestart),
                new NpgsqlParameter("@timeend", Timeend),
            });

            List<MemberModels.MembersViewModel> members = new List<MemberModels.MembersViewModel>();

            foreach (DataRow row in table.Rows)
            {
                MemberModels.MembersViewModel member = new MemberModels.MembersViewModel();
                member.Firstname = (string)row["firstname"];
                member.Lastname = (string)row["lastname"];
                member.TimestartTemp = (DateTime)row["timestart"];
                member.Email = (string)row["email"];
                members.Add(member);
            }

            return members;
        }

        public static List<MemberModels.MembersViewModel> GetEmailForReservations(int id)
        {
            PostgresModels Database = new PostgresModels();
            DataTable table = Database.SqlQuery("SELECT firstname, lastname, timestart, email FROM reservations INNER JOIN balls ON balls.reservationid = reservations.id INNER JOIN members ON members.id = balls.userid WHERE reservations.id = @id", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@id", id),
            });

            List<MemberModels.MembersViewModel> members = new List<MemberModels.MembersViewModel>();

            foreach (DataRow Row in table.Rows)
            {
                members.Add(new MemberModels.MembersViewModel()
                {
                    Firstname = (string)Row["firstname"],
                    Lastname = (string)Row["firstname"],
                    TimestartTemp = (DateTime)Row["timestart"],
                    Email = (string)Row["email"]
                });
            }

            return members;
        }

        public static List<MemberModels.MembersViewModel> GetEmailForContest(int id)
        {
            PostgresModels Database = new PostgresModels();
            DataTable table = Database.SqlQuery("SELECT firstname, lastname, timestart, email FROM members INNER JOIN players ON players.memberid = members.id INNER JOIN contests ON contests.id = players.contestid INNER JOIN reservations ON reservations.id = contests.reservationid WHERE contests.id = @id", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@id", id),
            });

            List<MemberModels.MembersViewModel> members = new List<MemberModels.MembersViewModel>();

            foreach (DataRow Row in table.Rows)
            {
                members.Add(new MemberModels.MembersViewModel()
                {
                    Firstname = (string)Row["firstname"],
                    Lastname = (string)Row["firstname"],
                    TimestartTemp = (DateTime)Row["timestart"],
                    Email = (string)Row["email"]
                });
            }

            return members;
        }
    }
}