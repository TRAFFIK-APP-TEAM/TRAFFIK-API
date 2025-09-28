using System;
using System.ComponentModel.DataAnnotations;

namespace TRAFFIK_API.Models
{
    /// <summary>
    /// Represents a user in the TRAFFIK system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the role assigned to the user.
        /// </summary>
        public int RoleId { get; set; } // Foreign key to Role

        /// <summary>
        /// The user's full name.
        /// </summary>
        [Required]
        public string FullName { get; set; }

        /// <summary>
        /// The user's email address.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// The hashed password for the user.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }

        /// <summary>
        /// The user's phone number.
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The date and time the user was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Indicates whether the user is currently active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}