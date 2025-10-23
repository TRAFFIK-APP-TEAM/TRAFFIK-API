using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TRAFFIK_API.Data;
using TRAFFIK_API.Models;
using TRAFFIK_API.Models.Dtos;

namespace TRAFFIK_API.Controllers
{
    [Route("api/RewardCatalog")]
    [ApiController]
    public class RewardCatalogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RewardCatalogController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RewardItem>>> GetCatalog()
        {
            return await _context.RewardItems.ToListAsync();
        }

        [HttpGet("user/{userId}/redeemed")]
        public async Task<ActionResult<IEnumerable<RedeemedRewardDto>>> GetRedeemedItems(int userId)
        {
            var redemptions = await _context.RewardRedemptions
                .Where(r => r.UserId == userId)
                .Include(r => r.Item)
                .ToListAsync();

            var result = redemptions.Select(r => new RedeemedRewardDto
            {
                ItemId = r.ItemId,
                Name = r.Item.Name,
                Description = r.Item.Description,
                Cost = r.Item.Cost,
                RedeemedAt = r.RedeemedAt,
                Used = r.Used
            });

            return Ok(result);
        }



        [HttpPost("redeem/{itemId}")]
        public async Task<ActionResult> RedeemItem(int itemId, [FromBody] RedeemCatalogItemRequest request)
        {
            var item = await _context.RewardItems.FindAsync(itemId);
            if (item == null) return NotFound();

            var availablePoints = await _context.Rewards
                .Where(r => r.UserId == request.UserId && !r.Redeemed)
                .SumAsync(r => (int?)r.Points) ?? 0;

            if (availablePoints < item.Cost)
                return BadRequest("Insufficient points");

            var redeemRequest = new RedeemRewardRequest
            {
                UserId = request.UserId,
                Points = item.Cost
            };

            var unredeemed = await _context.Rewards
                .Where(r => r.UserId == request.UserId && !r.Redeemed)
                .OrderBy(r => r.Id)
                .ToListAsync();

            int remaining = redeemRequest.Points;
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
                    r.Points -= remaining;
                    remaining = 0;
                }
            }

            _context.RewardRedemptions.Add(new RewardRedemption
            {
                UserId = request.UserId,
                ItemId = itemId,
                RedeemedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return Ok(new { redeemed = item.Cost, itemId });
        }

        [HttpPost("user/{userId}/redeemed/{itemId}/use")]
        public async Task<IActionResult> MarkAsUsed(int userId, int itemId)
        {
            var redemption = await _context.RewardRedemptions
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ItemId == itemId && !r.Used);

            if (redemption == null)
                return NotFound("Redemption not found or already marked as used.");

            redemption.Used = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}