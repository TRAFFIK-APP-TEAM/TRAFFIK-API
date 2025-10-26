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
    [Route("api/RewardItems")]
    [ApiController]
    public class RewardItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RewardItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/RewardItems
        /// <summary>
        /// Retrieves all reward items.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RewardItem>>> GetRewardItems()
        {
            return await _context.RewardItems.ToListAsync();
        }

        // GET: api/RewardItems/5
        /// <summary>
        /// Retrieves a specific reward item by ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RewardItem>> GetRewardItem(int id)
        {
            var rewardItem = await _context.RewardItems.FindAsync(id);

            if (rewardItem == null)
            {
                return NotFound();
            }

            return rewardItem;
        }

        // PUT: api/RewardItems/5
        /// <summary>
        /// Updates an existing reward item.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutRewardItem(int id, RewardItem rewardItem)
        {
            if (id != rewardItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(rewardItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RewardItemExists(id))
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

        // POST: api/RewardItems
        /// <summary>
        /// Creates a new reward item.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RewardItem>> PostRewardItem(RewardItem rewardItem)
        {
            _context.RewardItems.Add(rewardItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRewardItem", new { id = rewardItem.Id }, rewardItem);
        }

        // DELETE: api/RewardItems/5
        /// <summary>
        /// Deletes a reward item by ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRewardItem(int id)
        {
            var rewardItem = await _context.RewardItems.FindAsync(id);
            if (rewardItem == null)
            {
                return NotFound();
            }

            _context.RewardItems.Remove(rewardItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RewardItemExists(int id)
        {
            return _context.RewardItems.Any(e => e.Id == id);
        }
    }
}

