namespace TRAFFIK_API.Models
{
    /// <summary>
    /// Represents a reward earned by a user.
    /// </summary>
    public class Reward
    {
        /// <summary>
        /// The unique identifier for the reward.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the user who earned the reward.
        /// </summary>
        public int UserId { get; set; } // Foreign key to User

        /// <summary>
        /// The number of points associated with the reward.
        /// </summary>
        public int Points { get; set; }

        //public DateTime EarnedDate { get; set; }
        //oublic string Description { get; set; } // e.g "Earned for referring a friend"

        /// <summary>
        /// The type or category of the reward.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Indicates whether the reward has been redeemed.
        /// </summary>
        public bool Redeemed { get; set; }
    }
}