using CruiseApp.Data.Models.Enums;
using CruiseApp.Web.Infrastructure;

namespace CruiseApp.Web.ViewModels.Cruise
{
    public class CabinCardViewModel
    {
        public string ShipName { get; set; } = string.Empty;
        public CabinType CabinType { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description =>
            CabinDescriptionProvider.Get(ShipName, CabinType);

        public string CabinImage =>
         $"{ShipName.Replace(" ", "").ToLowerInvariant()}{CabinType.ToString().ToLowerInvariant()}.jpg";

        public decimal Price { get; set; }
    }
}
