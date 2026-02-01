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
        [StringLength(100)]
        [Unicode(false)]
        [Comment("Name of the Route")]
        public string Name { get; set; } = null!;

        [Required]
        [Comment("The Ship of the Route.")]
        public int ShipId { get; set; }

        [ForeignKey(nameof(ShipId))]
        public Ship Ship { get; set; } = null!;

        public ICollection<RouteDay> Days { get; set; } = new List<RouteDay>();
    }
}
