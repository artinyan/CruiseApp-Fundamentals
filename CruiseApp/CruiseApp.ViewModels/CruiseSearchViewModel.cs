using CruiseApp.ViewModels.Cruise;
namespace CruiseApp.ViewModels
{
    public class CruiseSearchViewModel
    {
        public int? ShipId { get; set; }
        public DateOnly? StartDate { get; set; }
        public int? StartPointId { get; set; }

        public IEnumerable<SelectOptionViewModel> Ships { get; set; } = new List<SelectOptionViewModel>();

        public IEnumerable<SelectOptionViewModel> StartPoints { get; set; } = new List<SelectOptionViewModel>();

        public IEnumerable<CruiseListItemViewModel> Cruises { get; set; } = new List<CruiseListItemViewModel>();
    }
}




