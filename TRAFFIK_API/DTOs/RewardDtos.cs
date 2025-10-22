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

    public class RedeemCatalogItemRequest
    {
        public int UserId { get; set; }
        public int ItemId { get; set; }
    }

}