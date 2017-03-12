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
        public int Hole { get; set; }
        public int Yellow { get; set; }
        public int Red { get; set; }
        public int Par { get; set; }
        public int HCP { get; set; }
        public int User_id1 { get; set; }
        public int User_1_Hcp { get; set; }
        public int User_id2 { get; set; }
        public int User_2_Hcp { get; set; }
        public int User_id3 { get; set; }
        public int User_3_Hcp { get; set; }
        public int User_id4 { get; set; }
        public int User_4_Hcp { get; set; }
        public DataTable AllData { get; set; }
        public DataTable Tees { get; set; }

    }
}