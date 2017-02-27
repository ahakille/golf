using Npgsql;
using System.Collections.Generic;

namespace Golf4.Models
{
    public class Member
    {
        public int ID { get; set; } = 0;
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string Adress { get; set; } = "";
        public string Postalcode { get; set; } = "";
        public string City { get; set; } = "";
        public string Email { get; set; } = "";
        public Gender Gender { get; set; }
        public double HCP { get; set; } = 0.0;
        public int GolfID { get; set; } = 0;
        public Membercategory Membercategory { get; set; } = new Membercategory();
        public string Telefone { get; set; } = "";

        public static void Boka(Reservation reservation)
        {
            PostgresModels Databas = new PostgresModels();
            Databas.SqlNonQuery("INSERT INTO reservation(timestart, timeend, closed, user)", PostgresModels.list = new List<NpgsqlParameter>()
            {                
                 new NpgsqlParameter("@timestart", reservation.Timestart),
                 new NpgsqlParameter("@timeend", reservation.Timeend),
                 new NpgsqlParameter("@closed", reservation.Closed),
                 new NpgsqlParameter("@user", reservation.ID)
            });           
        }

        public static void Boka(List<int> memberid, int reservationid)
        {
            PostgresModels Databas = new PostgresModels();
            foreach (int userid in memberid)
            {
                Databas.SqlNonQuery("INSERT INTO (userid, reservationid)", PostgresModels.list = new List<NpgsqlParameter>()
                {
                     new NpgsqlParameter("@userid", userid),
                     new NpgsqlParameter("@reservationid", reservationid)
                });
            }
        }

    }

    public enum Gender
    {
        Male = 1,
        Female = 2,
    }
}