namespace TRAFFIK_API.Models
{
    /// <summary>
    /// Represents a role assigned to a user.
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// The unique identifier for the user role.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the role.
        /// </summary>
        public string RoleName { get; set; } // e.g "Admin", "Customer", "Employee"
    }
}