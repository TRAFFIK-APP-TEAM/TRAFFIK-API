namespace TRAFFIK_API.Models
{
    public class RewardItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Cost { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class RewardRedemption
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public DateTime RedeemedAt { get; set; }
        public bool Used { get; set; } = false;

        public RewardItem Item { get; set; }
    }
}