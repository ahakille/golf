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

            public void EditContest(int contest_id, string name, DateTime start, DateTime end, DateTime close, int maxplayer, string description)
            {
                PostgresModels sql = new PostgresModels();
                ReservationModels.MakeBooking makebooking = new ReservationModels.MakeBooking();
                //int reservation_id = makebooking.(start, end, false, true, user);
                // behövs ny metod för att uppdatera en tävling
                sql.SqlNonQuery("INSERT INTO contests(name, closetime, maxplayers, publish, reservationid, description) VALUES(@name, @closetime, @maxplayers, FALSE, @reservationid, @description);", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@name", name),
                        new NpgsqlParameter("@closetime", close),
                        new NpgsqlParameter("@maxplayers", maxplayer),
                       // new NpgsqlParameter("@reservationid", reservation_id),
                        new NpgsqlParameter("@description", description)
                        });
            }


        }

        public class Contest
        {

            public static void MembersInContestTimeSetting(List<int> contestid)
            {
                const int MAX_PLAYERS_PER_MATCH = 3;

                PostgresModels Database = new PostgresModels();

                Random Random = new Random();
               
                foreach (int ID in contestid)
                {
                    DataTable Table = Database.SqlQuery("SELECT memberid FROM players WHERE contestid = @id", PostgresModels.list = new List<NpgsqlParameter>()
                    {
                        new NpgsqlParameter("@id", ID)
                    });

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
                        for (int i = 0; i < Unorderedlist.Count; i++)
                        {
                            if (Unorderedlist.Count - 1 == i || Unorderedlist.Count - 2 == i)
                            {
                                // ska lägga till en update här
                            }

                            else
                            {
                                // ska lägga till en update här
                            }
                        }
                    }

                    else
                    {                       
                        foreach (int Row in Unorderedlist)
                        {
                            if (counter == 3)
                            {
                                counter = 0;
                            }

                            if (counter <= MAX_PLAYERS_PER_MATCH)
                            {
                                // ska lägga till en update här
                                counter++;
                            }
                        }
                    }
                }
            }

            public DataTable GetAllContests()
            {
                PostgresModels Database = new PostgresModels();
                DataTable dt = new DataTable("data");
                dt = Database.SqlQuery("SELECT contests.name AS \"Namn\", reservations.timestart AS \"Start\", reservations.timeend AS \"Slut\",  contests.closetime AS \"Sista anm.\", contests.id FROM reservations, contests WHERE reservations.id = contests.reservationid AND reservations.timestart > CURRENT_DATE AND contests.closetime > CURRENT_DATE", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    //new NpgsqlParameter("@time", DateTime.Now)
                });

                return dt;
            }

            public DataTable MembersInContest(int contestid)
            {
                PostgresModels Database = new PostgresModels();
                DataTable dt = new DataTable("data");
                dt = Database.SqlQuery("SELECT golfid AS \"GolfID\", firstname AS \"Förnamn\", lastname AS \"Efternamn\", hcp AS \"HCP\", genders.gender AS \"Kön\", membercategories.category AS \"Medlemskategori\", members.id AS \"id\" FROM members LEFT JOIN membercategories ON membercategories.id = members.membercategory LEFT JOIN genders ON genders.id = members.gender LEFT JOIN players ON members.id = players.memberid LEFT JOIN contests ON players.contestid = contests.id WHERE contests.id = @contestid", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@contestid", contestid)
                });

                return dt;
            }

            public void AddPlayersToContest(int contestid, int memberid)
            {
                PostgresModels Database = new PostgresModels();
                Database.SqlNonQuery("INSERT INTO PLAYERS(contestid, memberid) VALUES(@contestid, @memberid)", PostgresModels.list = new List<NpgsqlParameter>()
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

            public class AdminViewModel
            {

            }
        }
    }
}