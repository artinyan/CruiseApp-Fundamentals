using CruiseApp.Data.Models.Enums;

namespace CruiseApp.ViewModels.Reservation
{
    public class MyReservationViewModel
    {
        public int Id { get; set; }
        public int CruiseId { get; set; }
        public string CabinName { get; set; } = null!;
        public decimal Price { get; set; }
        public ReservationStatus Status { get; set; }
        public bool IsPaid { get; set; }
    }
}
