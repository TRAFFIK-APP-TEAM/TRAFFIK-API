using Microsoft.AspNetCore.Mvc;
using TRAFFIK_API.Data;
using TRAFFIK_API.Models;
using System.Security.Cryptography;
using System.Text;

namespace TRAFFIK_API.Auth
{

    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
            {
                return BadRequest("Email already in use.");
            }


            // Get the next available ID manually
            var maxId = _context.Users.Max(u => (int?)u.Id) ?? 0;
            var nextId = maxId + 1;

            var user = new User
            {
                Id = nextId, // Use the next available ID
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                RoleId = dto.RoleId, // assign default role or from dto
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                PasswordHash = HashPassword(dto.Password)  //dto.Password = Plain password user sends in
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                id = user.Id,
                fullName = user.FullName,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
                roleId = user.RoleId
            });
        }

        /// <summary>
        /// Authenticates a user and returns basic profile info.
        /// </summary>
        /// <param name="dto">User login credentials.</param>
        /// <returns>User profile if login is successful.</returns>
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            try
            {
                var user = await Task.Run(() => _context.Users.SingleOrDefault(u => u.Email == dto.Email));
                if (user == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                var hashedPassword = HashPassword(dto.Password);
                if (user.PasswordHash != hashedPassword)
                {
                    return Unauthorized("Invalid email or password.");
                }

                //FUTURE JWT WILL BE IMPLEMENTED HERE
                return Ok(new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    user.PhoneNumber,
                    user.RoleId
                });
            }
            catch (Exception ex)
            {
                // Log the error and return 500 with details
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>Success message.</returns>
        [HttpPost("Logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Logout()
        {
            // No server-side session or token to invalidate yet
            return Ok("Logged out successfully.");
        }

        /// <summary>
        /// Deletes a user account by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>Success message if deletion is successful.</returns>
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User account deleted.");
        }

        private string HashPassword(string rawData)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        // DTOs (Transfers data between APP and API) Over Here, keeping it here for now for simplicity
        /// <summary>
        /// Data transfer object for user registration.
        /// </summary>
        public class UserRegisterDto
        {
            /// <summary>
            /// The full name of the user.
            /// </summary>
            public string FullName { get; set; }

            /// <summary>
            /// The email address of the user.
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// The plain text password of the user.
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// The phone number of the user.
            /// </summary>
            public string PhoneNumber { get; set; }

            /// <summary>
            /// The role ID to assign to the user.
            /// </summary>
            public int RoleId { get; set; }
        }

        /// <summary>
        /// Data transfer object for user login.
        /// </summary>
        public class UserLoginDto
        {
            /// <summary>
            /// The email address of the user.
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// The plain text password of the user.
            /// </summary>
            public string Password { get; set; }
        }
    }
}