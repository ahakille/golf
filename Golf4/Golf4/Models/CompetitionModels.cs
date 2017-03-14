using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf4.Models
{
    public class CompetitionModels
    {
        public int Competition_id { get; set; }
        public string Name { get; set; }
        public DateTime CloseTime { get; set; }
        public int MaxPlayers { get; set; }
        public bool Publish { get; set; }
        public int Reservation_id { get; set; }
        public DateTime Timestart { get; set; }
        public DateTime Timeend { get; set; }



        public class MakeCompetition
        {

        }
    }

}