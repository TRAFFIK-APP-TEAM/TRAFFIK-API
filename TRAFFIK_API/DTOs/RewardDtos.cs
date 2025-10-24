namespace TRAFFIK_API.Models.Dtos
{
    public class RedeemRewardRequest
    {
        public int UserId { get; set; }
        public int Points { get; set; }
    }
    public class EarnRewardRequest
    {
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public decimal AmountSpent { get; set; }
    }

        public class RedeemedRewardDto
        {
            public int ItemId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public int Cost { get; set; }
            public DateTime RedeemedAt { get; set; }
            public bool Used { get; set; }
        }


        public class RedeemCatalogItemRequest
        {
            public int UserId { get; set; }
        }


}