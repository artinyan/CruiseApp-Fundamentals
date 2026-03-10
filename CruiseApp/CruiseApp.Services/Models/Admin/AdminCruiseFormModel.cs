using System.ComponentModel.DataAnnotations;

namespace CruiseApp.Services.Models.Admin
{
    public class AdminCruiseFormModel
    {
        [Required]
        public int ShipId { get; set; }

        [Required]
        public string ShipName { get; set; } = string.Empty;

        [Required(ErrorMessage = "First day is required")]
        public DateOnly FirstDay { get; set; }


        [Required(ErrorMessage = "Last day is required")]
        public DateOnly LastDay { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public ICollection<AdminCruiseCabinPriceFormModel> CabinPrices { get; set; }
            = new List<AdminCruiseCabinPriceFormModel>();

    }
}


//using System.ComponentModel.DataAnnotations;

//namespace CruiseApp.Services.Models.Admin
//{
//    public class AdminCruiseFormModel
//    {
//        [Required]
//        public int ShipId { get; set; }

//        [Required]
//        public string ShipName { get; set; } = string.Empty;

//        [Required(ErrorMessage = "First day is required")]
//        public DateOnly FirstDay { get; set; }


//        [Required(ErrorMessage = "Last day is required")]
//        public DateOnly LastDay { get; set; }

//        [StringLength(1000)]
//        public string? Description { get; set; }
//    }
//}
