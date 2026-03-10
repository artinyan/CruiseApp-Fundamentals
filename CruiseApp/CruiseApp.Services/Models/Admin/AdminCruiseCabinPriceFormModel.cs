using CruiseApp.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CruiseApp.Services.Models.Admin
{
    public class AdminCruiseCabinPriceFormModel
    {
        [Required]
        public CabinType CabinType { get; set; }

        [Required]
        [Range(0.01, 100000)]
        public decimal Price { get; set; }
    }
}
