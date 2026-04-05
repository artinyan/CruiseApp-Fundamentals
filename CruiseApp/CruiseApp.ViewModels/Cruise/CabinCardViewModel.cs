using CruiseApp.Data.Models.Enums;
using CruiseApp.Common.Infrastructure;
using CruiseApp.ViewModels.Deck;

namespace CruiseApp.ViewModels.Cruise
{
    public class CabinCardViewModel
    {
        public string ShipName { get; set; } = string.Empty;
        public int CruiseId { get; set; }
        public CabinType CabinType { get; set; }

        public string Description =>
            CabinDescriptionProvider.Get(ShipName, CabinType);

        public string CabinImage =>
         $"{ShipName.Replace(" ", "").ToLowerInvariant()}{CabinType.ToString().ToLowerInvariant()}.jpg";

        public decimal Price { get; set; }

        public IEnumerable<DeckButtonViewModel> Decks { get; set; }
            = new List<DeckButtonViewModel>();
    }
}
