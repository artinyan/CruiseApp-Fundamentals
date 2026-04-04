using CruiseApp.Data.Models.Enums;

namespace CruiseApp.ViewModels.Admin
{
    public class AdminReservationEditViewModel
    {
        public string? ReferenceNumber { get; set; }
        public AdminReservationDetailsViewModel? Reservation { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class AdminReservationDetailsViewModel
    {
        public int Id { get; set; }
        public string CabinName { get; set; } = null!;
        public string ShipName { get; set; } = null!;
        public ReservationStatus Status { get; set; }
        public bool IsPaid { get; set; }
        public DateOnly FirstDay { get; set; }
        public DateOnly LastDay { get; set; }
        public List<PassengerViewModel> Passengers { get; set; } = new();
    }

    public class PassengerViewModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string Nationality { get; set; } = null!;
        public string PassportNumber { get; set; } = null!;
        public DateOnly PassportExpirationDate { get; set; }
        public string PassportIssuingCountry { get; set; } = null!;
        public bool IsCheckedIn { get; set; }
        public string PassportExpirationDateString { get; set; }
        public string DateOfBirthString { get; set; }
    }
}