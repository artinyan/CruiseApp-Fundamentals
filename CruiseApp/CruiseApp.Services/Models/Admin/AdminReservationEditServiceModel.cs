using CruiseApp.Data.Models.Enums;

namespace CruiseApp.Services.Core.Models.Admin
{
    public class AdminReservationEditServiceModel
    {
        public int Id { get; set; }

        public string ReferenceNumber { get; set; }

        public ReservationStatus Status { get; set; }

        public bool IsPaid { get; set; }
        public decimal Price { get; set; }

        public string ShipName { get; set; } = null!;
        public string CabinName { get; set; } = null!;
        public string CabinType { get; set; } = null!;

        public DateOnly FirstDay { get; set; }
        public DateOnly LastDay { get; set; }

        public string UserEmail { get; set; } = null!;

        public List<AdminPassengerServiceModel> Passengers { get; set; }
            = new List<AdminPassengerServiceModel>();
    }
}