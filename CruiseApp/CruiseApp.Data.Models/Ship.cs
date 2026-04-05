using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CruiseApp.Data.Models
{
    [Comment("Represent ship in the system.")]
    public class Ship
    {
        [Key]
        [Comment("Primary key for the Ship.")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Unicode(false)]
        [Comment("Name of the Ship.")]
        public string Name { get; set; } = null!;

        public ICollection<Deck> Decks { get; set; } = new List<Deck>();
    }
}