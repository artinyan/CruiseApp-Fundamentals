using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CruiseApp.Data.Models
{
    [Comment("Represents Cabin layout of Deck of Ship in the system.")]
    public class CabinLayout
    {
        [Key]
        [Comment("Primary key for the Cabin layout.")]
        public int Id { get; set; }

        [Required]
        [Comment("The Deck of the Cabin.")]
        public int DeckId { get; set; }

        [ForeignKey(nameof(DeckId))]
        public Deck Deck { get; set; } = null!;

        [Required]
        [Comment("The Cabin.")]
        public int CabinId { get; set; }

        [ForeignKey(nameof(CabinId))]
        public Cabin Cabin { get; set; } = null!;

        [Required]
        [Comment("X coordinate in pixels relative to the top-left corner of the deck image.")]
        public int PosX { get; set; }

        [Required]
        [Comment("Y coordinate in pixels relative to the top-left corner of the deck image.")]
        public int PosY { get; set; }
    }
}
