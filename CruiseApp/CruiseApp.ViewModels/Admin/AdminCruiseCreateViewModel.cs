using CruiseApp.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace CruiseApp.ViewModels.Admin
{
    public class AdminCruiseCreateViewModel
    {
        public int Id { get; set; }

        [Required]
        public int ShipId { get; set; }

        //[Required]
        //public int RouteId { get; set; }

        [Required(ErrorMessage = "First day is required")]
        public DateOnly FirstDay { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Last day is required")]
        [DateRange("FirstDay", ErrorMessage = "Last day must be after first day")]
        public DateOnly LastDay { get; set; }

        public List<AdminCruiseCabinPriceViewModel> CabinPrices { get; set; }
            = new();
    }
}
