using bedandbreakfast1.Models;
using bedandbreakfast1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace bedandbreakfast1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _service;

        public BookingController(BookingService service)
        {
            _service = service;
        }

        [HttpGet("rooms")]
        public IActionResult GetRooms()
        {
            return Ok(_service.GetRooms());
        }

        [HttpPost("booking")]
        public IActionResult CreateBooking(int roomId, Guest guest, string checkIn, string checkOut, bool HeadsUp)
        {
            var booking = _service.CreateBooking(roomId, guest, checkIn, checkOut, HeadsUp);

            if (booking == null)
                return BadRequest();


            Console.WriteLine($"Booking ID: {booking.Id}");
            Console.WriteLine($"Guest Name: {booking.Guest.Name}");
            Console.WriteLine($"Guest Phone: {booking.Guest.Phone}");
            Console.WriteLine($"Room Type: {booking.Room.Type}");
            Console.WriteLine($"Room Size: {booking.Room.Size}m²");
            Console.WriteLine($"Beds: {booking.Room.Beds}");
            Console.WriteLine($"Check-in: {booking.CheckIn}");
            Console.WriteLine($"Check-out: {booking.CheckOut}");
            Console.WriteLine($"Heads-up (after 20:00): {(booking.HeadsUp ? "Yes" : "No")}");
            Console.WriteLine($"Total Price: {booking.TotalPrice} DKK");
            return StatusCode(200);
        }
    }   
}

