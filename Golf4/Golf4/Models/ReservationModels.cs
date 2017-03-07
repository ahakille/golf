using System;
using System.ComponentModel.DataAnnotations;
using Npgsql;
using System.Data;
using System.Collections.Generic;

namespace Golf4.Models
{
    public class ReservationModels
    {
        public int ID { get; set; } = 0;
        public int MemberID { get; set; } = 0;
        public string MemberGolfID { get; set; } = "";
        public double MemberHCP { get; set; } = 0;
        public int MemberGender { get; set; } = 0;
        public DateTime Timestart { get; set; }
        public DateTime Timeend { get; set; }
        public bool Closed { get; set; } = false;
        public int User { get; set; } = 0;
        public string datepicker { get; set; } = "";
        public int ReservationID { get; set; } = 0;

        public static void RemoveReservation(ReservationModels reservation)
        {
            PostgresModels Database = new PostgresModels();
            Database.SqlNonQuery("DELETE FROM balls WHERE id = @id", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@id", reservation.MemberID),
            });

            Database = new PostgresModels();
            DataTable table = Database.SqlQuery("SELECT user_id, reservationid FROM reservations WHERE date(timestart) = @timestart", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@timestart", reservation.Timestart),
            });

            int? ID = Convert.ToInt16(table.Rows[0]["user_id"]);
            reservation.ReservationID = Convert.ToInt16(table.Rows[0]["reservationid"]);

            if (ID != null)
            {
                Database = new PostgresModels();
                Database.SqlNonQuery("Delete FROM balls WHERE reservationid = @reservationid; DELETE FROM reservations WHERE user-id = @id", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@id", reservation.MemberID),
                    new NpgsqlParameter("@reservationid", reservation.ReservationID),
                });
            }            
        }


        public class CloseGolfCourseView
        {
            public DateTime ClosingStartDate { get; set; }
            public DateTime ClosingStopDate { get; set; }
            public DateTime ClosingStartTime { get; set; }
            public DateTime ClosingStopTime { get; set; }
        }
        public class CreatereservationModel
        {
            public int ID { get; set; }
            [Display(Name = "Förnamn")]
            public string Firstname { get; set; }
            [Display(Name = "Efternamn")]
            public string Lastname { get; set; }
            [Display(Name = "Kön")]
            public string Gender { get; set; }
            [Display(Name = "HCP")]
            public double HCP { get; set; }
            [Display(Name = "Golfid")]
            public string GolfID { get; set; }
            [Display(Name ="Datum och tid")]
            public DateTime Timestart { get; set; }
            public DateTime Timeend { get; set; }
            public bool Closed { get; set; } = false;
            public int User { get; set; } 
        }
    }
    
}