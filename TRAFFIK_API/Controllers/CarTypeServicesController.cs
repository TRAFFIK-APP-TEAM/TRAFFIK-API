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
    [Route("api/[controller]")]
    [ApiController]
    public class CarTypeServicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarTypeServicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CarTypeServices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarTypeServices>>> GetCarTypeServices()
        {
            return await _context.CarTypeServices.ToListAsync();
        }

        // GET: api/CarTypeServices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CarTypeServices>> GetCarTypeServices(int id)
        {
            var carTypeServices = await _context.CarTypeServices.FindAsync(id);

            if (carTypeServices == null)
            {
                return NotFound();
            }

            return carTypeServices;
        }

        // PUT: api/CarTypeServices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarTypeServices(int id, CarTypeServices carTypeServices)
        {
            if (id != carTypeServices.CarTypeId)
            {
                return BadRequest();
            }

            _context.Entry(carTypeServices).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarTypeServicesExists(id))
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

        // POST: api/CarTypeServices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CarTypeServices>> PostCarTypeServices(CarTypeServices carTypeServices)
        {
            _context.CarTypeServices.Add(carTypeServices);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CarTypeServicesExists(carTypeServices.CarTypeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCarTypeServices", new { id = carTypeServices.CarTypeId }, carTypeServices);
        }

        // DELETE: api/CarTypeServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarTypeServices(int id)
        {
            var carTypeServices = await _context.CarTypeServices.FindAsync(id);
            if (carTypeServices == null)
            {
                return NotFound();
            }

            _context.CarTypeServices.Remove(carTypeServices);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarTypeServicesExists(int id)
        {
            return _context.CarTypeServices.Any(e => e.CarTypeId == id);
        }
    }
}
