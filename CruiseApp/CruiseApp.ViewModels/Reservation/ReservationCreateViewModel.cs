using CruiseApp.Data.Models.Enums;

namespace CruiseApp.ViewModels.Reservation
{
    public class ReservationCreateViewModel
    {
        public int CruiseId { get; set; }

        public string? ShipName { get; set; } = string.Empty;

        public DateOnly FirstDay { get; set; }

        public DateOnly LastDay { get; set; }

        public string? StartPoint { get; set; } = string.Empty;

        public int Nights { get; set; }

        public int DeckId { get; set; }

        public int DeckNumber { get; set; }

        public int CabinId { get; set; }

        public string? CabinName { get; set; } = null!;
        public CabinType CabinType { get; set; }

        public int Capacity { get; set; }

        public int PassengersCount { get; set; }

        public decimal Price { get; set; }

        public string? ImageName { get; set; } = null!;
        public string? Description { get; set; } = null!;

        public List<PassengerFormViewModel> Passengers { get; set; }
            = new List<PassengerFormViewModel>();
    }
}
