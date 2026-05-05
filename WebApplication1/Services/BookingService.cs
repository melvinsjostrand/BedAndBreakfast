using bedandbreakfast1.Data;
using bedandbreakfast1.Models;
using System.Text.RegularExpressions;

namespace bedandbreakfast1.Services
{
    public class BookingService
    {
        private List<Room> rooms = new();
        private readonly AppDbContext _context;

        public BookingService(AppDbContext context)
        {
            _context = context;

            rooms.Add(new Room { Id = 1, Type = "Single", Size = 11, Beds = 1, PricePerNight = 550 });
            rooms.Add(new Room { Id = 2, Type = "Double", Size = 14, Beds = 2, PricePerNight = 700 });
            rooms.Add(new Room { Id = 3, Type = "Double", Size = 16, Beds = 1, PricePerNight = 765 });
            rooms.Add(new Room { Id = 4, Type = "Family", Size = 24, Beds = 3, PricePerNight = 850 });
        }

        public List<Room> GetRooms()
        {
            return rooms;
        }

        public Booking CreateBooking(int roomId, Guest guest, string checkIn, string checkOut, bool headsUp)
        {
            Room room = rooms.FirstOrDefault(r => r.Id == roomId);

            if (room == null)
            {
                throw new ArgumentException("Room not found.");
            }



            DateTime CheckInDate = DateTime.Parse(checkIn);
            DateTime CheckOutDate = DateTime.Parse(checkOut);


            bool isBooked = _context.Bookings.Any(b =>
            b.Room.Id == roomId &&
            CheckInDate < b.CheckOut &&
            CheckOutDate > b.CheckIn);

            if (isBooked) {
                throw new ArgumentException("Room is already booked for the selected dates.");
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

            bool autoHeadsUp = CheckInDate.TimeOfDay > new TimeSpan(20, 0, 0);

            int days = (CheckOutDate - CheckInDate).Days;

            decimal total =
                Math.Max(room.PricePerNight,
                days * room.PricePerNight);

            Booking booking = new Booking
            {
                Room = room,
                Guest = guest,
                CheckIn = CheckInDate,
                CheckOut = CheckOutDate,
                HeadsUp = autoHeadsUp,
                TotalPrice = total
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return booking;
        }

        public Booking UpdateBooking(int id, string checkIn, string checkOut)
        {
            Booking booking = _context.Bookings.FirstOrDefault(b => b.Id == id);

            if (booking == null)
            {
                throw new ArgumentException("Booking not found.");
            }

            DateTime CheckInDate = DateTime.Parse(checkIn);
            DateTime CheckOutDate = DateTime.Parse(checkOut);

            bool isBooked = _context.Bookings.Any(b =>
            b.Room.Id == booking.Room.Id &&
            b.Id != id &&
            CheckInDate < b.CheckOut &&
            CheckOutDate > b.CheckIn);

            if (isBooked)
            {
                throw new ArgumentException("Room is already booked for these dates.");
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

            booking.CheckIn = CheckInDate;
            booking.CheckOut = CheckOutDate;
            booking.HeadsUp = CheckInDate.TimeOfDay > new TimeSpan(20, 0, 0);

            int days = (CheckOutDate - CheckInDate).Days;

            booking.TotalPrice =
                Math.Max(booking.Room.PricePerNight,
                days * booking.Room.PricePerNight);

            _context.SaveChanges();

            return booking;
        }

        public void DeleteBooking(int id)
        {
            Booking booking = _context.Bookings.FirstOrDefault(b => b.Id == id);

            if (booking == null)
            {
                throw new ArgumentException("Booking not found.");
            }

            _context.Bookings.Remove(booking);
            _context.SaveChanges();
        }
    }
}