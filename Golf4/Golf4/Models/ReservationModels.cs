using System;

namespace Golf4.Models
{
    public class ReservationModels
    {
        public int ID { get; set; } = 0;
        public int MedlemsID { get; set; } = 0;
        public string GolfID { get; set; } = "";
        public double HCP { get; set; } = 0;
        public int Gender { get; set; } = 0;
        public DateTime Timestart { get; set; }
        public DateTime Timeend { get; set; }
        public bool Closed { get; set; } = false;
        public int User { get; set; } = 0;
        public string datepicker { get; set; }
    }
    
}