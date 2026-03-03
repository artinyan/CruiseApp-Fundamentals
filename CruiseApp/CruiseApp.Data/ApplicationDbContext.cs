using CruiseApp.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CruiseApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Point> Points { get; set; } = null!;
        public DbSet<Ship> Ships { get; set; } = null!;
        public DbSet<Route> Routes { get; set; } = null!;
        public DbSet<RouteDay> RouteDays { get; set; } = null!;
        public DbSet<Deck> Decks { get; set; } = null!;
        public DbSet<Cabin> Cabins { get; set; } = null!;
        public DbSet<Cruise> Cruises { get; set; } = null!;
        public DbSet<CruiseLike> CruiseLikes { get; set; } = null!;
        public DbSet<CabinReservation> CabinReservations { get; set; } = null!;
        public DbSet<ReservationPassenger> ReservationPassengers { get; set; } = null!;
        public DbSet<Passenger> Passengers { get; set; } = null!;
        public DbSet<CruiseCabinPrice> CruiseCabinPrices { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ship>()
                .HasIndex(s => s.Name)
                .IsUnique();

            builder.Entity<Route>()
                .HasIndex(r => r.ShipId)
                .IsUnique();

            builder.Entity<Route>()
                .HasOne(r => r.Ship)
                .WithOne()
                .HasForeignKey<Route>(r => r.ShipId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Deck>()
                .HasIndex(d => new { d.ShipId, d.Number })
                .IsUnique();

            builder.Entity<Cabin>()
                .HasIndex(c => new { c.DeckId, c.SequenceNumber })
                .IsUnique();

            builder.Entity<RouteDay>()
                .HasIndex(rd => new { rd.RouteId, rd.Date })
                .IsUnique();

            builder.Entity<RouteDay>()
                .Property(rd => rd.Date)
                .HasColumnType("DATE")
                .HasConversion(
                    v => v.ToDateTime(TimeOnly.MinValue),
                    v => DateOnly.FromDateTime(v));

            builder.Entity<Cruise>()
                .HasOne(c => c.Route)
                .WithMany()
                .HasForeignKey(c => c.RouteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CruiseLike>()
                .HasKey(cl => new { cl.UserId, cl.CruiseId });

            builder.Entity<CabinReservation>()
                .HasIndex(r => new { r.CruiseId, r.CabinId, r.UserId })
                .IsUnique();

            builder.Entity<ReservationPassenger>()
                .HasIndex(rp => new { rp.CabinReservationId, rp.PassengerOrder })
                .IsUnique();

            builder.Entity<CruiseCabinPrice>()
                .HasIndex(p => new { p.CruiseId, p.CabinType })
                .IsUnique();

            builder.Entity<CruiseCabinPrice>()
                .HasOne(p => p.Cruise)
                .WithMany(c => c.CabinPrices)
                .HasForeignKey(p => p.CruiseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
