using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TRAFFIK_API.Data;
using TRAFFIK_API.Models;

namespace TRAFFIK_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CarTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleType>>> GetCarTypes()
        {
            return await _context.VehicleTypes.ToListAsync();
        }

        // GET: api/CarTypes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleType>> GetCarType(int id)
        {
            var carType = await _context.VehicleTypes.FindAsync(id);

            if (carType == null)
            {
                return NotFound();
            }

            return carType;
        }

        // POST: api/CarTypes
        [HttpPost]
        public async Task<ActionResult<VehicleType>> PostCarType(VehicleType carType)
        {
            _context.VehicleTypes.Add(carType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCarType), new { id = carType.Id }, carType);
        }

        // PUT: api/CarTypes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarType(int id, VehicleType carType)
        {
            if (id != carType.Id)
            {
                return BadRequest();
            }

            _context.Entry(carType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarTypeExists(id))
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

        // DELETE: api/CarTypes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarType(int id)
        {
            var carType = await _context.VehicleTypes.FindAsync(id);
            if (carType == null)
            {
                return NotFound();
            }

            _context.VehicleTypes.Remove(carType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarTypeExists(int id)
        {
            return _context.VehicleTypes.Any(e => e.Id == id);
        }
    }
}
