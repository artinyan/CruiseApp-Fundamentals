using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CruiseApp.Data.Models
{
    [Comment("Represents likes for a cruise.")]
    public class CruiseLike
    {
        [Required]
        [Comment("User who liked the cruise.")]
        public string UserId { get; set; } = null!;

        [Required]
        [Comment("Cruise which is liked from the user.")]
        public int CruiseId { get; set; }

        [ForeignKey(nameof(CruiseId))]
        public Cruise Cruise { get; set; } = null!;
    }
}

