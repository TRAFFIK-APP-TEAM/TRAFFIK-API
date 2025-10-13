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
    /// <summary>
    /// Controller for managing user-related operations.
    /// </summary>
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user with the specified ID, or 404 if not found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="user">The updated user object.</param>
        /// <returns>No content if successful; 400 if ID mismatch; 404 if user not found.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user object to create.</param>
        /// <returns>The newly created user with a 201 status code.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            user.CreatedAt = user.CreatedAt != default
                ? DateTime.SpecifyKind(user.CreatedAt, DateTimeKind.Utc)
                : DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>No content if successful; 404 if user not found.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks if a user exists by ID.
        /// </summary>
        /// <param name="id">The ID to check.</param>
        /// <returns>True if the user exists; otherwise, false.</returns>
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}