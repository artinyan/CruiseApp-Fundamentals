using CruiseApp.Data.Models.Enums;

namespace CruiseApp.ViewModels.Reservation
{
    public class CheckInViewModel
    {
        public int ReservationId { get; set; }

        public ReservationStatus Status { get; set; }

        public List<PassengerCheckInViewModel> Passengers { get; set; }
            = new List<PassengerCheckInViewModel>();
    }
}
