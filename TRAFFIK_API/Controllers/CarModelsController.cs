using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TRAFFIK_API.Data;
using TRAFFIK_API.DTOs;
using TRAFFIK_API.Models;

namespace TRAFFIK_API.Controllers
{
    public class CarModelCreateRequest
    {
        public int UserId { get; set; }
        public string VehicleType { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string PlateNumber { get; set; } = string.Empty;
        public int Year { get; set; }
    }
    [Route("api/CarModels")]
    [ApiController]
    public class CarModelsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarModelsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new car model for a user.
        /// </summary>
        /// <param name="carModel">The car model to create.</param>
        /// <returns>The created car model.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CarModel>> PostCarModel(CarModel carModel)
        {
            _context.CarModels.Add(carModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarModel", new { id = carModel.Id }, carModel);
        }

        /// <summary>
        /// Gets a car model by ID.
        /// </summary>
        /// <param name="id">The ID of the car model.</param>
        /// <returns>The car model.</returns>
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

        /// <summary>
        /// Gets car models for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>List of car models for the user.</returns>
        [HttpGet("User/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CarModel>>> GetUserCarModels(int userId)
        {
            return await _context.CarModels
                .Where(cm => cm.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Creates or gets a default car model for a user and vehicle.
        /// </summary>
        /// <param name="request">The car model creation request.</param>
        /// <returns>The car model object.</returns>
        [HttpPost("CreateOrGet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CarModelDto>> CreateOrGetCarModel(CarModelCreateRequest request)
        {
            var existingCarModel = await _context.CarModels
                .Include(cm => cm.CarType)
                .FirstOrDefaultAsync(cm =>
                    cm.UserId == request.UserId &&
                    cm.Make == request.Make &&
                    cm.Model == request.Model &&
                    cm.PlateNumber == request.PlateNumber);

            CarModel carModel;

            if (existingCarModel != null)
            {
                carModel = existingCarModel;
            }
            else
            {
                var carType = await _context.VehicleTypes.FirstOrDefaultAsync(vt => vt.Type == request.VehicleType);
                if (carType == null)
                {
                    carType = new VehicleType { Type = request.VehicleType };
                    _context.VehicleTypes.Add(carType);
                    await _context.SaveChangesAsync();
                }

                carModel = new CarModel
                {
                    UserId = request.UserId,
                    CarTypeId = carType.Id,
                    Make = request.Make,
                    Model = request.Model,
                    PlateNumber = request.PlateNumber,
                    Year = request.Year
                };

                _context.CarModels.Add(carModel);
                await _context.SaveChangesAsync();
            }

            var dto = new CarModelDto
            {
                Id = carModel.Id,
                UserId = carModel.UserId,
                VehicleType = carModel.CarType?.Type ?? request.VehicleType,
                Make = carModel.Make,
                Model = carModel.Model,
                PlateNumber = carModel.PlateNumber,
                Year = carModel.Year
            };

            return Ok(dto);
        }
    }
}
