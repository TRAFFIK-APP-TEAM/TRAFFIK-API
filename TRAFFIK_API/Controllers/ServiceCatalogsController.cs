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
    [Route("api/ServiceCatalogs")]
    [ApiController]
    public class ServiceCatalogsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiceCatalogsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ServiceCatalogs
        /// <summary>
        /// Retrieves all service catalog entries, optionally filtered by vehicle type.
        /// </summary>
        /// <param name="vehicleTypeId">Optional vehicle type ID to filter services.</param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceCatalog>>> GetServiceCatalogs(int? vehicleTypeId = null)
        {
            var query = _context.ServiceCatalogs.AsQueryable();
            
            if (vehicleTypeId.HasValue)
            {
                // Show services for this vehicle type OR generic services (null vehicle type)
                query = query.Where(sc => sc.VehicleTypeId == vehicleTypeId.Value || sc.VehicleTypeId == null);
            }
            
            return await query.Include(sc => sc.VehicleType).ToListAsync();
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

        /// <summary>
        /// Retrieves available services for a specific vehicle by license plate.
        /// </summary>
        /// <param name="licensePlate">The license plate of the vehicle.</param>
        /// <param name="sortBy">Sort by 'name' or 'price'. Default is 'name'.</param>
        /// <param name="direction">Sort direction 'asc' or 'desc'. Default is 'asc'.</param>
        [HttpGet("ForVehicle/{licensePlate}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ServiceCatalog>>> GetServicesForVehicle(string licensePlate, string? sortBy = "name", string? direction = "asc")
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);

            if (vehicle == null)
                return NotFound("Vehicle not found");

            IQueryable<ServiceCatalog> baseQuery = _context.ServiceCatalogs
                .Where(sc => sc.VehicleTypeId == vehicle.VehicleTypeId || sc.VehicleTypeId == null)
                .Include(sc => sc.VehicleType);

            bool desc = string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase);
            if (string.Equals(sortBy, "price", StringComparison.OrdinalIgnoreCase))
            {
                var services = desc ? await baseQuery.OrderByDescending(s => s.Price).ToListAsync() : await baseQuery.OrderBy(s => s.Price).ToListAsync();
                return Ok(services);
            }
            else
            {
                var services = desc ? await baseQuery.OrderByDescending(s => s.Name).ToListAsync() : await baseQuery.OrderBy(s => s.Name).ToListAsync();
                return Ok(services);
            }
        }

        /// <summary>
        /// Retrieves available services for a specific vehicle type ID.
        /// </summary>
        /// <param name="vehicleTypeId">The ID of the vehicle type.</param>
        /// <param name="sortBy">Sort by 'name' or 'price'. Default is 'name'.</param>
        /// <param name="direction">Sort direction 'asc' or 'desc'. Default is 'asc'.</param>
        [HttpGet("ByVehicleType/{vehicleTypeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceCatalog>>> GetServicesByVehicleType(int vehicleTypeId, string? sortBy = "name", string? direction = "asc")
        {
            IQueryable<ServiceCatalog> baseQuery = _context.ServiceCatalogs
                .Where(sc => sc.VehicleTypeId == vehicleTypeId || sc.VehicleTypeId == null)
                .Include(sc => sc.VehicleType);

            bool desc = string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase);
            if (string.Equals(sortBy, "price", StringComparison.OrdinalIgnoreCase))
            {
                var services = desc ? await baseQuery.OrderByDescending(s => s.Price).ToListAsync() : await baseQuery.OrderBy(s => s.Price).ToListAsync();
                return Ok(services);
            }
            else
            {
                var services = desc ? await baseQuery.OrderByDescending(s => s.Name).ToListAsync() : await baseQuery.OrderBy(s => s.Name).ToListAsync();
                return Ok(services);
            }
        }
    }
}