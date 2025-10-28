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
    [Route("api/VehicleTypes")]
    [ApiController]
    public class VehicleTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VehicleTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/VehicleTypes
        /// <summary>
        /// Retrieves all vehicle types.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VehicleType>>> GetVehicleTypes()
        {
            return await _context.VehicleTypes.ToListAsync();
        }

        // GET: api/VehicleTypes/5
        /// <summary>
        /// Retrieves a specific vehicle type by ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VehicleType>> GetVehicleType(int id)
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(id);

            if (vehicleType == null)
            {
                return NotFound();
            }

            return vehicleType;
        }

        // PUT: api/VehicleTypes/5
        /// <summary>
        /// Updates an existing vehicle type.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutVehicleType(int id, VehicleType vehicleType)
        {
            if (id != vehicleType.Id)
            {
                return BadRequest();
            }

            _context.Entry(vehicleType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleTypeExists(id))
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

        // POST: api/VehicleTypes
        /// <summary>
        /// Creates a new vehicle type.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VehicleType>> PostVehicleType(VehicleType vehicleType)
        {
            _context.VehicleTypes.Add(vehicleType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicleType", new { id = vehicleType.Id }, vehicleType);
        }

        // DELETE: api/VehicleTypes/5
        /// <summary>
        /// Deletes a vehicle type by ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVehicleType(int id)
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            _context.VehicleTypes.Remove(vehicleType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehicleTypeExists(int id)
        {
            return _context.VehicleTypes.Any(e => e.Id == id);
        }
    }
}

