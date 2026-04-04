namespace CruiseApp.Services.Core.Models.Admin
{
    public class AdminPassengerServiceModel
    {
        public int? PassengerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public bool IsCheckedIn { get; set; }
        public string Gender { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        public string Nationality { get; set; } = null!;
        public string PassportNumber { get; set; } = null!;
        public DateOnly? PassportExpirationDate { get; set; }
        public string PassportIssuingCountry { get; set; } = null!;
        public int PassengerOrder { get; set; }
    }
}
