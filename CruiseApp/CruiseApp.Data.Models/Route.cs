using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CruiseApp.Data.Models
{
    [Comment("Represents Route in the system.")]
    public class Route
    {
        [Key]
        [Comment("Primary key for Route.")]
        public int Id { get; set; }

        [Required]
        [Comment("The Ship of the Route.")]
        public int ShipId { get; set; }

        [ForeignKey(nameof(ShipId))]
        public Ship Ship { get; set; } = null!;

        public ICollection<RouteDay> Days { get; set; } = new List<RouteDay>();


        [NotMapped]
        public string Name => Ship != null? $"Route of {Ship.Name}" : "Route";
    }
}
