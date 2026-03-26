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

            try
            {
                Booking booking = _service.CreateBooking(roomId, guest, checkIn, checkOut, HeadsUp);
                return StatusCode(201, booking);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }   
}

