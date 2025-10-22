using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TRAFFIK_API.Data;
using TRAFFIK_API.Models;
using TRAFFIK_API.Models.Dtos;
using static TRAFFIK_API.Controllers.RewardsController;

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

            // Redeem points directly here instead of nesting controller
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

            await _context.SaveChangesAsync();
            return Ok(new { redeemed = item.Cost, itemId });
        }
    }
}