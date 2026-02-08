
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CruiseApp.Web.ViewModels
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
        public IEnumerable<SelectListItem> Ships { get; set; }
            = new List<SelectListItem>();

        public IEnumerable<SelectListItem> StartPoints { get; set; }
            = new List<SelectListItem>();

        // ============================
        // Search results
        // ============================

        public IEnumerable<CruiseListItemViewModel> Cruises { get; set; }
            = new List<CruiseListItemViewModel>();
    }
}


/////////////////////////////////////////////////


//using CruiseApp.Web.ViewModels;
//using Microsoft.AspNetCore.Mvc.Rendering;

//namespace CruiseApp.Web.Controllers
//{
//    public class CruiseSearchViewModel
//    {
//        // ============================
//        // Search criteria (INPUT)
//        // ============================
//        public int? ShipId { get; set; }
//        public DateOnly? StartDate { get; set; }
//        public int? StartPointId { get; set; }

//        // ============================
//        // Dropdowns (UI data)
//        // ============================
//        public IEnumerable<SelectListItem> Ships { get; set; }
//            = Enumerable.Empty<SelectListItem>();

//        public IEnumerable<SelectListItem> StartPoint { get; set; }
//            = Enumerable.Empty<SelectListItem>();

//        // ============================
//        // Search result (OUTPUT)
//        // ============================
//        public IEnumerable<CruiseListItemViewModel> Cruises { get; set; }
//            = Enumerable.Empty<CruiseListItemViewModel>();

//    }
//}



