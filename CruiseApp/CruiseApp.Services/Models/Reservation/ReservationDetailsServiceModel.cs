using CruiseApp.Data.Models.Enums;


namespace CruiseApp.Services.Core.Models.Reservation
{
    public class ReservationDetailsServiceModel
    {
        public int Id { get; set; }

        public int CruiseId { get; set; }

        public string CabinName { get; set; } = null!;

        public decimal Price { get; set; }

        public ReservationStatus Status { get; set; }

        public bool IsPaid { get; set; }

        public List<PassengerDetailsServiceModel> Passengers { get; set; }
            = new();
    }
}