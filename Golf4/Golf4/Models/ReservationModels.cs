using Npgsql;
using System.Collections.Generic;
using System;

namespace Golf4.Models
{
    public class ReservationModels
    {
        public int ID { get; set; } = 0;
        public DateTime Timestart { get; set; }
        public DateTime Timeend { get; set; }
        public bool Closed { get; set; } = false;
        public int User { get; set; } = 0;


        public static void AddReservation(ReservationModels reservation)
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

        public static void AddBall(List<int> memberid, int reservationid)
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

        public static void RemoveBall(int reservationid, int userid)
        {
            PostgresModels Database = new PostgresModels();
            Database.SqlNonQuery("DELETE FROM balls WHERE reservationid = @reservationid AND userid = @userid", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@reservationid", reservationid),
                new NpgsqlParameter("@userid", userid)
            });
        }

    }
}