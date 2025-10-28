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
                Used = r.Used,
                Code = r.Code,
                UserId = r.UserId
            });

            return Ok(result);
        }

        // NEW ENDPOINT: Get all redeemed rewards (admin)
        [HttpGet("redeemed/all")]
        public async Task<ActionResult<List<RedeemedRewardDto>>> GetAllRedeemed()
        {
            var redeemedRewards = await _context.RewardRedemptions
                .Include(r => r.Item)
                .Select(r => new RedeemedRewardDto
                {
                    ItemId = r.ItemId,
                    Name = r.Item.Name,
                    Description = r.Item.Description,
                    Cost = r.Item.Cost,
                    RedeemedAt = r.RedeemedAt,
                    Used = r.Used,
                    Code = r.Code,
                    UserId = r.UserId
                })
                .OrderByDescending(r => r.RedeemedAt)
                .ToListAsync();

            return Ok(redeemedRewards);
        }

        // NEW ENDPOINT: Mark redemption code as used
        [HttpPut("redeemed/{code}/mark-used")]
        public async Task<ActionResult> MarkAsUsedByCode(string code)
        {
            var redeemedReward = await _context.RewardRedemptions
                .FirstOrDefaultAsync(r => r.Code == code);

            if (redeemedReward == null)
            {
                return NotFound($"Redemption code '{code}' not found.");
            }

            if (redeemedReward.Used)
            {
                return BadRequest("Code has already been deactivated.");
            }

            redeemedReward.Used = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Code deactivated successfully" });
        }



        [HttpPost("redeem/{itemId}")]
        public async Task<ActionResult<RedemptionResponse>> RedeemItem(int itemId, [FromBody] RedeemCatalogItemRequest request)
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

            // Generate unique redemption code
            var redemptionCode = GenerateRedemptionCode();

            _context.RewardRedemptions.Add(new RewardRedemption
            {
                UserId = request.UserId,
                ItemId = itemId,
                Code = redemptionCode,
                RedeemedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return Ok(new RedemptionResponse
            {
                Redeemed = item.Cost,
                ItemId = itemId,
                Code = redemptionCode
            });
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

        // Helper method to generate unique redemption code
        private string GenerateRedemptionCode()
        {
            // Random alphanumeric (10 characters, readable)
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var code = new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            
            // Check if code already exists (very unlikely but be safe)
            var exists = _context.RewardRedemptions.Any(r => r.Code == code);
            if (exists)
            {
                return GenerateRedemptionCode(); // Recursively generate until unique
            }

            return code;
        }
    }
}