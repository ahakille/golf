using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Npgsql;

namespace Golf4.Models
{
    public class HomeCompModels
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

        public class Contest
        {
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
                dt = Database.SqlQuery("SELECT golfid AS \"GolfID\", firstname AS \"Förnamn\", lastname AS \"Efternamn\", hcp AS \"HCP\", gender AS \"Kön\", membercategory AS \"Medlemskategori\" FROM members LEFT JOIN players ON members.id = players.memberid LEFT JOIN contests ON players.contestid = contests.id WHERE contests.id = @contestid", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@contestid", contestid)
                });

                return dt;
            }
        }
    }
}