namespace CruiseApp.Web.ViewModels.Cruise
{
    public class CabinsViewModel
    {
        public int CruiseId { get; set; }

        public string ShipName { get; set; } = string.Empty;

        public DateOnly FirstDay { get; set; }

        public DateOnly LastDay { get; set; }

        public string StartPoint { get; set; } = string.Empty;

        public int Nights { get; set; }

        public ICollection<CabinCardViewModel> Cabins { get; set; }
            = new List<CabinCardViewModel>();
    }
}
