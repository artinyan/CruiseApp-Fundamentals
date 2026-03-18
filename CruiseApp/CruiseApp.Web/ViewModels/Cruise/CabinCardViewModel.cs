using CruiseApp.Data.Models.Enums;
using CruiseApp.Web.Infrastructure;
using CruiseApp.Web.ViewModels.Deck;

namespace CruiseApp.Web.ViewModels.Cruise
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

        // дековете, които имат този тип кабина
        public IEnumerable<DeckButtonViewModel> Decks { get; set; }
            = new List<DeckButtonViewModel>();
    }
}
