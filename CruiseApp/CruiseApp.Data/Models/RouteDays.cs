using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CruiseApp.Data.Models
{
    [Comment("Represents RouteDay in the system.")]
    public class RouteDay
    {
        [Key]
        [Comment("Primary key for RouteDay.")]
        public int Id { get; set; }

        [Required]
        [Comment("Calendar date of the RouteDay.")]
        public DateOnly Date { get; set; }

        [Required]
        [Comment("The Route of the RouteDay.")]
        public int RouteId { get; set; }

        [ForeignKey(nameof(RouteId))]
        public Route Route { get; set; } = null!;


        [Required]
        [Comment("The Point of the RouteDay.")]
        public int PointId { get; set; }

        [ForeignKey(nameof(PointId))]
        public Point Point { get; set; } = null!;
    }
}