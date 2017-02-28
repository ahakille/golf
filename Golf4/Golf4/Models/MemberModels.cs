using Npgsql;
using System.Collections.Generic;

namespace Golf4.Models
{
    public class MemberModels
    {
        public int ID { get; set; } = 0;
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string Adress { get; set; } = "";
        public string Postalcode { get; set; } = "";
        public string City { get; set; } = "";
        public string Email { get; set; } = "";
        public int Gender { get; set; } = 0;
        public double HCP { get; set; } = 0.0;
        public int GolfID { get; set; } = 0;
        public int Membercategory { get; set; } = 0;
        public string Telephone { get; set; } = "";
        public bool Payment { get; set; } = false;

        public static void Reservation(ReservationModels reservation)
        {
            PostgresModels Database = new PostgresModels();
            Database.SqlNonQuery("INSERT INTO reservation(timestart, timeend, closed, user) VALUES(@timestart, @timeend, @closed, @user)", PostgresModels.list = new List<NpgsqlParameter>()
            {                
                 new NpgsqlParameter("@timestart", reservation.Timestart),
                 new NpgsqlParameter("@timeend", reservation.Timeend),
                 new NpgsqlParameter("@closed", reservation.Closed),
                 new NpgsqlParameter("@user", reservation.ID)
            });           
        }

        public static void Reservation(List<int> memberid, int reservationid)
        {
            PostgresModels Database = new PostgresModels();
            foreach (int userid in memberid)
            {
                Database.SqlNonQuery("INSERT INTO balls(userid, reservationid) VALUES(@userid, @reservationid)", PostgresModels.list = new List<NpgsqlParameter>()
                {
                     new NpgsqlParameter("@userid", userid),
                     new NpgsqlParameter("@reservationid", reservationid)
                });
            }
        }

        public static void RemoveReservation(int reservationid)
        {
            PostgresModels Database = new PostgresModels();
            Database.SqlNonQuery("DELETE FROM balls WHERE reservationid = @reservationid; DELETE FROM reservation WHERE id = @reservationid", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@reservationid", reservationid),
            });
        }
    }

}