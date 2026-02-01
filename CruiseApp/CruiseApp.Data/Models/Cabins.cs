using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CruiseApp.Data.Models
{
    [Comment("Represents Cabin of Deck of Ship in the system.")]
    public class Cabin
    {
        [Key]
        [Comment("Primary key for the Cabin.")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        [Comment("Name of Cabin - combination of name of the Deck and sequence number.")]
        public string Name { get; set; } = null!;

        [Required]
        [Comment("The Deck of the Cabin.")]
        public int DeckId { get; set; }

        [ForeignKey(nameof(DeckId))]
        public Deck Deck { get; set; } = null!;

    }
}
