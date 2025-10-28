namespace TRAFFIK_API.Models
{
    /// <summary>
    /// Represents a redemption of a reward item by a user.
    /// </summary>
    public class RewardRedemption
    {
        /// <summary>
        /// The unique identifier for the redemption.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the user who redeemed the item.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The ID of the reward item that was redeemed.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// The unique redemption code for this redemption.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The date and time when the item was redeemed.
        /// </summary>
        public DateTime RedeemedAt { get; set; }

        /// <summary>
        /// Indicates whether the redeemed item has been used.
        /// </summary>
        public bool Used { get; set; } = false;

        /// <summary>
        /// Navigation property to the user who made the redemption.
        /// </summary>
        public User User { get; set; } = null!;

        /// <summary>
        /// Navigation property to the reward item that was redeemed.
        /// </summary>
        public RewardItem Item { get; set; } = null!;
    }
}
