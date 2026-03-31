using CruiseApp.ViewModels.Cruise;
namespace CruiseApp.ViewModels
{
    public class CruiseSearchViewModel
    {
        // ============================
        // Search criteria (input)
        // ============================
        public int? ShipId { get; set; }
        public DateOnly? StartDate { get; set; }
        public int? StartPointId { get; set; }

        // ============================
        // Dropdown data
        // ============================
        //public IEnumerable<SelectListItem> Ships { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectOptionViewModel> Ships { get; set; } = new List<SelectOptionViewModel>();

        public IEnumerable<SelectOptionViewModel> StartPoints { get; set; } = new List<SelectOptionViewModel>();

        // ============================
        // Search results
        // ============================

        public IEnumerable<CruiseListItemViewModel> Cruises { get; set; } = new List<CruiseListItemViewModel>();
    }
}




