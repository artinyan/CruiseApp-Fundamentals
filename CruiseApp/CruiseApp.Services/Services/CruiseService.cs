using CruiseApp.Data;
using CruiseApp.Data.Models;
using CruiseApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CruiseApp.Services.Services
{
    public class CruiseService : ICruiseService
    {
        private readonly ApplicationDbContext db;

        public CruiseService(ApplicationDbContext db)
        {
            this.db = db;
        }

        // ============================
        // ADMIN
        // ============================
        public async Task<int> CreateCruiseAsync(int shipId, DateOnly firstDay, DateOnly lastDay)
        {
            var route = await db.Routes
                .Include(r => r.Days)
                .ThenInclude(rd => rd.Point)
                .FirstOrDefaultAsync(r => r.ShipId == shipId);

            if (route == null)
                throw new InvalidOperationException("Route for ship not found.");

            var cruise = new Cruise(route, firstDay, lastDay);

            db.Cruises.Add(cruise);
            await db.SaveChangesAsync();

            return cruise.Id;
        }


        // ============================
        // PUBLIC SEARCH
        // ============================

        public async Task<IEnumerable<Cruise>> SearchCruisesAsync(
            int? shipId, DateOnly? startDate, int? startPointId)
        {
            var query = db.Cruises
                 .AsNoTracking()
                 .Include(c => c.Route)
                 .ThenInclude(r => r.Ship)
                 .Include(c => c.Route)
                 .ThenInclude(r => r.Days)
                 .ThenInclude(d => d.Point)
                 .AsQueryable();

            // Ship filter
            if (shipId.HasValue)
            {
                query = query.Where(c => c.Route.ShipId == shipId.Value);
            }

            // First Date
            if (startDate.HasValue)
            {
                query = query.Where(c => c.FirstDay == startDate.Value);
            }

            // First Point
            if (startPointId.HasValue)
            {
                query = query.Where(c =>
                    c.Route.Days.Any(d =>
                    d.Date == c.FirstDay &&
                    d.PointId == startPointId.Value));
            }


            return await query
                .OrderBy(c => c.FirstDay)
                .ToListAsync();
        }
        public async Task<Cruise?> GetByIdAsync(int id)
        {
            return await db.Cruises
                .Include(c => c.Route)
                    .ThenInclude(c => c.Ship)
                .Include(c => c.Route)
                    .ThenInclude(r => r.Days)
                        .ThenInclude(d => d.Point)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
