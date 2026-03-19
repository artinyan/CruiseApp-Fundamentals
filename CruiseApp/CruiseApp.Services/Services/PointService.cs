using CruiseApp.Data;
using CruiseApp.Data.Models;
using CruiseApp.Services.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CruiseApp.Services.Core.Services
{
    public class PointService : IPointService
    {
        private readonly ApplicationDbContext db;
        public PointService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Point>> GetAllAsync()
        {
            return await db.Points
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
    }
}

