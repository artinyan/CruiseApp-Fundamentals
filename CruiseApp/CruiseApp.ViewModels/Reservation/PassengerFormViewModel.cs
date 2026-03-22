using CruiseApp.ViewModels.Reservation;
using System.ComponentModel.DataAnnotations;

namespace CruiseApp.ViewModels.Reservation
{
    public class PassengerFormViewModel
    {
        public int ReservationId { get; set; }

        public string CabinName { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        public List<PassengerCheckInViewModel> Passengers { get; set; } = new();
    }
}