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
    [Route("api/vehicle")]
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
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
        {
            return await _context.Vehicles.ToListAsync();
        }

        // GET: api/Vehicle/{licensePlate}
        /// <summary>
        /// Retrieves a specific vehicle by License Plate.
        /// </summary>
        /// <param name="licensePlate">The license plate of the vehicle to retrieve.</param>
        [HttpGet("{licensePlate}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Vehicle>> GetVehicle(string licensePlate)
        {
            var vehicle = await _context.Vehicles.FindAsync(licensePlate);

            if (vehicle == null)
            {
                return NotFound();
            }

            return vehicle;
        }

        // PUT: api/Vehicle/{licensePlate}
        /// <summary>
        /// Updates an existing Vehicle.
        /// </summary>
        /// <param name="licensePlate">The license plate of vehicle to update.</param>
        /// <param name="vehicleDto">The updated vehicle DTO.</param>
        [HttpPut("{licensePlate}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutVehicle(string licensePlate, VehicleUpdateDto vehicleDto)
        {
            if (licensePlate != vehicleDto.LicensePlate)
            {
                return BadRequest("License plate mismatch");
            }

            var vehicle = await _context.Vehicles.FindAsync(licensePlate);
            if (vehicle == null)
            {
                return NotFound();
            }

            // Check if vehicle type exists
            var vehicleType = await _context.VehicleTypes.FindAsync(vehicleDto.VehicleTypeId);
            if (vehicleType == null) return BadRequest("Vehicle type not found");

            // Update vehicle properties
            vehicle.Make = vehicleDto.Make;
            vehicle.Model = vehicleDto.Model;
            vehicle.ImageUrl = vehicleDto.ImageUrl;
            vehicle.VehicleTypeId = vehicleDto.VehicleTypeId;
            vehicle.Color = vehicleDto.Color;
            vehicle.Year = vehicleDto.Year;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(licensePlate))
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

        // POST: api/Vehicle
        /// <summary>
        /// Creates a new vehicle.
        /// </summary>
        /// <param name="vehicleDto">The vehicle DTO to create.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Vehicle>> PostVehicle(VehicleCreateDto vehicleDto)
        {
            // Check if user exists
            var user = await _context.Users.FindAsync(vehicleDto.UserId);
            if (user == null) return BadRequest("User not found");
            
            // Check if vehicle type exists
            var vehicleType = await _context.VehicleTypes.FindAsync(vehicleDto.VehicleTypeId);
            if (vehicleType == null) return BadRequest("Vehicle type not found");
            
            // Create vehicle entity from DTO
            var vehicle = new Vehicle
            {
                UserId = vehicleDto.UserId,
                Make = vehicleDto.Make,
                Model = vehicleDto.Model,
                LicensePlate = vehicleDto.LicensePlate,
                ImageUrl = vehicleDto.ImageUrl,
                VehicleTypeId = vehicleDto.VehicleTypeId,
                Color = vehicleDto.Color,
                Year = vehicleDto.Year,
                User = user
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehicle), new { licensePlate = vehicle.LicensePlate }, vehicle);
        }

        // DELETE: api/Vehicle/{licensePlate}
        /// <summary>
        /// Deletes a vehicle by license plate.
        /// </summary>
        /// <param name="licensePlate">The license plate of the vehicle to delete.</param>
        [HttpDelete("{licensePlate}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVehicle(string licensePlate)
        {
            var vehicle = await _context.Vehicles.FindAsync(licensePlate);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks if vehicle exists by license plate.
        /// </summary>
        /// <param name="licensePlate">The license plate to check.</param>
        /// <returns>True if the vehicle exists; otherwise, false.</returns>
        private bool VehicleExists(string licensePlate)
        {
            return _context.Vehicles.Any(e => e.LicensePlate == licensePlate);
        }

        // GET: api/Vehicle/User/{userId}
        /// <summary>
        /// Retrieves all vehicles associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose vehicles to retrieve.</param>
        [HttpGet("User/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetUserVehicles(int userId)
        {
            return await _context.Vehicles
                .Where(v => v.UserId == userId)
                .ToListAsync();
        }

        // GET: api/Vehicle/Types
        /// <summary>
        /// Retrieves all vehicle types from the database.
        /// </summary>
        [HttpGet("Types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VehicleType>>> GetVehicleTypes()
        {
            var types = await _context.VehicleTypes
                .OrderBy(v => v.Type)
                .ToListAsync();

            return Ok(types);
        }

    }
}