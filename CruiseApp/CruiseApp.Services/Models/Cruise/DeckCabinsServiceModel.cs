using CruiseApp.Web.ViewModels.Cruise;

namespace CruiseApp.Services.Models.Cruise
{
    public class DeckCabinsServiceModel
    {
        public int DeckId { get; set; }

        public string DeckName { get; set; }

        public IEnumerable<CabinButtonViewModel> Cabins { get; set; }
            = new List<CabinButtonViewModel>();
    }
}
