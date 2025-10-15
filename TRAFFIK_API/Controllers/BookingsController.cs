using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TRAFFIK_API.Data;
using TRAFFIK_API.Models;

namespace TRAFFIK_API.Controllers
{
    [Route("api/Bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all bookings in the system.
        /// </summary>
        /// <returns>List of all bookings.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific booking by ID.
        /// </summary>
        /// <param name="id">The ID of the booking.</param>
        /// <returns>The booking details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        /// <summary>
        /// Updates an existing booking.
        /// </summary>
        /// <param name="id">The ID of the booking to update.</param>
        /// <param name="booking">The updated booking object.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new booking.
        /// </summary>
        /// <param name="booking">The booking object to create.</param>
        /// <returns>The created booking.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        }

        /// <summary>
        /// Deletes a booking by ID.
        /// </summary>
        /// <param name="id">The ID of the booking to delete.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }

        /// <summary>
        /// Retrieves available time slots for a service on a given date.
        /// </summary>
        /// <param name="serviceId">The ID of the service.</param>
        /// <param name="date">The desired booking date.</param>
        /// <returns>List of available time slots.</returns>
        [HttpGet("AvailableSlots")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TimeOnly>>> GetAvailableSlots(int serviceId, DateOnly date)
        {
            var bookedTimes = await _context.Bookings
                .Where(b => b.ServiceId == serviceId && b.BookingDate == date)
                .Select(b => b.BookingTime)
                .ToListAsync();

            var allSlots = new List<TimeOnly>
            {
                new TimeOnly(9, 0),
                new TimeOnly(10, 0),
                new TimeOnly(11, 0),
                new TimeOnly(13, 0),
                new TimeOnly(14, 0),
                new TimeOnly(15, 0)
            };

            return allSlots.Except(bookedTimes).ToList();
        }

        /// <summary>
        /// Confirms a booking after validating availability.
        /// </summary>
        /// <param name="booking">The booking object to confirm.</param>
        /// <returns>The confirmed booking.</returns>
        [HttpPost("Confirm")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Booking>> ConfirmBooking(Booking booking)
        {
            var conflict = await _context.Bookings.AnyAsync(b =>
                b.BookingDate == booking.BookingDate &&
                b.BookingTime == booking.BookingTime &&
                b.ServiceId == booking.ServiceId &&
                b.VehicleLicensePlate == booking.VehicleLicensePlate);

            if (conflict)
                return Conflict("Time slot already booked");

            booking.Status = "Pending";
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        }

        /// <summary>
        /// Retrieves bookings for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>List of bookings for the user.</returns>
        [HttpGet("User/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Booking>>> GetUserBookings(int userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }
    }
}