using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Npgsql;

namespace Golf4.Models
{
    public class CompetitionModels
    {
        public int Competition_id { get; set; }
        [Required]
        [Display(Name="Tävlingsnamnet")]
        public string Name { get; set; }
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




        public class MakeCompetition
        {
            public void CreateCompetition(string name, DateTime start, DateTime end ,DateTime close, int maxplayer)
            {
                ReservationModels.MakeBooking makebooking = new ReservationModels.MakeBooking();
                int resvervation_id= makebooking.MakeReservations(start,end,false,1);
                PostgresModels sql = new PostgresModels();
                // Sql behöver fixar för en insrt till competion
                sql.SqlNonQuery("INSERT INTO cope(timestart, timeend, closed, user_id) VALUES(@timestart, @timeend, @closed, @user_id);", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@timestart", close),

                        });

            }
        }
    }

}