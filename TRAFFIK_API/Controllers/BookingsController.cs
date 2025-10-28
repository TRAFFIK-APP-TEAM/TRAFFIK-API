using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TRAFFIK_API.Data;
using TRAFFIK_API.DTOs;
using TRAFFIK_API.Models;

namespace TRAFFIK_API.Controllers
{
    public class BookingRequest
    {
        public Booking Booking { get; set; } = null!;
    }
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
        /// Retrieves all bookings for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>List of bookings for the user.</returns>
        [HttpGet("User/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByUser(int userId)
        {
            var bookings = await _context.Bookings
                .Include(b => b.ServiceCatalog)
                .Include(b => b.Vehicle)
                .Where(b => b.UserId == userId)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    ServiceCatalogId = b.ServiceCatalogId,
                    BookingDate = b.BookingDate,
                    BookingTime = b.BookingTime,
                    Status = b.Status,
                    VehicleLicensePlate = b.VehicleLicensePlate,
                    ServiceName = b.ServiceCatalog != null && !string.IsNullOrEmpty(b.ServiceCatalog.Name) ? b.ServiceCatalog.Name : "Service",
                    VehicleDisplayName = b.Vehicle != null ? $"{b.Vehicle.Make ?? ""} {b.Vehicle.Model ?? ""}".Trim() : ""
                })
                .ToListAsync();

            return bookings;
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
        /// <param name="request">The booking request containing the booking object.</param>
        /// <returns>The created booking.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookingDto>> PostBooking(BookingRequest request)
        {
            var booking = request.Booking;

            var user = await _context.Users.FindAsync(booking.UserId);
            if (user == null)
                return BadRequest("User not found");

            // Verify Vehicle exists if provided
            if (!string.IsNullOrEmpty(booking.VehicleLicensePlate))
            {
                var vehicle = await _context.Vehicles.FindAsync(booking.VehicleLicensePlate);
                if (vehicle == null)
                    return BadRequest("Vehicle not found");
            }

            // Load ServiceCatalog if provided
            ServiceCatalog? serviceCatalog = null;
            if (booking.ServiceCatalogId.HasValue)
            {
                serviceCatalog = await _context.ServiceCatalogs.FindAsync(booking.ServiceCatalogId.Value);
            }

            var newBooking = new Booking
            {
                UserId = booking.UserId,
                ServiceCatalogId = booking.ServiceCatalogId,
                BookingDate = booking.BookingDate,
                BookingTime = booking.BookingTime,
                Status = booking.Status,
                VehicleLicensePlate = booking.VehicleLicensePlate
            };

            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();

            var dto = new BookingDto
            {
                Id = newBooking.Id,
                UserId = newBooking.UserId,
                ServiceCatalogId = newBooking.ServiceCatalogId,
                BookingDate = newBooking.BookingDate,
                BookingTime = newBooking.BookingTime,
                Status = newBooking.Status,
                VehicleLicensePlate = newBooking.VehicleLicensePlate
            };

            return CreatedAtAction("GetBooking", new { id = dto.Id }, dto);
        }

        [HttpPost("Confirm")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<BookingDto>> ConfirmBooking(BookingRequest request)
        {
            var booking = request.Booking;

            var conflict = await _context.Bookings.AnyAsync(b =>
                b.BookingDate == booking.BookingDate &&
                b.BookingTime == booking.BookingTime &&
                b.ServiceCatalogId == booking.ServiceCatalogId &&
                b.VehicleLicensePlate == booking.VehicleLicensePlate);

            if (conflict)
                return Conflict("Time slot already booked");

            booking.Status = "Pending";

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var dto = new BookingDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                ServiceCatalogId = booking.ServiceCatalogId,
                BookingDate = booking.BookingDate,
                BookingTime = booking.BookingTime,
                Status = booking.Status,
                VehicleLicensePlate = booking.VehicleLicensePlate
            };

            return CreatedAtAction("GetBooking", new { id = dto.Id }, dto);
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
        public async Task<ActionResult<IEnumerable<TimeOnly>>> GetAvailableSlots(int serviceCatalogId, DateOnly date)
        {
            var bookedTimes = await _context.Bookings
                .Where(b => b.ServiceCatalogId == serviceCatalogId && b.BookingDate == date)
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

    }
}