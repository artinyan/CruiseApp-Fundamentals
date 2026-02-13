using CruiseApp.Data;
using CruiseApp.Data.Models;
using Microsoft.EntityFrameworkCore;

public class CruiseLikeService : ICruiseLikeService
{
    private readonly ApplicationDbContext db;

    public CruiseLikeService(ApplicationDbContext db)
    {
        this.db = db;
    }

    public async Task LikeAsync(int cruiseId, string userId)
    {
        if (await IsLikedAsync(cruiseId, userId))
            return;

        db.CruiseLikes.Add(new CruiseLike
        {
            CruiseId = cruiseId,
            UserId = userId
        });

        await db.SaveChangesAsync();
    }

    public async Task UnlikeAsync(int cruiseId, string userId)
    {
        var like = await db.CruiseLikes
            .FirstOrDefaultAsync(l => l.CruiseId == cruiseId && l.UserId == userId);

        if (like != null)
        {
            db.CruiseLikes.Remove(like);
            await db.SaveChangesAsync();
        }
    }

    public async Task<bool> IsLikedAsync(int cruiseId, string userId)
        => await db.CruiseLikes
            .AnyAsync(l => l.CruiseId == cruiseId && l.UserId == userId);

    public async Task<int> GetLikesCountAsync(int cruiseId)
        => await db.CruiseLikes.CountAsync(l => l.CruiseId == cruiseId);

    public async Task<IEnumerable<Cruise>> GetLikedCruisesAsync(string userId)
    {
        return await db.CruiseLikes
            .Where(cl => cl.UserId == userId)
            .Include(cl => cl.Cruise)
                .ThenInclude(c => c.Route)
                    .ThenInclude(r => r.Ship)
            .Include(cl => cl.Cruise)
                .ThenInclude(c => c.Route)
                    .ThenInclude(r => r.Days)
                        .ThenInclude(d => d.Point)
            .Select(cl => cl.Cruise)
            .ToListAsync();
    }
}
