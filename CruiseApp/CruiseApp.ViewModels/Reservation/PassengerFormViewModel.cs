using CruiseApp.ViewModels.Reservation;
using System.ComponentModel.DataAnnotations;

namespace CruiseApp.ViewModels.Reservation
{
    public class PassengerFormViewModel
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;
    }
}