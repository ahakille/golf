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
        public double TotalHCP { get; set; } = 0;

        public static void RemoveReservation(int user_id, int reservationID)
        {
            PostgresModels Database = new PostgresModels();
            DataTable Table = Database.SqlQuery("SELECT id, user_id FROM reservations WHERE user_id = @user_id AND id = @reservationid", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@user_id", user_id),
                new NpgsqlParameter("@reservationid", reservationID),
            });

            if (Table.Rows.Count != 0)
            {
                Database = new PostgresModels();
                Database.SqlNonQuery("DELETE FROM balls WHERE reservationid = @reservationid; DELETE FROM reservations WHERE id = @id", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@id", user_id),
                    new NpgsqlParameter("@reservationid", reservationID),
                    
                });
            }

            else
            {
                Database = new PostgresModels();
                Database.SqlNonQuery("DELETE FROM balls WHERE userid = @id", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@id", user_id),
                });
            }
        }
        public class MakeBooking
        {
            public int MakeReservations(DateTime timestart, DateTime timeend, bool closed, int id_user)
            {
                int id_reservation =0 ;
                PostgresModels Database = new PostgresModels();
                DataTable dt = Database.SqlQuery("INSERT INTO reservations(timestart, timeend, closed, user_id) VALUES(@timestart, @timeend, @closed, @user_id) returning id;", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@timestart", timestart),
                        new NpgsqlParameter("@timeend", timeend),
                        new NpgsqlParameter("@closed", closed),
                        new NpgsqlParameter("@user_id", id_user)
                        });
                foreach (DataRow dr in dt.Rows)
                {
                 id_reservation = (int)dr["id"];
                }

                return id_reservation;
            }
            public void MakeReservationBalls(int id_reservation , int id_user)
            {
                PostgresModels Database = new PostgresModels();
                Database.SqlNonQuery("INSERT INTO balls(userid, reservationid) VALUES(@user, @reservationid);", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@reservationid", id_reservation),
                        new NpgsqlParameter("@user", id_user),
                    });
            }
            public int CollectReservationId(DateTime chosendate)
            {
                int id_reservation = 0;
                PostgresModels Database = new PostgresModels();
                DataTable dt =Database.SqlQuery("SELECT reservations.id FROM reservations WHERE timestart = @chosendate", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                            new NpgsqlParameter("@chosendate", chosendate),
                        });
                foreach (DataRow dr in dt.Rows)
                {
                    id_reservation = (int)dr["id"];
                }
                return id_reservation;
            }
            public void DeleteBoll(int reservation_id, int user_id)
            {
                PostgresModels Database = new PostgresModels();
                Database.SqlNonQuery("DELETE FROM balls WHERE reservationid = @reservationid AND userid = @userid", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@reservationid", reservation_id),
                        new NpgsqlParameter("@userid", user_id)
                        });
            }
            public void DeleteReservation(int reservation_id)
            {
                PostgresModels Database = new PostgresModels();
                Database.SqlNonQuery("DELETE FROM balls WHERE reservationid = @reservationid; DELETE FROM reservations WHERE id = @reservationid", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@reservationid", reservation_id),
            });
            }
            public Tuple<bool,int> CheckReservationUser(DateTime timestart, int user_id)
            {
                bool check = false;
                int user_id_reservation =0, id=0;
                PostgresModels Database = new PostgresModels();
                DataTable dt =Database.SqlQuery("SELECT user_id, id FROM Reservations WHERE timestart=@timestart AND user_id = @userid", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@timestart", timestart),
                        new NpgsqlParameter("@userid", user_id)
                        });
                foreach (DataRow dr in dt.Rows)
                {
                    id = (int)dr["id"];
                    user_id_reservation = (int)dr["user_id"];
                }
                if (user_id == user_id_reservation)
                {
                    check = true;
                }
                return Tuple.Create(check, id);
            }
        }


        public class CloseGolfCourseView
        {
            public DateTime ClosingStartDate { get; set; }
            public DateTime ClosingStopDate { get; set; }
            public DateTime ClosingStartTime { get; set; }
            public DateTime ClosingStopTime { get; set; }

            public static void CloseCourse(DateTime Timestart, DateTime Timeend)
            {
                PostgresModels Database = new PostgresModels();
                DataTable Table = Database.SqlQuery("SELECT * FROM reservations WHERE timestart BETWEEN @timestart AND @timeend ORDER BY timestart", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@timestart", Timestart),
                    new NpgsqlParameter("@timeend", Timeend.AddDays(1)),
                });

                foreach (DataRow row in Table.Rows)
                {
                    Database.SqlQuery("UPDATE reservations SET closed='TRUE' WHERE id = @id", PostgresModels.list = new List<NpgsqlParameter>()
                    {
                        new NpgsqlParameter("@id",(int)row["id"]),                        
                    });
                }
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
            public int CountGolfers { get; set; }
            [Display(Name = "Totalt handikapp i bollen")]
            public double TotalHCP { get; set; }
        }
        public class AdminViewModel
        {
            public int ID { get; set; }
            public DataTable medlemmar { get; set; }
            public DataTable reservation { get; set; }
            [Display(Name = "Datum och starttid")]
            public DateTime Timestart { get; set; }
            public DateTime Timeend { get; set; }
            public bool Closed { get; set; } = false;
            
        }
    }
    
}