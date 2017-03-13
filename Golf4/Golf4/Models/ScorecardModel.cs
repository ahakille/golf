using System;
using System.Data;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf4.Models
{
    public class ScorecardModel
    {
        public int TeeID { get; set; } = 0;
        public string TeeName { get; set; } = "";
        public double WomanCR { get; set; } = 0;
        public int WomanSlope { get; set; } = 0;
        public double ManCR { get; set; } = 0;
        public int ManSlope { get; set; } = 0;
        public DataTable AllData { get; set; }
        public DataTable Tees { get; set; }
        public int FirstHalfPar { get; set; } = 0;
        public int LastHalfPar { get; set; } = 0;
        public int TotalPar { get; set; } = 0;
        public int Strokes { get; set; } = 0;
        public MemberModels ScorecardMember { get; set; } = new MemberModels();
        public ReservationModels ScorecardReservation { get; set; } = new ReservationModels();
        public List<ScorecardModel> Guestlist { get; set; } = new List<ScorecardModel>();
    }
}