using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CruiseApp.Data.Models
{
    [Comment("Represent Point in the system.")]
    public class Point
    {
        [Key]
        [Comment("Primary key for the Point.")]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Unicode(false)]
        [Comment("Name of the Point.")]
        public string Name { get; set; } = null!;

        [Comment("Indicates whether the Point is at sea.")]
        public bool IsSea { get; set; }

    }
}