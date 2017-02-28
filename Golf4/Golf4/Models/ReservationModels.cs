using System;

namespace Golf4.Models
{
    public class ReservationModels
    {
        public int ID { get; set; } = 0;
        public DateTime Timestart { get; set; }
        public DateTime Timeend { get; set; }
        public bool Closed { get; set; }
        public int User { get; set; } = 0;
    }
}