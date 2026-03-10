namespace CruiseApp.Services.Models.Cruise
{
    public class DeckCabinsServiceModel
    {
        public int DeckId { get; set; }

        public string DeckName { get; set; }

        public IEnumerable<CabinButtonServiceModel> Cabins { get; set; }
            = new List<CabinButtonServiceModel>();
    }
}
