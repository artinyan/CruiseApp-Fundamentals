using System.ComponentModel.DataAnnotations;

namespace CruiseApp.Services.Models.Admin
{
    public class AdminCruiseFormModel
    {
        [Required]
        public int ShipId { get; set; }

        [Required]
        public DateOnly FirstDay { get; set; }

        [Required]
        public DateOnly LastDay { get; set; }
    }
}
