using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TRAFFIK_API.Data;
using TRAFFIK_API.Models;
using TRAFFIK_API.Models.Dtos;

namespace TRAFFIK_API.Controllers
{
    [Route("api/Reward")]
    [ApiController]
    public class RewardsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RewardsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Rewards
        /// <summary>
        /// Retrieves all rewards.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Reward>>> GetRewards()
        {
            return await _context.Rewards.ToListAsync();
        }

        // GET: api/Rewards/5
        /// <summary>
        /// Retrieves a specific reward by ID.
        /// </summary>
        /// <param name="id">The ID of the reward to retrieve.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Reward>> GetReward(int id)
        {
            var reward = await _context.Rewards.FindAsync(id);

            if (reward == null)
            {
                return NotFound();
            }

            return reward;
        }

        // PUT: api/Rewards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates an existing reward.
        /// </summary>
        /// <param name="id">The ID of the reward to update.</param>
        /// <param name="reward">The updated reward object.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutReward(int id, Reward reward)
        {
            if (id != reward.Id)
            {
                return BadRequest();
            }

            _context.Entry(reward).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RewardExists(id))
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

        // POST: api/Rewards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a new reward.
        /// </summary>
        /// <param name="reward">The reward object to create.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Reward>> PostReward(Reward reward)
        {
            _context.Rewards.Add(reward);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReward", new { id = reward.Id }, reward);
        }

        // DELETE: api/Rewards/5
        /// <summary>
        /// Deletes a reward by ID.
        /// </summary>
        /// <param name="id">The ID of the reward to delete.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReward(int id)
        {
            var reward = await _context.Rewards.FindAsync(id);
            if (reward == null)
            {
                return NotFound();
            }

            _context.Rewards.Remove(reward);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RewardExists(int id)
        {
            return _context.Rewards.Any(e => e.Id == id);
        }

        [HttpGet("User/{userId}/balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetUserRewardBalance(int userId)
        {
            var balance = await _context.Rewards
                .Where(r => r.UserId == userId && !r.Redeemed)
                .SumAsync(r => (int?)r.Points) ?? 0;
            return Ok(balance);
        }

        // POST: api/Reward/earn
        /// Adds reward points for a user.
        [HttpPost("earn")]
        public async Task<ActionResult<Reward>> Earn(EarnRewardRequest request)
        {
            if (request.AmountSpent <= 0)
                return BadRequest("Amount must be positive");

            // Example: 1 point per R10 spent
            int pointsEarned = (int)(request.AmountSpent / 10);

            if (pointsEarned <= 0)
                return BadRequest("Amount too low to earn points");

            var reward = new Reward
            {
                UserId = request.UserId,
                Points = pointsEarned,
                Type = "Booking",
                Redeemed = false
            };

            _context.Rewards.Add(reward);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReward), new { id = reward.Id }, reward);
        }


        // POST: api/Reward/redeem
        /// <summary>
        /// Redeems a number of reward points for a user, consuming oldest unredeemed rewards first.
        /// </summary>
        [HttpPost("redeem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Redeem(RedeemRewardRequest request)
        {
            if (request.Points <= 0)
                return BadRequest("Points must be positive");

            var availablePoints = await _context.Rewards
                .Where(r => r.UserId == request.UserId && !r.Redeemed)
                .SumAsync(r => (int?)r.Points) ?? 0;

            if (availablePoints < request.Points)
                return BadRequest("Insufficient points");

            // Simple strategy: mark full reward rows as redeemed until covered
            var unredeemed = await _context.Rewards
                .Where(r => r.UserId == request.UserId && !r.Redeemed)
                .OrderBy(r => r.Id)
                .ToListAsync();

            int remaining = request.Points;
            foreach (var r in unredeemed)
            {
                if (remaining <= 0) break;

                if (r.Points <= remaining)
                {
                    remaining -= r.Points;
                    r.Redeemed = true;
                }
                else
                {
                    // Split record: reduce current points and create a redeemed row for the consumed part
                    int consumed = remaining;
                    r.Points -= consumed;
                    remaining = 0;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { redeemed = request.Points });
        }
    }
}