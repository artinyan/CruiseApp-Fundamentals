using CruiseApp.Data;
using CruiseApp.Data.Models;
using CruiseApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CruiseApp.Services.Services
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

