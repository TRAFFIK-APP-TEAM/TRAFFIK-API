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
        public DbSet<VehicleType> VehicleTypes { get; set; } = default!;
        public DbSet<ServiceCatalog> ServiceCatalogs { get; set; } = default!;
        public DbSet<Booking> Bookings { get; set; } = default!;
        public DbSet<BookingStages> BookingStages { get; set; } = default!;
        public DbSet<Notifications> Notifications { get; set; } = default!;
        public DbSet<Payments> Payments { get; set; } = default!;
        public DbSet<UserRole> UserRoles { get; set; } = default!;
        public DbSet<Reward> Rewards { get; set; } = default!;
        public DbSet<RewardItem> RewardItems { get; set; }
        public DbSet<ServiceHistory> ServiceHistories { get; set; } = default!;
        public DbSet<RewardRedemption> RewardRedemptions { get; set; }
        public DbSet<InstagramPost> InstagramPosts { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // RewardRedemption relationships
            modelBuilder.Entity<RewardRedemption>()
                .HasOne(r => r.Item)
                .WithMany(i => i.Redemptions)
                .HasForeignKey(r => r.ItemId);

            modelBuilder.Entity<RewardRedemption>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            // RewardRedemption unique index on Code
            modelBuilder.Entity<RewardRedemption>()
                .HasIndex(r => r.Code)
                .IsUnique();

            // User to UserRole relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);

            // User to Vehicles relationship
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.User)
                .WithMany()
                .HasForeignKey(v => v.UserId);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.VehicleType)
                .WithMany(vt => vt.Vehicles)
                .HasForeignKey(v => v.VehicleTypeId);

            // BookingStages relationships
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
                .HasOne(b => b.ServiceCatalog)
                .WithMany(sc => sc.Bookings)
                .HasForeignKey(b => b.ServiceCatalogId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Vehicle)
                .WithMany(v => v.Bookings)
                .HasForeignKey(b => b.VehicleLicensePlate)
                .OnDelete(DeleteBehavior.SetNull);

            // Notifications relationships
            modelBuilder.Entity<Notifications>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);

            modelBuilder.Entity<Notifications>()
                .HasOne(n => n.Booking)
                .WithMany(b => b.Notifications)
                .HasForeignKey(n => n.BookingId)
                .OnDelete(DeleteBehavior.SetNull);

            // Payments relationships
            modelBuilder.Entity<Payments>()
                .HasOne(p => p.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BookingId);

            // Rewards relationships
            modelBuilder.Entity<Reward>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rewards)
                .HasForeignKey(r => r.UserId);

            // ServiceHistory relationships
            modelBuilder.Entity<ServiceHistory>()
                .HasOne(sh => sh.Vehicle)
                .WithMany(v => v.ServiceHistories)
                .HasForeignKey(sh => sh.VehicleLicensePlate);

            modelBuilder.Entity<ServiceHistory>()
                .HasOne(sh => sh.ServiceCatalog)
                .WithMany()
                .HasForeignKey(sh => sh.ServiceCatalogId);

            modelBuilder.Entity<ServiceHistory>()
                .HasOne(sh => sh.User)
                .WithMany()
                .HasForeignKey(sh => sh.UserId);

            // VehicleType relationships
            modelBuilder.Entity<ServiceCatalog>()
                .HasOne(sc => sc.VehicleType)
                .WithMany(vt => vt.Services)
                .HasForeignKey(sc => sc.VehicleTypeId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
