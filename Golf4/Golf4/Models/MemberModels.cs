namespace Golf4.Models
{
    public class MemberModels
    {
        public int ID { get; set; } = 0;
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string Adress { get; set; } = "";
        public string Postalcode { get; set; } = "";
        public string City { get; set; } = "";
        public string Email { get; set; } = "";
        public int Gender { get; set; } = 0;
        public double HCP { get; set; } = 0.0;
        public int GolfID { get; set; } = 0;
        public int Membercategory { get; set; } = 0;
        public string Telephone { get; set; } = "";
        public bool Payment { get; set; } = false;

    }

}