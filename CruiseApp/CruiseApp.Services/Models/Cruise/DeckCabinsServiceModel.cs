namespace CruiseApp.Services.Models.Cruise
{
    public class DeckCabinsServiceModel
    {
        public int ShipId { get; set; }

        public string ShipName { get; set; }

        public int DeckId { get; set; }

        public string DeckName { get; set; }

        public string DeckImage { get; set; }

        public IEnumerable<CabinButtonServiceModel> Cabins { get; set; }
            = new List<CabinButtonServiceModel>();
    }
}
