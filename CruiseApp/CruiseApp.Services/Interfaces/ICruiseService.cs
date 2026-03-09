using CruiseApp.Data.Models;
using CruiseApp.Data.Models.Enums;
using CruiseApp.Services.Models.Admin;
using CruiseApp.Services.Models.Cruise;



namespace CruiseApp.Services.Interfaces
{
    public interface ICruiseService
    {
        // ADMIN
        //Task<int> CreateCruiseAsync(
        //    int shipId,
        //    DateOnly firstDay,
        //    DateOnly lastDay);


        // ======================
        // ADMIN
        // ======================

        Task<IEnumerable<AdminCruiseListModel>> GetAllAdminAsync();

        Task CreateAsync(AdminCruiseFormModel model);

        Task<AdminCruiseFormModel?> GetForEditAsync(int id);

        Task UpdateAsync(int id, AdminCruiseFormModel model);

        Task<AdminCruiseListModel?> GetForDeleteAsync(int id);

        Task DeleteAsync(int id);


        // ======================
        // PUBLIC / ANONYMOUS
        // ======================
        Task<IEnumerable<Cruise>> SearchCruisesAsync(
            int? shipId,
            DateOnly? startDate,
            int? startPointId);
        Task<Cruise?> GetByIdAsync(int id);

        // ============================
        // Validation helpers
        // ============================
        Task EnsureUniqueCruiseAsync(int shipId, DateOnly firstDay, DateOnly lastDay, int? ignoreCruiseId = null);

        Task<CabinsServiceModel?> GetCabinsAsync(int cruiseId);

        Task<DeckCabinsServiceModel> GetDeckCabinsAsync(int cruiseId, int deckId, CabinType cabinType);
    }
}
