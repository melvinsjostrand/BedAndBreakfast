using bedandbreakfast1.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace bedandbreakfast1.Services
{
    public class BookingService
    {
        private List<Room> rooms = new();
        private List<Booking> bookings = new();

        public BookingService()
        {
            rooms.Add(new Room { Id = 1, Type = "Single", Size = 11, Beds = 1, PricePerNight = 550 });
            rooms.Add(new Room { Id = 2, Type = "Double", Size = 14, Beds = 2, PricePerNight = 700 });
            rooms.Add(new Room { Id = 3, Type = "Double", Size = 16, Beds = 1, PricePerNight = 765 });
            rooms.Add(new Room { Id = 4, Type = "Family", Size = 24, Beds = 3, PricePerNight = 850 });
        }
        public List<Room> GetRooms()
        {
            return rooms;
        }

        public Booking CreateBooking(int roomId, Guest guest, string CheckIn, string CheckOut, bool HeadsUp)
        {
            Room room = rooms.FirstOrDefault(r => r.Id == roomId);
            
            if (room == null)
            {
                Console.WriteLine("Room not found");
                return null;
            }
            DateTime CheckInDate = DateTime.Parse(CheckIn);
            DateTime CheckOutDate = DateTime.Parse(CheckOut);

            if (CheckOutDate <= CheckInDate)
            {
                Console.WriteLine("Check-out must be after check-in");
                return null;
            }

            if (CheckInDate.TimeOfDay < new TimeSpan(14, 0, 0) ||
                CheckInDate.TimeOfDay > new TimeSpan(22, 30, 0))
            {
                Console.WriteLine("Check-in must be between 14:00 and 22:30");
                return null;
            }

            if (CheckOutDate.TimeOfDay > new TimeSpan(12, 0, 0))
            {
                Console.WriteLine("Check-out must be before 12:00");
                return null;
            }

            bool headsUp = CheckInDate.TimeOfDay > new TimeSpan(20, 0, 0);

            int days = (CheckOutDate - CheckInDate).Days;
            decimal total = Math.Max(room.PricePerNight, (days) * room.PricePerNight);
   

            Booking booking = new Booking
            {
                Id = bookings.Count + 1,
                Room = room,
                Guest = guest,
                CheckIn = CheckInDate,
                CheckOut = CheckOutDate,
                HeadsUp = headsUp,
                TotalPrice = total
            };

            bookings.Add(booking);

            return booking;

        }
    }
}
