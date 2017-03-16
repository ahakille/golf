﻿using System;
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
        public int Competition_id { get; set; }
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

            public void EditContest(int contest_id ,string name, DateTime start, DateTime end, DateTime close, int maxplayer, string description)
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

                List<int> MemberID = new List<int>();

                Random random = new Random();
               
                foreach (int id in contestid)
                {
                    DataTable Table = Database.SqlQuery("SELECT memberid FROM players WHERE contestid = @id", PostgresModels.list = new List<NpgsqlParameter>()
                    {
                        new NpgsqlParameter("@id", id)
                    });


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
                dt = Database.SqlQuery("SELECT golfid, firstname, lastname, hcp, gender, membercategory FROM members LEFT JOIN players ON members.id = players.memberid LEFT JOIN contests ON players.contestid = contests.id WHERE contests.id = @contestid", PostgresModels.list = new List<NpgsqlParameter>()
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
        }

        public class AdminViewModel
        {

        }
    }
}