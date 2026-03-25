namespace CruiseApp.Services.Core.Models.Reservation
{
    public class PassengerDetailsServiceModel
    {
        public int? PassengerId { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public bool IsCheckedIn { get; set; }
    }
}
