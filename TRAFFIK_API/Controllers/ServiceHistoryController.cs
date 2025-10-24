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
            var vehicle = await _context.CarModels.FindAsync(dto.CarModelId);
            var service = await _context.ServiceCatalogs.FindAsync(dto.ServiceCatalogId);

            if (user == null || vehicle == null || service == null)
                return BadRequest("Invalid user, vehicle, or service ID.");

            var wash = new ServiceHistory
            {
                CarModelId = dto.CarModelId,
                ServiceCatalogId = dto.ServiceCatalogId,
                CompletedAt = DateTime.UtcNow,
                UserId = dto.UserId
            };

            _context.ServiceHistories.Add(wash);
            await _context.SaveChangesAsync();

            return Ok("Wash tracked successfully.");
        }

        [HttpGet("CarModel/{carModelId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ServiceHistory>>> GetServiceHistoryByCar(int carModelId)
        {
            var vehicle = await _context.CarModels.FindAsync(carModelId);
            if (vehicle == null)
                return NotFound("CarModel not found.");

            var history = await _context.ServiceHistories
                .Include(h => h.User)
                .Include(h => h.ServiceCatalog)
                .Where(h => h.CarModelId == carModelId)
                .ToListAsync();

            return Ok(history);
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceHistory>>> GetAllServiceHistory()
        {
            var history = await _context.ServiceHistories
                .Include(h => h.User)
                .Include(h => h.CarModel)
                .Include(h => h.ServiceCatalog)
                .ToListAsync();

            return Ok(history);
        }
    }
}