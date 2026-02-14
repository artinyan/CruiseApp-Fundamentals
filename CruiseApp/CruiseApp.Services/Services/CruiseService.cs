using CruiseApp.Data;
using CruiseApp.Data.Models;
using CruiseApp.Services.Interfaces;
using CruiseApp.Services.Models.Admin;
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
        // PUBLIC
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

            if (shipId.HasValue)
                query = query.Where(c => c.Route.ShipId == shipId.Value);

            if (startDate.HasValue)
                query = query.Where(c => c.FirstDay == startDate.Value);

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
                    .ThenInclude(r => r.Ship)
                .Include(c => c.Route)
                    .ThenInclude(r => r.Days)
                        .ThenInclude(d => d.Point)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // ============================
        // ADMIN
        // ============================

        public async Task<IEnumerable<AdminCruiseListModel>> GetAllAdminAsync()
        {
            return await db.Cruises
                .Include(c => c.Route)
                    .ThenInclude(r => r.Ship)
                .OrderByDescending(c => c.FirstDay)
                .Select(c => new AdminCruiseListModel
                {
                    Id = c.Id,
                    ShipName = c.Route.Ship.Name,
                    FirstDay = c.FirstDay,
                    LastDay = c.LastDay,
                    CruiseLength = c.CruiseLength
                })
                .ToListAsync();
        }

        public async Task CreateAsync(AdminCruiseFormModel model)
        {
            var route = await db.Routes
                .Include(r => r.Days)
                .ThenInclude(d => d.Point)
                .FirstOrDefaultAsync(r => r.ShipId == model.ShipId);

            if (route == null)
                throw new InvalidOperationException("Route not found for ship.");

            var cruise = new Cruise(route, model.FirstDay, model.LastDay);

            db.Cruises.Add(cruise);
            await db.SaveChangesAsync();
        }

        public async Task<AdminCruiseFormModel?> GetForEditAsync(int id)
        {
            return await db.Cruises
                .Where(c => c.Id == id)
                .Select(c => new AdminCruiseFormModel
                {
                    ShipId = c.Route.ShipId,
                    FirstDay = c.FirstDay,
                    LastDay = c.LastDay
                })
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(int id, AdminCruiseFormModel model)
        {
            var cruise = await db.Cruises
                .Include(c => c.Route)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cruise == null)
                throw new InvalidOperationException("Cruise not found.");

            cruise.ChangePeriod(model.FirstDay, model.LastDay);

            await db.SaveChangesAsync();
        }

        public async Task<AdminCruiseListModel?> GetForDeleteAsync(int id)
        {
            return await db.Cruises
                .Include(c => c.Route)
                    .ThenInclude(r => r.Ship)
                .Where(c => c.Id == id)
                .Select(c => new AdminCruiseListModel
                {
                    Id = c.Id,
                    ShipName = c.Route.Ship.Name,
                    FirstDay = c.FirstDay,
                    LastDay = c.LastDay,
                    CruiseLength = c.CruiseLength
                })
                .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cruise = await db.Cruises.FindAsync(id);

            if (cruise != null)
            {
                db.Cruises.Remove(cruise);
                await db.SaveChangesAsync();
            }
        }
    }
}
