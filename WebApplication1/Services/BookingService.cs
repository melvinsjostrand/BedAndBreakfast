using bedandbreakfast1.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.RegularExpressions;
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
            DateTime CheckInDate = DateTime.Parse(CheckIn);
            DateTime CheckOutDate = DateTime.Parse(CheckOut);


            if (room == null)
            {
                throw new ArgumentException("Room not found.");
            }


            if (!Regex.IsMatch(guest.Name, @"^[\p{L}\s]{2,50}$"))
            {
                throw new ArgumentException("Name must contain only letters and be 2–50 characters long.");
            }

            if (!Regex.IsMatch(guest.Phone, @"^\+?[0-9]{7,15}$"))
            {
                throw new ArgumentException("Invalid phone number format.");
            }
            if (CheckOutDate <= CheckInDate)
            {
                throw new ArgumentException("Check-out must be after check-in.");
            }

            if (CheckInDate.TimeOfDay < new TimeSpan(14, 0, 0) ||
                CheckInDate.TimeOfDay > new TimeSpan(22, 30, 0))
            {
                throw new ArgumentException("Check-in must be between 14:00 and 22:30.");
            }

            if (CheckOutDate.TimeOfDay > new TimeSpan(12, 0, 0))
            {
                throw new ArgumentException("Check-out must be before 12:00.");
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
