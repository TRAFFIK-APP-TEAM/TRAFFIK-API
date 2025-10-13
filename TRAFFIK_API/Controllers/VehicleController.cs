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
    [Route("api/Vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VehicleController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Vehicle
        /// <summary>
        /// Retrieves all vehicles.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Models.Vehicle>>> GetVehicles()
        {
            return await _context.Vehicles.ToListAsync();
        }

        // GET: api/Vehicle/5 ?? api/Vehicle/{licensePlate} UHHH DOESNT REALLY MAKE SENSE BUT IM PROBS SLEEP DEPRIVED ATP
        /// <summary>
        /// Retrieves a specific vehicle by License Plate.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Models.Vehicle>> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return vehicle;
        }

        // PUT: api/CarModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates an existing Vehicle.
        /// </summary>
        /// <param name="licensePlate">The ID of the car model to update.</param>
        /// <param name="Vehicle">The updated car model object.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutVehicle(int id, Models.Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return BadRequest();
            }

            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Vehicle.lExists(id))
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

        // POST: api/CarModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a new car model.
        /// </summary>
        /// <param name="vehicle">The car model object to create.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Models.Vehicle>> PosVehicle(Models.Vehicle vehicle)
        {
            // Ensure referenced CarType exists if provided
            if (vehicle.CarTypeId != 0 && !await _context.Vehicles.AnyAsync(ct => ct.Id == vehicle.CarTypeId))
            {
                return BadRequest("Invalid CarTypeId");
            }

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicle", new { id = vehicle.Id }, vehicle);
        }

        // DELETE: api/CarModels/5
        /// <summary>
        /// Deletes a car model by ID.
        /// </summary>
        /// <param name="id">The ID of the car model to delete.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (carModel == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks if a car model exists by ID.
        /// </summary>
        /// <param name="id">The ID to check.</param>
        /// <returns>True if the car model exists; otherwise, false.</returns>
        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }

        // GET: api/CarModels/User/{userId}      gets users vehicles
        /// <summary>
        /// Retrieves all vehicles associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose vehicles to retrieve.</param>
        [HttpGet("User/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Models.Vehicle>>> GetUserVehicles(int userId)
        {
            return await _context.Vehicle
                .Where(v => v.UserId == userId)
                .Include(v => v.VehicleType)
                .ToListAsync();
        }
    }
}