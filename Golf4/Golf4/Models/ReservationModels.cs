﻿using System;
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
        
        public static void RemoveReservation(ReservationModels reservation)
        {           
            PostgresModels Database = new PostgresModels();
            DataTable Table = Database.SqlQuery("SELECT id, user_id FROM reservations WHERE timestart = @timestart", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@timestart", reservation.Timestart),
            });

            int? ID = Convert.ToInt16(Table.Rows[0]["user_id"]);
            reservation.ID = Convert.ToInt16(Table.Rows[0]["id"]);

            if (ID != null)
            {
                Database = new PostgresModels();
                Database.SqlNonQuery("DELETE FROM balls WHERE id = @reservationid; DELETE FROM reservations WHERE id = @id", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@reservationid", reservation.ID),
                    new NpgsqlParameter("@id", reservation.MemberID),                    
                });
            }

            else
            {
                Database = new PostgresModels();
                Database.SqlNonQuery("DELETE FROM balls WHERE id = @id", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@id", reservation.MemberID),
                });
            }
        }

        public class CloseGolfCourseView
        {
            public DateTime ClosingStartDate { get; set; }
            public DateTime ClosingStopDate { get; set; }
            public DateTime ClosingStartTime { get; set; }
            public DateTime ClosingStopTime { get; set; }

            public static void CloseCourse(ReservationModels reservation)
            {
                PostgresModels Database = new PostgresModels();
                DataTable Table = Database.SqlQuery("", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@id", reservation.MemberID),
                });



            }

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
            [Display(Name = "Golfid2")]
            public string GolfID2 { get; set; }
            [Display(Name = "Golfid3")]
            public string GolfID3 { get; set; }
            [Display(Name = "Golfid4")]
            public string GolfID4 { get; set; }
            [Display(Name ="Datum och tid")]
            public DateTime Timestart { get; set; }
            public DateTime Timeend { get; set; }
            public bool Closed { get; set; } = false;
            public int User { get; set; }
            [Display(Name = "Spelaren är en gäst")]
            public bool Guest { get; set; }
            [Display(Name = "Antal... bollar?")]
            public int Countballs { get; set; }
        }
        public class AdminViewModel
        {
            public int ID { get; set; }
            public DataTable medlemmar { get; set; }
            public DataTable reservation { get; set; }
            public DateTime Timestart { get; set; }
            public DateTime Timeend { get; set; }
            public bool Closed { get; set; } = false;
            
        }
    }
    
}