using CruiseApp.Data.Models.Enums;

namespace CruiseApp.Services.Core.Models.Reservation
{
    public class MyReservationServiceModel
    {
        public int Id { get; set; }
        public int CruiseId { get; set; }
        public string CabinName { get; set; } = null!;
        public decimal Price { get; set; }
        public ReservationStatus Status { get; set; }
        public bool IsPaid { get; set; }
        public string ShipName { get; set; } = null!;
        public string StartPoint { get; set; } = null!;
        public DateOnly FirstDay { get; set; }
        public DateOnly LastDay { get; set; }
        public int Nights { get; set; }
        public string? CruiseDiscription { get; set; }
        public int DeckNumber { get; set; }
        public CabinType CabinType { get; set; }
        public string? CabinDiscription { get; set; }
        public string Destinations { get; set; } = string.Empty;
        public string PointImage => $"{StartPoint.ToLower()}.jpg";
    }
}
