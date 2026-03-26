using CruiseApp.ViewModels.Reservation;
using System.ComponentModel.DataAnnotations;

namespace CruiseApp.ViewModels.Reservation
{
    public class PassengerFormViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [MinLength(2)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        [MinLength(2)]
        public string LastName { get; set; } = null!;
    }
}