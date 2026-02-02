using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CruiseApp.Data.Models;

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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ship>()
                .HasIndex(s => s.Name)
                .IsUnique();

            builder.Entity<Route>()
                .HasIndex(r => r.ShipId)
                .IsUnique();

            builder.Entity<Deck>()
                .HasIndex(d => new { d.ShipId, d.Number })
                .IsUnique();

            builder.Entity<Cabin>()
                .HasIndex(c => new { c.DeckId, c.SequenceNumber })
                .IsUnique();

            builder.Entity<RouteDay>()
                .HasIndex(rd => new { rd.RouteId, rd.Date })
                .IsUnique();
        }
    }
}
