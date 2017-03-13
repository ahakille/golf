using System;
using System.ComponentModel.DataAnnotations;
using Npgsql;
using System.Data;
using System.Collections.Generic;
using System.Linq;

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
        public bool CheckedIn { get; set; } = false;

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
                Database.SqlNonQuery("INSERT INTO balls(userid, reservationid, checkedin) VALUES(@user, @reservationid, FALSE);", PostgresModels.list = new List<NpgsqlParameter>()
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
        public class CheckInMember
        {
            // Metod som checkar in alla medlemmar i samma reservation(tid) eller endast en!
            public void CheckInAllMember(DateTime timestart, int userid)
            {
                string sqlfråga;
                if (userid == 0)
                {
                   sqlfråga = "SELECT userid, balls.reservationid FROM balls JOIN reservations ON balls.reservationid = reservations.id WHERE reservations.timestart = @timestart";
                }
                else
                {
                    sqlfråga = "SELECT userid, balls.reservationid FROM balls JOIN reservations ON balls.reservationid = reservations.id WHERE reservations.timestart = @timestart AND userid = @user_id";
                }
                PostgresModels sql = new PostgresModels();
                DataTable dt= sql.SqlQuery(sqlfråga, PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@timestart", timestart),
                        new NpgsqlParameter("@user_id", userid)
                        });
                foreach (DataRow dr in dt.Rows)
                {
                   int user_id = (int)dr["userid"];
                   int balls_id = (int)dr["reservationid"];
                    CheckInMembers(user_id, balls_id);
                }
            }
            //metod som checkar in medlmemen! 
            public void CheckInMembers(int user_id, int balls_id)
            {
                PostgresModels sql = new PostgresModels();
                sql.SqlNonQuery("UPDATE balls set checkedin = TRUE WHERE userid = @userid AND reservationid=@reservatuinid", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@userid", user_id),
                new NpgsqlParameter("@reservatuinid", balls_id)
            });
            }

            public List<MemberModels.MembersViewModel> GetMembersInReservation(DateTime timestart)
            {
                
                List<MemberModels.MembersViewModel> list = new List<MemberModels.MembersViewModel>();
                PostgresModels sql = new PostgresModels();
                DataTable dt= sql.SqlQuery("SELECT  members.golfid , members.firstname , members.lastname , members.email, members.telephone, members.hcp, genders.gender, membercategories.category , members.id,balls.checkedin,reservations.timestart FROM reservations JOIN balls ON balls.reservationid = reservations.id JOIN members ON balls.userid = members.id LEFT JOIN genders ON members.gender = genders.id  LEFT JOIN membercategories ON members.membercategory = membercategories.id WHERE reservations.timestart = @timestart", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@timestart", timestart),
                        });
                foreach (DataRow item in dt.Rows)
                {
                    MemberModels.MembersViewModel model = new MemberModels.MembersViewModel();
                    model.GolfID=(string)item["golfid"];
                    model.Firstname = (string)item["firstname"];
                    model.Lastname = (string)item["lastname"];
                    model.HCP = (double)item["hcp"];
                    model.Gender = (string)item["gender"];
                    model.Membercategory = (string)item["category"];
                    model.Email = (string)item["email"];
                    model.Telephone = (string)item["telephone"];
                    model.timestart = (DateTime)item["timestart"];
                    model.ID = (int)item["id"];
                    
                    bool check = (bool)item["checkedin"];
                    if (!check)
                    {
                     model.CheckedIn = item["id"].ToString();
                    }
                    else
                    {
                        model.CheckedIn = "0";
                    }
                    list.Add(model);
                }
                return list;
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
            public bool Guest { get; set; } = false;
            [Display(Name = "Antal... bollar?")]
            public int CountGolfers { get; set; }
            [Display(Name = "Totalt handikapp i bollen")]
            public double TotalHCP { get; set; }
        }
        public class AdminViewModel
        {
            public int ReservationID { get; set; }
            public string CheckedIn { get; set; }
            public int ID { get; set; }            
            public string Firstname { get; set; }           
            public string Lastname { get; set; }           
            public string Adress { get; set; }            
            public string Postalcode { get; set; }           
            public string City { get; set; }           
            public string Email { get; set; }
            public string Telephone { get; set; }
            public string Gender { get; set; }          
            public double HCP { get; set; }           
            public string GolfID { get; set; }            
            public string Membercategory { get; set; }          
            public DataTable medlemmar { get; set; }
            public DataTable reservation { get; set; }
            [Display(Name ="Datum och Tid")]
            public DateTime Timestart { get; set; }
            public DateTime Timeend { get; set; }
            public bool Closed { get; set; } = false;
            
        }
    }
    
}