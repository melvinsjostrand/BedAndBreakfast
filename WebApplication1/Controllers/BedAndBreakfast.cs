using bedandbreakfast1.Models;
using bedandbreakfast1.Services;
using Microsoft.AspNetCore.Mvc;

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

        // GET: api/booking/rooms
        [HttpGet("rooms")]
        public IActionResult GetRooms()
        {
            return Ok(_service.GetRooms());
        }

        // POST: api/booking/create
        [HttpPost("create")]
        public IActionResult CreateBooking(
            int roomId,
            Guest guest,
            string checkIn,
            string checkOut)
        {
            try
            {
                Booking booking =
                    _service.CreateBooking(roomId, guest, checkIn, checkOut, false);

                return StatusCode(201, booking);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/booking/update/1
        [HttpPut("update/{id}")]
        public IActionResult UpdateBooking(
            int id,
            string checkIn,
            string checkOut)
        {
            try
            {
                Booking booking =
                    _service.UpdateBooking(id, checkIn, checkOut);

                return Ok(booking);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/booking/delete/1
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteBooking(int id)
        {
            try
            {
                _service.DeleteBooking(id);

                return Ok("Booking deleted.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}