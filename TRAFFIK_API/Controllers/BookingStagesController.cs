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
using Microsoft.AspNetCore.Authorization;

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

        // GET: api/BookingStages/Booking/5
        /// <summary>
        /// Retrieves all booking stages for a specific booking.
        /// </summary>
        /// <param name="bookingId">The ID of the booking to get stages for.</param>
        [HttpGet("Booking/{bookingId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BookingStageUpdateRequestDto>>> GetBookingStagesByBooking(int bookingId)
        {
            // Get all stages for this booking
            var stages = await _context.BookingStages
                .Where(bs => bs.BookingId == bookingId)
                .OrderBy(bs => bs.TimeStamp)
                .Include(bs => bs.UpdatedByUser)
                .ToListAsync();

            // Get the booking to find its current status
            var booking = await _context.Bookings
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            if (stages == null || stages.Count == 0)
            {
                // No stages found, determine the initial stage based on booking status
                string initialStage = booking.Status switch
                {
                    "In Progress" => "Pending",  // New bookings with In Progress status start at Pending
                    "Completed" => "Completed",
                    "Closed" => "Paid",
                    _ => "Pending"  // Default to Pending
                };
                
                return Ok(new List<BookingStageUpdateRequestDto> 
                { 
                    new BookingStageUpdateRequestDto
                    {
                        BookingId = bookingId,
                        CurrentStage = initialStage,
                        AvailableStages = initialStage == "Pending" ? new List<string> { "Started", "Inspection", "Completed", "Paid" } : new List<string>(),
                        UpdatedAt = DateTime.UtcNow
                    }
                });
            }

            // Get the latest stage
            var latestStage = stages.Last();

            // Define stage sequence
            var stageSequence = new List<string> { "Pending", "Started", "Inspection", "Completed", "Paid" };
            
            // Determine available next stages
            var currentStageIndex = stageSequence.IndexOf(latestStage.StageName);
            var availableStages = new List<string>();
            
            if (currentStageIndex >= 0 && currentStageIndex < stageSequence.Count - 1)
            {
                // Can move to the next stage
                availableStages.Add(stageSequence[currentStageIndex + 1]);
                
                // Can also skip ahead if not too far
                for (int i = currentStageIndex + 2; i < stageSequence.Count && i <= currentStageIndex + 2; i++)
                {
                    availableStages.Add(stageSequence[i]);
                }
            }

            var response = new BookingStageUpdateRequestDto
            {
                Id = latestStage.Id,
                BookingId = bookingId,
                VehicleId = 0, // You may need to adjust this based on your Vehicle model
                CurrentStage = latestStage.StageName,
                AvailableStages = availableStages.Count > 0 ? availableStages : new List<string>(),
                SelectedStage = string.Empty,
                IsConfirmed = latestStage.Status == "Completed" || latestStage.Status == "Closed",
                UpdatedAt = latestStage.TimeStamp,
                UpdatedBy = latestStage.UpdatedByUser?.FullName ?? "Unknown"
            };

            // Return as list to match frontend expectation
            return Ok(new List<BookingStageUpdateRequestDto> { response });
        }

        //POST /api/BookingStages/UpdateStage
        //[Authorize(Roles = "Employee,Admin")] // Temporarily disabled for testing
        [HttpPost("UpdateStage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStage([FromBody] BookingStageUpdateRequestDto requestDto)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(requestDto.SelectedStage))
                {
                    return BadRequest(new { error = "SelectedStage is required.", details = ModelState });
                }

                var booking = await _context.Bookings.FindAsync(requestDto.BookingId);
                if (booking == null)
                    return BadRequest(new { error = "Invalid booking ID." });

                // Get user ID - try from claim first, then fallback to any staff user
                int updatedByUserId;
                var userIdClaim = User.FindFirst("UserId");
                
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out updatedByUserId))
                {
                    // Found user ID from claim
                    var user = await _context.Users.FindAsync(updatedByUserId);
                    if (user == null)
                    {
                        return BadRequest(new { error = "Invalid user ID from token." });
                    }
                }
                else
                {
                    // Fallback: get first staff user
                    var staffUser = await _context.Users.FirstOrDefaultAsync(u => u.RoleId == 2 || u.RoleId == 1);
                    if (staffUser == null)
                        return BadRequest(new { error = "No staff user found in database." });
                    updatedByUserId = staffUser.Id;
                }

                // Map stage name to status
                var status = MapStageToStatus(requestDto.SelectedStage);

                var stage = new BookingStages
                {
                    BookingId = requestDto.BookingId,
                    StageName = requestDto.SelectedStage,
                    Status = status,
                    TimeStamp = DateTime.UtcNow,
                    UpdatedByUserId = updatedByUserId
                };

                _context.BookingStages.Add(stage);

                // Update booking status to use the mapped status value
                booking.Status = status;
                _context.Entry(booking).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok(true);
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                return StatusCode(500, new { 
                    error = "An error occurred while updating the booking stage.",
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        private string MapStageToStatus(string stageName)
        {
            return stageName.ToLower() switch
            {
                "pending" => "In Progress",
                "started" => "In Progress",
                "inspection" => "In Progress",
                "completed" => "Completed",
                "paid" => "Closed",
                _ => "In Progress"
            };
        }
    }
}