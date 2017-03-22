using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data;
using Npgsql;

namespace Golf4.Models
{
    public class ContestModels
    {
        public int ContestID { get; set; }
        [Required]
        [Display(Name = "Tävlingsnamnet")]
        public string Name { get; set; }
        [Display(Name = "Beskrivning")]
        public string description { get; set; }
        [Required]
        [Display(Name = "Startdatum och tid")]
        public DateTime Timestart { get; set; }
        [Required]
        [Display(Name = "Slutdatum och tid")]
        public DateTime Timeend { get; set; }
        [Required]
        [Display(Name = "Sista anmälningsdag")]
        public DateTime CloseTime { get; set; }
        [Required]
        [Display(Name = "Max antal spelare")]
        public int MaxPlayers { get; set; }
        public bool Publish { get; set; }
        public int Reservation_id { get; set; }
        public DataTable AllContests { get; set; }
        public DataTable ContestMembers { get; set; }
        [Display(Name = "Tävlingsnamn och datum")]
        public string NameAndDate { get; set; }
        public DataTable ViewResultList { get; set; }
        public DataTable PublishedContests { get; set; }

        public class MakeCompetition
        {
            public void Createcontest(int user, string name, DateTime start, DateTime end, DateTime close, int maxplayer, string description)
            {
                ReservationModels.MakeBooking makebooking = new ReservationModels.MakeBooking();
             
                int reservation_id = makebooking.MakeReservations(start, end, false, true, user);
                PostgresModels sql = new PostgresModels();

                sql.SqlNonQuery("INSERT INTO contests(name, closetime, maxplayers, publish, reservationid, description) VALUES(@name, @closetime, @maxplayers, FALSE, @reservationid, @description);", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@name", name),
                        new NpgsqlParameter("@closetime", close),
                        new NpgsqlParameter("@maxplayers", maxplayer),
                        new NpgsqlParameter("@reservationid", reservation_id),
                        new NpgsqlParameter("@description", description)
                        });
            }

