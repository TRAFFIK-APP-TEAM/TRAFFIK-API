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

            if (stages == null || stages.Count == 0)
            {
                // No stages found, return default "Pending" stage
                return Ok(new List<BookingStageUpdateRequestDto> 
                { 
                    new BookingStageUpdateRequestDto
                    {
                        BookingId = bookingId,
                        CurrentStage = "Pending",
                        AvailableStages = new List<string> { "Started", "Inspection", "Completed", "Paid" },
                        UpdatedAt = DateTime.UtcNow
                    }
                });
            }

            // Get the latest stage
            var latestStage = stages.Last();
            
            // Get the booking to find vehicle ID
            var booking = await _context.Bookings
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

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
        // TODO: Re-enable authorization when JWT authentication is configured
        // [Authorize(Roles = "Employee,Admin")]
        [HttpPost("UpdateStage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStage([FromBody] object rawRequest)
        {
            try
            {
                // Debug logging
                Console.WriteLine($"[UpdateStage] Received raw request: {rawRequest}");
                
                if (rawRequest == null)
                {
                    Console.WriteLine("[UpdateStage] Request is null");
                    return BadRequest("Request data is required.");
                }
                
                // Manually deserialize to handle missing properties
                dynamic requestJson = rawRequest;
                int bookingId = 0;
                string selectedStage = string.Empty;
                
                try
                {
                    bookingId = Convert.ToInt32(requestJson.BookingId);
                    selectedStage = requestJson.SelectedStage?.ToString() ?? string.Empty;
                    Console.WriteLine($"[UpdateStage] Parsed BookingId={bookingId}, SelectedStage={selectedStage}");
                }
                catch (Exception parseEx)
                {
                    Console.WriteLine($"[UpdateStage] Failed to parse request: {parseEx.Message}");
                    return BadRequest("Invalid request format. Ensure BookingId and SelectedStage are provided.");
                }
                
                if (string.IsNullOrEmpty(selectedStage))
                {
                    Console.WriteLine("[UpdateStage] SelectedStage is empty");
                    return BadRequest("SelectedStage is required.");
                }

                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                {
                    Console.WriteLine($"[UpdateStage] Booking not found: {bookingId}");
                    return BadRequest("Invalid booking ID.");
                }

                // Get user ID - use fallback for now since JWT isn't configured
                var staffUser = await _context.Users.FirstOrDefaultAsync(u => u.RoleId == 2 || u.RoleId == 1);
                if (staffUser == null)
                {
                    Console.WriteLine("[UpdateStage] No staff user found");
                    return BadRequest("No staff user found in the database.");
                }
                var updatedByUserId = staffUser.Id;
                Console.WriteLine($"[UpdateStage] Using staff user ID: {updatedByUserId}");

                // Map stage name to status
                var status = MapStageToStatus(selectedStage);
                Console.WriteLine($"[UpdateStage] Mapped stage '{selectedStage}' to status '{status}'");

                var stage = new BookingStages
                {
                    BookingId = bookingId,
                    StageName = selectedStage,
                    Status = status,
                    TimeStamp = DateTime.UtcNow,
                    UpdatedByUserId = updatedByUserId
                };

                _context.BookingStages.Add(stage);
                Console.WriteLine($"[UpdateStage] Added stage to context");

                // Update booking status to match the stage
                booking.Status = selectedStage;
                _context.Entry(booking).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                Console.WriteLine($"[UpdateStage] Saved changes successfully. New stage ID: {stage.Id}");

                return Ok(new { message = "Booking stage updated successfully.", stageId = stage.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UpdateStage] Exception occurred: {ex.Message}");
                Console.WriteLine($"[UpdateStage] Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = "An error occurred while updating the booking stage.", details = ex.Message });
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