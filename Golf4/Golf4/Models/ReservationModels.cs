using System;
using System.ComponentModel.DataAnnotations;

namespace Golf4.Models
{
    public class ReservationModels
    {
        public int ID { get; set; } = 0;
        public int MemberID { get; set; } = 0;
        public string MemberGolfID { get; set; } = "";
        public double MemberHCP { get; set; } = 0;
        public int MemberGender { get; set; } = 0;
        public DateTime Timestart { get; set; }
        public DateTime Timeend { get; set; }
        public bool Closed { get; set; } = false;
        public int User { get; set; } = 0;
        public string datepicker { get; set; }

        public class CloseGolfCourseView
        {
            public DateTime ClosingStartDate { get; set; }
            public DateTime ClosingStopDate { get; set; }
            public DateTime ClosingStartTime { get; set; }
            public DateTime ClosingStopTime { get; set; }
        }
        public class CreatereservationModel
        {
            public int ID { get; set; }
            [Display(Name = "Förnamn")]
            public string Firstname { get; set; }
            [Display(Name = "Efternamn")]
            public string Lastname { get; set; }
            [Display(Name = "Kön")]
            public string Gender { get; set; }
            [Display(Name = "HCP")]
            public double HCP { get; set; }
            [Display(Name = "Golfid")]
            public string GolfID { get; set; }
            [Display(Name ="Datum och tid")]
            public DateTime Timestart { get; set; }
            public DateTime Timeend { get; set; }
            public bool Closed { get; set; } = false;
            public int User { get; set; } 
        }
    }
    
}