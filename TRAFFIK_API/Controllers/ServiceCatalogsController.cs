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
        /// <summary>
        /// Retrieves all service catalog entries.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceCatalog>>> GetServiceCatalogs()
        {
            return await _context.ServiceCatalogs.ToListAsync();
        }

        // GET: api/ServiceCatalogs/5
        /// <summary>
        /// Retrieves a specific service catalog entry by ID.
        /// </summary>
        /// <param name="id">The ID of the service catalog item to retrieve.</param>
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates an existing service catalog entry.
        /// </summary>
        /// <param name="id">The ID of the service catalog item to update.</param>
        /// <param name="serviceCatalog">The updated service catalog object.</param>
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a new service catalog entry.
        /// </summary>
        /// <param name="serviceCatalog">The service catalog object to create.</param>
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
        /// <summary>
        /// Deletes a service catalog entry by ID.
        /// </summary>
        /// <param name="id">The ID of the service catalog item to delete.</param>
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

        // for now doesnt work, must add selection logic later
        // GET: api/ServiceCatalog/AvailableForVehicle/{carModelId} 
        /// <summary>
        /// Retrieves available services for a specific vehicle model.
        /// </summary>
        /// <param name="carModelId">The ID of the car model to check services for.</param>
        /// 
        //GET /api/ServiceCatalog/AvailableForVehicle/{carModelId}
        [HttpGet("AvailableForVehicle/{carModelId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ServiceCatalog>>> GetAvailableServices(int carModelId)
        {

          var carModel = await _context.CarModels
        .Include(cm => cm.CarModelServices)
        .ThenInclude(cms => cms.ServiceCatalog)
        .FirstOrDefaultAsync(cm => cm.Id == carModelId);

            if (carModel == null)
                return NotFound("Vehicle not found");

            var compatibleServices = carModel.CarModelServices
                .Select(cms => cms.ServiceCatalog)
                .ToList();

            return Ok(compatibleServices);

        }
    }
}