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
    }
}
