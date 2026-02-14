using System.ComponentModel.DataAnnotations;
namespace CruiseApp.Web.ViewModels.Admin
{
    public class AdminCruiseFormViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ShipId { get; set; }

        [Required]
        public int RouteId { get; set; }

        [Required]
        public DateOnly FirstDay { get; set; }

        [Required]
        public DateOnly LastDay { get; set; }
    }
}
