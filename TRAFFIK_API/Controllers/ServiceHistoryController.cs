using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRAFFIK_API.Data;
using TRAFFIK_API.Models;
using TRAFFIK_API.DTOs;

namespace TRAFFIK_API.Controllers
{
    [Route("api/ServiceHistory")]
    [ApiController]
    public class ServiceHistoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiceHistoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("TrackWash")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TrackService([FromBody] ServiceHistoryDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            var vehicle = await _context.Vehicles.FindAsync(dto.VehicleLicensePlate);
            var service = await _context.ServiceCatalogs.FindAsync(dto.ServiceCatalogId);

            if (user == null || vehicle == null || service == null)
                return BadRequest("Invalid user, vehicle, or service ID.");

            var wash = new ServiceHistory
            {
                VehicleLicensePlate = dto.VehicleLicensePlate,
                ServiceCatalogId = dto.ServiceCatalogId,
                CompletedAt = DateTime.UtcNow,
                UserId = dto.UserId
            };

            _context.ServiceHistories.Add(wash);
            await _context.SaveChangesAsync();

            return Ok("Wash tracked successfully.");
        }

        [HttpGet("Vehicle/{vehicleLicensePlate}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ServiceHistory>>> GetServiceHistoryByCar(string vehicleLicensePlate)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleLicensePlate);
            if (vehicle == null)
                return NotFound("Vehicle not found.");

            var history = await _context.ServiceHistories
                .Include(h => h.User)
                .Include(h => h.ServiceCatalog)
                .Where(h => h.VehicleLicensePlate == vehicleLicensePlate)
                .ToListAsync();

            return Ok(history);
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceHistory>>> GetAllServiceHistory()
        {
            var history = await _context.ServiceHistories
                .Include(h => h.User)
                .Include(h => h.Vehicle)
                .Include(h => h.ServiceCatalog)
                .ToListAsync();

            return Ok(history);
        }
    }
}