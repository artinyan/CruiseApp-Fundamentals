using CruiseApp.Web.ViewModels.Cruise;

namespace CruiseApp.Web.ViewModels.Cruise
{
    public class DeckCabinsViewModel
    {
        public string ShipName { get; set; } = string.Empty;

        public string DeckName { get; set; } = string.Empty;
        public string DeckImage {  get; set; } = string.Empty;

        public List<CabinButtonViewModel> Cabins { get; set; } = new();
    }
}