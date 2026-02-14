using CruiseApp.Data.Models;
using CruiseApp.Services.Models.Admin;



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
    }
}
