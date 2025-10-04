using Microsoft.EntityFrameworkCore;
using TRAFFIK_API.Models;

namespace TRAFFIK_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<CarModel> CarModels { get; set; } = default!;
        public DbSet<CarModelService> CarModelServices { get; set; } = default!;
        public DbSet<ServiceCatalog> ServiceCatalogs { get; set; } = default!;
        public DbSet<Booking> Bookings { get; set; } = default!;
        public DbSet<BookingStages> BookingStages { get; set; } = default!;
        public DbSet<Notifications> Notifications { get; set; } = default!;
        public DbSet<Payments> Payments { get; set; } = default!;
        public DbSet<UserRole> UserRoles { get; set; } = default!;
        public DbSet<Review> Reviews { get; set; } = default!;
        public DbSet<Reward> Rewards { get; set; } = default!;
        public DbSet<ServiceHistory> ServiceHistories { get; set; } = default!;

        // Override OnModelCreating if you need to configure relationships, keys, etc.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarModelService>()
                .HasKey(cms => new { cms.CarModelId, cms.ServiceCatalogId });
            modelBuilder.Entity<BookingStages>()
            .HasOne(bs => bs.Booking)
            .WithMany(b => b.BookingStages)
            .HasForeignKey(bs => bs.BookingId);

            modelBuilder.Entity<BookingStages>()
                .HasOne(bs => bs.UpdatedByUser)
                .WithMany()
                .HasForeignKey(bs => bs.UpdatedByUserId);
        }
    }
}
