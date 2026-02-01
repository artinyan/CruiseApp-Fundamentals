using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CruiseApp.Data.Models
{
    [Comment("Represents Deck of the Ship in the system.")]
    public class Deck
    {
        [Key]
        [Comment("Primary key for Deck.")]
        public int Id { get; set; }

        [Required]
        [Range (1, 99, ErrorMessage = "Deck number must be between 1 and 99.")] // Second Validation. (first sould be javascript frontend), third sould be in migration
        [Comment("Numeric identifier of the Deck.")]
        public int Number { get; set; }

        [NotMapped]
        public string Name => Number.ToString("D2");

        [Required]
        [Comment("The Ship of the Deck.")]
        public int ShipId { get; set; }

        [ForeignKey(nameof(ShipId))]
        public Ship Ship { get; set; } = null!;

        public ICollection<Cabin> Cabins { get; set; } = new List<Cabin>();
    }
}
