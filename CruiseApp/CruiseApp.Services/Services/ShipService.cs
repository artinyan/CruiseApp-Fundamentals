using CruiseApp.Data;
using CruiseApp.Data.Models;
using CruiseApp.Services.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CruiseApp.Services.Core.Services
{
    public class ShipService : IShipService
    {
        private readonly ApplicationDbContext db;

        public ShipService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Ship>> GetAllAsync()
        {
            return await db.Ships
                .AsNoTracking()
                .ToListAsync();
        }
    }
}