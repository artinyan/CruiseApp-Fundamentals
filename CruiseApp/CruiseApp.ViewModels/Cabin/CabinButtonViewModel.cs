
using CruiseApp.Data.Models.Enums;

namespace CruiseApp.ViewModels.Cruise
{
    public class CabinButtonViewModel
    {
        public int Id { get; set; }

        public string Number { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public int PosX { get; set; }
        public int PosY { get; set; }

        public CabinType CabinType { get; set; }

        public bool IsAvailable { get; set; }

        public string CssClass =>
            CabinType switch
            {
                CabinType.Interior => "cabin-blue",
                CabinType.SeaView => "cabin-green",
                CabinType.Balcony => "cabin-orange",
                CabinType.Suite => "cabin-purple",
                _ => "cabin-gray"
            };
    }
}