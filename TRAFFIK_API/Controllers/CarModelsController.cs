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
    [Route("api/CarModels")]
    [ApiController]
    public class CarModelsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarModelsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CarModels
        /// <summary>
        /// Retrieves all car models.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CarModel>>> GetCarModels()
        {
            return await _context.CarModels.ToListAsync();
        }

        // GET: api/CarModels/5
        /// <summary>
        /// Retrieves a specific car model by ID.
        /// </summary>
        /// <param name="id">The ID of the car model to retrieve.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarModel>> GetCarModel(int id)
        {
            var carModel = await _context.CarModels.FindAsync(id);

            if (carModel == null)
            {
                return NotFound();
            }

            return carModel;
        }

        // PUT: api/CarModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates an existing car model.
        /// </summary>
        /// <param name="id">The ID of the car model to update.</param>
        /// <param name="carModel">The updated car model object.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCarModel(int id, CarModel carModel)
        {
            if (id != carModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(carModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarModelExists(id))
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
        /// <param name="carModel">The car model object to create.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CarModel>> PostCarModel(CarModel carModel)
        {
            // Ensure referenced CarType exists if provided
            if (carModel.CarTypeId != 0 && !await _context.CarTypes.AnyAsync(ct => ct.Id == carModel.CarTypeId))
            {
                return BadRequest("Invalid CarTypeId");
            }

            _context.CarModels.Add(carModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarModel", new { id = carModel.Id }, carModel);
        }

        // DELETE: api/CarModels/5
        /// <summary>
        /// Deletes a car model by ID.
        /// </summary>
        /// <param name="id">The ID of the car model to delete.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCarModel(int id)
        {
            var carModel = await _context.CarModels.FindAsync(id);
            if (carModel == null)
            {
                return NotFound();
            }

            _context.CarModels.Remove(carModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks if a car model exists by ID.
        /// </summary>
        /// <param name="id">The ID to check.</param>
        /// <returns>True if the car model exists; otherwise, false.</returns>
        private bool CarModelExists(int id)
        {
            return _context.CarModels.Any(e => e.Id == id);
        }

        // GET: api/CarModels/User/{userId}      gets users vehicles
        /// <summary>
        /// Retrieves all vehicles associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose vehicles to retrieve.</param>
        [HttpGet("User/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CarModel>>> GetUserVehicles(int userId)
        {
            return await _context.CarModels
                .Where(v => v.UserId == userId)
                .Include(v => v.CarType)
                .ToListAsync();
        }
    }
}