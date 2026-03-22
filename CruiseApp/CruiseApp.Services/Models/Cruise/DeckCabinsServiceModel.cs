using CruiseApp.Data.Models.Enums;

namespace CruiseApp.Services.Core.Models.Cruise
{
    public class DeckCabinsServiceModel
    {
        public int ShipId { get; set; }
        public string ShipName { get; set; } = null!;
        public int DeckId { get; set; }
        public int DeckNumber { get; set; }
        public string DeckName { get; set; } = null!;
        public string DeckImage { get; set; } = null!;
        public CabinType CabinType { get; set; }
        public int CruiseId { get; set; }

        public IEnumerable<CabinButtonServiceModel> Cabins { get; set; }
            = new List<CabinButtonServiceModel>();
    }
}
