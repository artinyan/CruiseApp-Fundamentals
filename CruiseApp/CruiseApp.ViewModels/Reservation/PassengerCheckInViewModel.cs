using System.ComponentModel.DataAnnotations;

namespace CruiseApp.ViewModels.Reservation
{
    public class PassengerCheckInViewModel
    {
        public int PassengerId { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string Gender { get; set; } = null!;

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public string Nationality { get; set; } = null!;

        [Required]
        public string PassportNumber { get; set; } = null!;

        [Required]
        public DateOnly PassportExpirationDate { get; set; }

        [Required]
        public string PassportIssuingCountry { get; set; } = null!;
    }
}
