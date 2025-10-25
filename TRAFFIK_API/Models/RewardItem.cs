namespace TRAFFIK_API.Models
{
    /// <summary>
    /// Represents an item in the reward catalog that can be redeemed with points.
    /// </summary>
    public class RewardItem
    {
        /// <summary>
        /// The unique identifier for the reward item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the reward item.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The description of the reward item.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The cost in points required to redeem this item.
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// The image URL for the reward item.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Indicates whether this item is currently available for redemption.
        /// </summary>
        public bool IsAvailable { get; set; } = true;

        /// <summary>
        /// Navigation property to reward redemptions.
        /// </summary>
        public ICollection<RewardRedemption> Redemptions { get; set; } = new List<RewardRedemption>();
    }
}