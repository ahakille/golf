using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf4.Models
{
    public class Reservation
    {
        public int ID { get; set; } = 0;
        public DateTime Timestart { get; set; }
        public DateTime Timeend { get; set; }
        public bool Closed { get; set; }
        public int User { get; set; } = 0;
    }
}