            public void DeleteContest(int contest_id, DateTime start, DateTime end)
            {                
               
                PostgresModels sql = new PostgresModels();
                ReservationModels.MakeBooking makebooking = new ReservationModels.MakeBooking();
             // int reservation_id = makebooking.MakeReservations(start, end, false, true, 1);
                // behövs ny metod för att uppdatera en tävling
                sql.SqlNonQuery("DELETE FROM contests WHERE id=@id", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@id", contest_id),
                        });
                ReservationModels.CancelReservationsWhenContest(start, end, 1);
            }


        }

        public class Group
        {
            public List<int> Groups { get; set; } = new List<int>();
        }

        public class Contest
        {
            public void MembersInContestTimeSetting(int contestid)
            {
                EmailModels.SendEmail("tim592096@gmail.com", "zave12ave", EmailModels.GetEmailForContest(contestid), "Tävling", " Denna tid har har du blivit bokad på");

                const int MAX_PLAYERS_PER_MATCH = 3;

                Random Random = new Random();
                
                PostgresModels Database = new PostgresModels();

                DataTable Table = Database.SqlQuery("SELECT memberid, timestart FROM players INNER JOIN contests ON contests.id = players.contestid INNER JOIN reservations ON  reservations.id = contests.reservationid WHERE contestid = @id", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@id", contestid)
                });

                DateTime temptime = new DateTime();
                foreach (DataRow starttime in Table.Rows)
                {
                    temptime = (DateTime)starttime["timestart"];
                    break;
                }

                DateTime time = new DateTime(temptime.Year, temptime.Month, temptime.Day, temptime.Hour, temptime.Minute, 00);

                int counter = 0;

                List<DataRow> MemberID = Table.AsEnumerable().ToList();
                var TempUnorderedlist = MemberID.OrderBy(x => Random.Next());
                List<int> Unorderedlist = new List<int>();

                foreach (DataRow Row in TempUnorderedlist)
                {
                    Unorderedlist.Add((int)Row["memberid"]);
                }

                if (Table.Rows.Count % 3 == 1)
                {
                    List<Group> Groups = new List<Group>();
                    Group group = new Group();

                    foreach (int Row in Unorderedlist)
                    {
                        if (counter == 3)
                        {
                            counter = 0;
                            Groups.Add(group);
                            group = new Group();
                            group.Groups.Add(Row);
                            counter++;
                        }

                        else
                        {
                            group.Groups.Add(Row);
                            counter++;
                        }
                    }

                    Groups.Add(group);

                    var temp1 = Groups.Where(x => x.Groups.Count == 1).ToList();
                    var temp2 = Groups.Where(x => x.Groups.Count == 3).ToList();

                    int j = 0;

                    for (int t = 0; t < temp1.Count(); t++)
                    {
                        temp1[t].Groups.Add(temp2[j].Groups[j]);
                        temp2[t].Groups.Remove(temp2[j].Groups[j]);
                        j++;
                    }

                    foreach (var item in temp2)
                    {
                        temp1.Add(item);
                    }

                    foreach (var onegroup in temp1)
                    {
                        foreach (int memberid in onegroup.Groups)
                        {
                            Database = new PostgresModels();
                            Database.SqlNonQuery("UPDATE players SET starttime = @time WHERE memberid = @memberid AND contestid = @contestid", PostgresModels.list = new List<NpgsqlParameter>()
                            {
                                new NpgsqlParameter("@memberid", memberid),
                                new NpgsqlParameter("@contestid", contestid),
                                new NpgsqlParameter("@time", time)
                            });
                        }
                        time = time.AddMinutes(10);
                    }
                }

                else
                {
                    foreach (int Row in Unorderedlist)
                    {
                        if (counter == 3)
                        {
                            counter = 0;
                            time = time.AddMinutes(10);
                        }

                        if (counter < MAX_PLAYERS_PER_MATCH)
                        {
                            Database = new PostgresModels();
                            Database.SqlNonQuery("UPDATE players SET starttime = @time WHERE memberid = @memberid AND contestid = @contestid", PostgresModels.list = new List<NpgsqlParameter>()
                            {
                                new NpgsqlParameter("@memberid", Row),
                                new NpgsqlParameter("@contestid", contestid),
                                new NpgsqlParameter("@time", time)
                            });
                            counter++;
                        }
                    }
                }
            }
            public DataTable GetContest(int id)
            {
                PostgresModels Database = new PostgresModels();
                DataTable dt = new DataTable("data");
                dt = Database.SqlQuery("SELECT contests.name AS \"Namn\", reservations.timestart AS \"Start\", reservations.timeend AS \"Slut\", contests.maxplayers AS \"Max spelare\", contests.closetime AS \"Sista anm.\", contests.id, contests.description FROM reservations, contests WHERE reservations.id = contests.reservationid AND contests.id=@id", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@id", id)
                });
                return dt;

            }
            public DataTable GetAllContestsGuests()
            {
                PostgresModels Database = new PostgresModels();
                DataTable dt = new DataTable("data");
                dt = Database.SqlQuery("SELECT contests.name AS \"Namn\", reservations.timestart AS \"Start\", reservations.timeend AS \"Slut\", contests.maxplayers AS \"Max spelare\", contests.closetime AS \"Sista anm.\", contests.id, contests.description FROM reservations, contests WHERE reservations.id = contests.reservationid AND reservations.timestart > CURRENT_DATE AND contests.closetime > CURRENT_DATE", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    //new NpgsqlParameter("@time", DateTime.Now)
                });

                return dt;
            }

            public DataTable GetAllContests()
            {
                PostgresModels Database = new PostgresModels();
                DataTable dt = new DataTable("data");
                dt = Database.SqlQuery("SELECT contests.name AS \"Namn\", reservations.timestart AS \"Start\", reservations.timeend AS \"Slut\",  contests.closetime AS \"Sista anm.\", contests.id FROM reservations, contests WHERE reservations.id= contests.reservationid AND reservations.timestart > CURRENT_DATE AND contests.closetime > CURRENT_DATE", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    //new NpgsqlParameter("@time", DateTime.Now)
                });

                return dt;
            }

            public DataTable GetPublishedContests()
            {
                PostgresModels Database = new PostgresModels();
                DataTable dt = new DataTable("data");
                dt = Database.SqlQuery("SELECT contests.name AS \"Namn\", timestart AS \"Start\", timeend AS \"Slut\", contests.id FROM contests LEFT JOIN reservations ON contests.reservationid = reservations.id WHERE publish = TRUE ", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    //new NpgsqlParameter("@time", DateTime.Now)
                });

                return dt;
            }

            public DataTable MembersInContest(int contestid)
            {
                PostgresModels Database = new PostgresModels();
                DataTable dt = new DataTable("data");
                dt = Database.SqlQuery("SELECT golfid AS \"GolfID\", firstname AS \"Förnamn\", lastname AS \"Efternamn\", hcp AS \"HCP\", genders.gender AS \"Kön\", membercategories.category AS \"Medlemskategori\", players.result AS \"Resultat\", members.id AS \"Välj åtgärd\" FROM members LEFT JOIN membercategories ON membercategories.id = members.membercategory LEFT JOIN genders ON genders.id = members.gender LEFT JOIN players ON members.id = players.memberid LEFT JOIN contests ON players.contestid = contests.id WHERE contests.id = @contestid", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@contestid", contestid)
                });

                return dt;
            }

            public void AddPlayersToContest(int contestid, int memberid)
            {
                PostgresModels Database = new PostgresModels();
                Database.SqlNonQuery("INSERT INTO PLAYERS(contestid, memberid, result) VALUES(@contestid, @memberid, -1)", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@contestid", contestid),
                    new NpgsqlParameter("@memberid", memberid),
                });
            }

            public void DeletePlayersFromContest(int contestid, int memberid)
            {
                PostgresModels Database = new PostgresModels();
                Database.SqlNonQuery("DELETE FROM players WHERE contestid = @contestid AND memberid = @memberid", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@contestid", contestid),
                    new NpgsqlParameter("@memberid", memberid),
                });
            }

            //Metod för att hämta namn och datum för en specifik tävling
            public DataTable GetNameAndDate(int contestid)
            {
                DataTable dt = new DataTable();
                PostgresModels Database = new PostgresModels();
                dt = Database.SqlQuery("SELECT contests.name AS \"cn\", timestart FROM contests LEFT JOIN reservations ON contests.reservationid = reservations.id WHERE contests.id = @contestid", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@contestid", contestid),
                });

                return dt;
            }

            public DataTable MyFinishedContests(int memberid)
            {
                DataTable dt = new DataTable();
                PostgresModels Database = new PostgresModels();
                dt = Database.SqlQuery("SELECT contests.name AS \"Namn\", timestart AS \"Datum\", result AS \"Resultat\" from contests LEFT JOIN players ON contests.id = players.memberid LEFT JOIN reservations ON contests.reservationid = reservations.id WHERE memberid = 1 AND publish = TRUE", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@contestid", memberid),
                });

                return dt;
            }
        }

        public class Result
        {
            public DataTable GetResultList(int contestid)
            {
                PostgresModels Database = new PostgresModels();
                DataTable dt = new DataTable("data");
                dt = Database.SqlQuery("SELECT firstname AS \"Förnamn\", lastname AS \"Efternamn\", result AS \"Resultat\" FROM players LEFT JOIN members ON players.memberid = members.id LEFT JOIN contests ON players.contestid = contests.id WHERE contestid = @contestid AND publish = true ORDER BY result DESC", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@contestid", contestid),
                });

                return dt;
            }
        }
    }
    public class ContestScore
    {
        public int User_id { get; set; }
        [Display(Name = "Namnet")]
        public string Name { get; set; }
        public string Contest { get; set; }
        [Required]
        [Display(Name = "Hål 1")]
        public int Hole1 { get; set; }
        [Required]
        [Display(Name = "Hål 2")]
        public int Hole2 { get; set; }
        [Required]
        [Display(Name = "Hål 3")]
        public int Hole3 { get; set; }
        [Required]
        [Display(Name = "Hål 4")]
        public int Hole4 { get; set; }
        [Required]
        [Display(Name = "Hål 5")]
        public int Hole5 { get; set; }
        [Required]
        [Display(Name = "Hål 6")]
        public int Hole6 { get; set; }
        [Required]
        [Display(Name = "Hål 7")]
        public int Hole7 { get; set; }
        [Required]
        [Display(Name = "Hål 8")]
        public int Hole8 { get; set; }
        [Required]
        [Display(Name = "Hål 9")]
        public int Hole9 { get; set; }
        [Required]
        [Display(Name = "Hål 10")]
        public int Hole10 { get; set; }
        [Required]
        [Display(Name = "Hål 11")]
        public int Hole11 { get; set; }
        [Required]
        [Display(Name = "Hål 12")]
        public int Hole12 { get; set; }
        [Required]
        [Display(Name = "Hål 13")]
        public int Hole13 { get; set; }
        [Required]
        [Display(Name = "Hål 14")]
        public int Hole14 { get; set; }
        [Required]
        [Display(Name = "Hål 15")]
        public int Hole15 { get; set; }
        [Required]
        [Display(Name = "Hål 16")]
        public int Hole16 { get; set; }
        [Required]
        [Display(Name = "Hål 17")]
        public int Hole17 { get; set; }
        [Required]
        [Display(Name = "Hål 18")]
        public int Hole18 { get; set; }
        public int Par { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public double HCP { get; set; }
        public string GolfID { get; set; }
        public int Gender { get; set; }
        public int TeeID { get; set; }
        public double WomanCR { get; set; }
        public int WomanSlope { get; set; }
        public double ManCR { get; set; }
        public int ManSlope { get; set; }
        public int Strokes { get; set; }
        public int Counting { get; set; }
        public int Rest { get; set; }
        public int Hole { get; set; }
        public int HoleHCP { get; set; }
        public int HolePar { get; set; }
        public int Result { get; set; }
        public int ContestID { get; set; }
    }

}
