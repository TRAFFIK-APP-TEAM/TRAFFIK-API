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
    [Route("api/ServiceCatalog")]
    [ApiController]
    public class ServiceCatalogsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiceCatalogsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ServiceCatalogs
        /// Retrieves all service catalog entries.
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceCatalog>>> GetServiceCatalogs()
        {
            return await _context.ServiceCatalogs.ToListAsync();
        }

        // GET: api/ServiceCatalogs/5
        /// Retrieves a specific service catalog entry by ID.
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceCatalog>> GetServiceCatalog(int id)
        {
            var serviceCatalog = await _context.ServiceCatalogs.FindAsync(id);

            if (serviceCatalog == null)
            {
                return NotFound();
            }

            return serviceCatalog;
        }

        // PUT: api/ServiceCatalogs/5
        /// Updates an existing service catalog entry.
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutServiceCatalog(int id, ServiceCatalog serviceCatalog)
        {
            if (id != serviceCatalog.Id)
            {
                return BadRequest();
            }

            _context.Entry(serviceCatalog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceCatalogExists(id))
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

        // POST: api/ServiceCatalogs
        /// Creates a new service catalog entry.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceCatalog>> PostServiceCatalog(ServiceCatalog serviceCatalog)
        {
            _context.ServiceCatalogs.Add(serviceCatalog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServiceCatalog", new { id = serviceCatalog.Id }, serviceCatalog);
        }

        // DELETE: api/ServiceCatalogs/5
        /// Deletes a service catalog entry by ID.
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteServiceCatalog(int id)
        {
            var serviceCatalog = await _context.ServiceCatalogs.FindAsync(id);
            if (serviceCatalog == null)
            {
                return NotFound();
            }

            _context.ServiceCatalogs.Remove(serviceCatalog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceCatalogExists(int id)
        {
            return _context.ServiceCatalogs.Any(e => e.Id == id);
        }

        // GET: api/ServiceCatalog/AvailableForVehicle/{carModelId}
        [HttpGet("AvailableForVehicle/{carModelId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ServiceCatalog>>> GetAvailableServices(int carModelId, string? sortBy = "name", string? direction = "asc")
        {
            var carModel = await _context.CarModels
                .Include(cm => cm.CarType)
                .FirstOrDefaultAsync(cm => cm.Id == carModelId);

            if (carModel == null)
                return NotFound("Vehicle not found");

            var carTypeId = carModel.CarTypeId;

            var query = _context.CarTypeServices
                .Where(cts => cts.CarTypeId == carTypeId)
                .Select(cts => cts.ServiceCatalog)
                .Distinct();

            bool desc = string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase);
            if (string.Equals(sortBy, "price", StringComparison.OrdinalIgnoreCase))
            {
                query = desc ? query.OrderByDescending(s => s.Price) : query.OrderBy(s => s.Price);
            }
            else
            {
                query = desc ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name);
            }

            var services = await query.ToListAsync();
            return Ok(services);
        }

        // GET: api/ServiceCatalog/ByCarType/{carTypeId}
        /// <summary>
        /// Retrieves available services for a specific car type id, sorted by price or name.
        /// </summary>
        [HttpGet("ByCarType/{carTypeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceCatalog>>> GetServicesByCarType(int carTypeId, string? sortBy = "name", string? direction = "asc")
        {
            var query = _context.CarTypeServices
                .Where(cts => cts.CarTypeId == carTypeId)
                .Select(cts => cts.ServiceCatalog)
                .Distinct();

            bool desc = string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase);
            if (string.Equals(sortBy, "price", StringComparison.OrdinalIgnoreCase))
            {
                query = desc ? query.OrderByDescending(s => s.Price) : query.OrderBy(s => s.Price);
            }
            else
            {
                query = desc ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name);
            }

            var services = await query.ToListAsync();
            return Ok(services);
        }
    }
}