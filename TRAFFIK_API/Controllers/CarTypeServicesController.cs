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

        // GET: api/CarTypeServices/{carTypeId}/{serviceCatalogId}
        [HttpGet("{carTypeId:int}/{serviceCatalogId:int}")]
        public async Task<ActionResult<CarTypeServices>> GetCarTypeServices(int carTypeId, int serviceCatalogId)
        {
            var carTypeServices = await _context.CarTypeServices.FindAsync(carTypeId, serviceCatalogId);

            if (carTypeServices == null)
            {
                return NotFound();
            }

            return carTypeServices;
        }

        // PUT: api/CarTypeServices/{carTypeId}/{serviceCatalogId}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{carTypeId:int}/{serviceCatalogId:int}")]
        public async Task<IActionResult> PutCarTypeServices(int carTypeId, int serviceCatalogId, CarTypeServices carTypeServices)
        {
            if (carTypeId != carTypeServices.CarTypeId || serviceCatalogId != carTypeServices.ServiceCatalogId)
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
                if (!CarTypeServicesExists(carTypeId, serviceCatalogId))
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
                if (CarTypeServicesExists(carTypeServices.CarTypeId, carTypeServices.ServiceCatalogId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetCarTypeServices), new { carTypeId = carTypeServices.CarTypeId, serviceCatalogId = carTypeServices.ServiceCatalogId }, carTypeServices);
        }

        // DELETE: api/CarTypeServices/{carTypeId}/{serviceCatalogId}
        [HttpDelete("{carTypeId:int}/{serviceCatalogId:int}")]
        public async Task<IActionResult> DeleteCarTypeServices(int carTypeId, int serviceCatalogId)
        {
            var carTypeServices = await _context.CarTypeServices.FindAsync(carTypeId, serviceCatalogId);
            if (carTypeServices == null)
            {
                return NotFound();
            }

            _context.CarTypeServices.Remove(carTypeServices);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarTypeServicesExists(int carTypeId, int serviceCatalogId)
        {
            return _context.CarTypeServices.Any(e => e.CarTypeId == carTypeId && e.ServiceCatalogId == serviceCatalogId);
        }
    }
}
