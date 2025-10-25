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
        public DbSet<Vehicle> Vehicles { get; set; } = default!;
        public DbSet<CarModel> CarModels { get; set; } = default!;
        public DbSet<VehicleType> VehicleTypes { get; set; } = default!;
        public DbSet<CarModelService> CarModelServices { get; set; } = default!;
        public DbSet<ServiceCatalog> ServiceCatalogs { get; set; } = default!;
        public DbSet<Booking> Bookings { get; set; } = default!;
        public DbSet<BookingStages> BookingStages { get; set; } = default!;
        public DbSet<Notifications> Notifications { get; set; } = default!;
        public DbSet<Payments> Payments { get; set; } = default!;
        public DbSet<UserRole> UserRoles { get; set; } = default!;
        public DbSet<Review> Reviews { get; set; } = default!;
        public DbSet<Reward> Rewards { get; set; } = default!;
        public DbSet<RewardItem> RewardItems { get; set; }
        public DbSet<ServiceHistory> ServiceHistories { get; set; } = default!;
        public DbSet<CarTypeServices> CarTypeServices { get; set; } = default!;

        public DbSet<RewardRedemption> RewardRedemptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarModelService>()
                .HasKey(cms => new { cms.CarModelId, cms.ServiceCatalogId });

            modelBuilder.Entity<CarModelService>()
                .HasOne(cms => cms.CarModel)
                .WithMany(cm => cm.CarModelServices)
                .HasForeignKey(cms => cms.CarModelId);

            modelBuilder.Entity<CarModelService>()
                .HasOne(cms => cms.ServiceCatalog)
                .WithMany(sc => sc.CarModelServices)
                .HasForeignKey(cms => cms.ServiceCatalogId);

            modelBuilder.Entity<CarTypeServices>()
            .HasKey(cts => new { cts.CarTypeId, cts.ServiceCatalogId });

            modelBuilder.Entity<CarTypeServices>()
                .HasOne(cts => cts.CarType)
                .WithMany(ct => ct.CarTypeServices)
                .HasForeignKey(cts => cts.CarTypeId);

            modelBuilder.Entity<CarTypeServices>()
                .HasOne(cts => cts.ServiceCatalog)
                .WithMany(sc => sc.CarTypeServices)
                .HasForeignKey(cts => cts.ServiceCatalogId);

            modelBuilder.Entity<RewardRedemption>()
                .HasOne(r => r.Item)
                .WithMany()
                .HasForeignKey(r => r.ItemId);


            modelBuilder.Entity<CarModel>()
                .HasOne(cm => cm.CarType)
                .WithMany(ct => ct.CarModels)
                .HasForeignKey(cm => cm.CarTypeId);

            modelBuilder.Entity<BookingStages>()
            .HasOne(bs => bs.Booking)
            .WithMany(b => b.BookingStages)
            .HasForeignKey(bs => bs.BookingId);

            modelBuilder.Entity<BookingStages>()
                .HasOne(bs => bs.UpdatedByUser)
                .WithMany()
                .HasForeignKey(bs => bs.UpdatedByUserId);

            // Booking relationships
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.CarModel)
                .WithMany(cm => cm.Bookings)
                .HasForeignKey(b => b.CarModelId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ServiceCatalog)
                .WithMany(sc => sc.Bookings)
                .HasForeignKey(b => b.ServiceCatalogId);

            // ServiceHistory relationships
            modelBuilder.Entity<ServiceHistory>()
                .HasOne(sh => sh.CarModel)
                .WithMany(cm => cm.ServiceHistories)
                .HasForeignKey(sh => sh.CarModelId);

            modelBuilder.Entity<ServiceHistory>()
                .HasOne(sh => sh.ServiceCatalog)
                .WithMany()
                .HasForeignKey(sh => sh.ServiceCatalogId);

            modelBuilder.Entity<ServiceHistory>()
                .HasOne(sh => sh.User)
                .WithMany()
                .HasForeignKey(sh => sh.UserId);

            // Notifications relationships
            modelBuilder.Entity<Notifications>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);

            modelBuilder.Entity<Notifications>()
                .HasOne(n => n.Booking)
                .WithMany(b => b.Notifications)
                .HasForeignKey(n => n.BookingId);

            // Payments relationships
            modelBuilder.Entity<Payments>()
                .HasOne(p => p.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BookingId);

            // Reviews relationships
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Booking)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookingId);

            // Rewards relationships
            modelBuilder.Entity<Reward>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rewards)
                .HasForeignKey(r => r.UserId);

            // ServiceCatalog relationships
            modelBuilder.Entity<ServiceCatalog>()
                .HasOne(sc => sc.CarType)
                .WithMany(ct => ct.Services)
                .HasForeignKey(sc => sc.CarTypeId);

        }
    }
}
