﻿using System;
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
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(Sender,Password);

                var inlineLogo = new LinkedResource(HttpContext.Current.Server.MapPath("~/Picture/golf.png"));
                inlineLogo.ContentId = Guid.NewGuid().ToString();

                string body = string.Format(@"
                <p>{0}</p>
                <p>{1}</p>
                <img src=""cid:{2}"" />
                <p>Ha en trevlig dag</p>
                <p>Med vänliga hälsningar Hålslaget GK</p> 
                ", "Hej " + to.Firstname + " " + to.Lastname, to.TimestartTemp + " " + " " + Message, inlineLogo.ContentId);

                MailMessage mail = new MailMessage(Sender, to.Email, Subject, Message);
                mail.BodyEncoding = Encoding.UTF8;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                view.LinkedResources.Add(inlineLogo);
                mail.AlternateViews.Add(view);

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
    }
}