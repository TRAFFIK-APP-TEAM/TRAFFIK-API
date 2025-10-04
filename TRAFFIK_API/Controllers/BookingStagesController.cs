using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TRAFFIK_API.Data;
using TRAFFIK_API.Models;
using TRAFFIK_API.DTOs;

namespace TRAFFIK_API.Controllers
{
    [Route("api/BookingStages")]
    [ApiController]
    public class BookingStagesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingStagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/BookingStages
        /// <summary>
        /// Retrieves all booking stages.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookingStages>>> GetBookingStages()
        {
            return await _context.BookingStages.ToListAsync();
        }

        // GET: api/BookingStages/5
        /// <summary>
        /// Retrieves a specific booking stage by ID.
        /// </summary>
        /// <param name="id">The ID of the booking stage to retrieve.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingStages>> GetBookingStages(int id)
        {
            var bookingStages = await _context.BookingStages.FindAsync(id);

            if (bookingStages == null)
            {
                return NotFound();
            }

            return bookingStages;
        }

        // PUT: api/BookingStages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates an existing booking stage.
        /// </summary>
        /// <param name="id">The ID of the booking stage to update.</param>
        /// <param name="bookingStages">The updated booking stage object.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutBookingStages(int id, BookingStages bookingStages)
        {
            if (id != bookingStages.Id)
            {
                return BadRequest();
            }

            _context.Entry(bookingStages).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingStagesExists(id))
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

        // POST: api/BookingStages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a new booking stage.
        /// </summary>
        /// <param name="bookingStages">The booking stage object to create.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookingStages>> PostBookingStages(BookingStages bookingStages)
        {
            _context.BookingStages.Add(bookingStages);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookingStages", new { id = bookingStages.Id }, bookingStages);
        }

        // DELETE: api/BookingStages/5
        /// <summary>
        /// Deletes a booking stage by ID.
        /// </summary>
        /// <param name="id">The ID of the booking stage to delete.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBookingStages(int id)
        {
            var bookingStages = await _context.BookingStages.FindAsync(id);
            if (bookingStages == null)
            {
                return NotFound();
            }

            _context.BookingStages.Remove(bookingStages);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks if a booking stage exists by ID.
        /// </summary>
        /// <param name="id">The ID to check.</param>
        /// <returns>True if the booking stage exists; otherwise, false.</returns>
        private bool BookingStagesExists(int id)
        {
            return _context.BookingStages.Any(e => e.Id == id);
        }

        //POST /api/BookingStages/UpdateStage
        [HttpPost("UpdateStage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStage([FromBody] BookingStageUpdateDto dto)
        {
            var booking = await _context.Bookings.FindAsync(dto.BookingId);
            var user = await _context.Users.FindAsync(dto.UpdatedByUserId);

            if (booking == null || user == null)
                return BadRequest("Invalid booking or user ID.");

            var stage = new BookingStages
            {
                BookingId = dto.BookingId,
                StageName = dto.StageName,
                Status = dto.Status,
                TimeStamp = DateTime.UtcNow,
                UpdatedByUserId = dto.UpdatedByUserId
            };

            _context.BookingStages.Add(stage);
            await _context.SaveChangesAsync();

            return Ok("Booking stage updated.");
        }
    }
}