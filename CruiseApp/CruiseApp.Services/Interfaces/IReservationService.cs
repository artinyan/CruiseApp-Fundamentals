using CruiseApp.Services.Core.Models.Reservation;

namespace CruiseApp.Services.Core.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationCreateServiceModel> GetCreateModelAsync(int cabinId, int cruiseId);

        Task CreateReservationAsync(string userId, ReservationCreateServiceModel model);

        Task<IEnumerable<MyReservationServiceModel>> GetUserReservationsAsync(string userId);

        Task<ReservationDetailsServiceModel?> GetReservationDetailsAsync(int reservationId);

        Task CheckInAsync(int reservationId, List<PassengerCheckInServiceModel> passengers);

        Task<bool> IsCabinAvailableAsync(int cruiseId, int cabinId);
    }
}
