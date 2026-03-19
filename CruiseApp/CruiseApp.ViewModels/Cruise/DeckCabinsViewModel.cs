using CruiseApp.Data.Models.Enums;

namespace CruiseApp.ViewModels.Cruise
{
    public class DeckCabinsViewModel
    {
        public string ShipName { get; set; } = string.Empty;
        public int DeckId { get; set; }
        public int DeckNumber { get; set; }
        public string DeckName { get; set; } = string.Empty;
        public string DeckImage {  get; set; } = string.Empty;
        public CabinType CabinType { get; set; }
        public int CruiseId { get; set; }

        public List<CabinButtonViewModel> Cabins { get; set; } = new();
    }
}