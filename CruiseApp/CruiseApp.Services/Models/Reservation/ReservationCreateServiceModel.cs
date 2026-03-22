using CruiseApp.Data.Models.Enums;

namespace CruiseApp.Services.Core.Models.Reservation
{
    public class ReservationCreateServiceModel
    {
        public int CruiseId { get; set; }

        public string ShipName { get; set; } = string.Empty;
        public DateOnly FirstDay { get; set; }
        public DateOnly LastDay { get; set; }
        public string StartPoint { get; set; } = string.Empty;
        public int Nights { get; set; }
        public int DeckId { get; set; }
        public int DeckNumber { get; set; }
        public int CabinId { get; set; }
        public string CabinName { get; set; } = null!;
        public CabinType CabinType { get; set; }

        public int PassengersCount { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public string ImageName { get; set; } = null!;

        public string? Description { get; set; }

        public List<PassengerFormServiceModel> Passengers { get; set; }
            = new();
    }
}
