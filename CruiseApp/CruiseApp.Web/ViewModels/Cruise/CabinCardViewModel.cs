using CruiseApp.Data.Models.Enums;

namespace CruiseApp.Web.ViewModels.Cruise
{
    public class CabinCardViewModel
    {
        public string ShipName { get; set; } = string.Empty;
        public CabinType CabinType { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string CabinImage =>
         $"{ShipName.Replace(" ", "").ToLowerInvariant()}{CabinType.ToString().ToLowerInvariant()}.jpg";
        //$"{ShipName.Replace(" ", "").ToLower()}{CabinType.ToString().ToLower()}.jpg";

        public decimal Price { get; set; }
    }
}
