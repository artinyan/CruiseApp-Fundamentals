using CruiseApp.Data.Models;

public interface ICruiseLikeService
{
    Task LikeAsync(int cruiseId, string userId);
    Task UnlikeAsync(int cruiseId, string userId);
    Task<bool> IsLikedAsync(int cruiseId, string userId);
    Task<int> GetLikesCountAsync(int cruiseId);
    Task<IEnumerable<Cruise>> GetLikedCruisesAsync(string userId);
}
