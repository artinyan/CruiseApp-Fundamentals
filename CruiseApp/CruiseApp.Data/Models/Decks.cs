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
        [StringLength(50)]
        [Unicode(false)]
        [Comment("Name of the deck (sequence number as string).")]
        public string Name { get; set; } = null!;

        [Required]
        [Comment("The Ship of the Deck.")]
        public int ShipId { get; set; }

        [ForeignKey(nameof(ShipId))]
        public Ship Ship { get; set; } = null!;

        public ICollection<Cabin> Cabins { get; set; } = new List<Cabin>();
    }
}
