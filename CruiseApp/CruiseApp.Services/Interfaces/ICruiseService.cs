using CruiseApp.Data.Models;

namespace CruiseApp.Services.Interfaces
{
    public interface ICruiseService
    {
        // ADMIN
        Task<int> CreateCruiseAsync(
            int shipId,
            DateOnly firstDay,
            DateOnly lastDay);

        // PUBLIC / ANONIMUS
        Task<IEnumerable<Cruise>> SearchCruisesAsync(
            int? shipId,
            DateOnly? startDate,
            int? startPointId);
    }
}
