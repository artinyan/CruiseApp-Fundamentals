using CruiseApp.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CruiseApp.Web.ViewModels.Admin
{
    public class AdminCruiseCabinPriceViewModel
    {
        [Required]
        public CabinType CabinType { get; set; }

        [Required]
        [Range(0.01, 100000)]
        public decimal Price { get; set; }
    }
}
