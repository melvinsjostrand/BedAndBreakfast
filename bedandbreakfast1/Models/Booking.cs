using System.Text.Json.Serialization;

namespace bedandbreakfast1.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public Room Room { get; set; }

        public Guest Guest { get; set; }

        public DateTime CheckIn { get; set; } 

        public DateTime CheckOut { get; set; }

        public bool HeadsUp { get; set; }

        public int Time { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
