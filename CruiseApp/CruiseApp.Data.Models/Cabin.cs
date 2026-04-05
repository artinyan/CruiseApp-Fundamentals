using CruiseApp.Data.Models.Enums;
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
        [Range(1, 999, ErrorMessage = "Cabin number must be between 1 and 999")]
        public int SequenceNumber { get; set; }

        [Required]
        [Comment("Defines type of the Cabin.")]
        public CabinType CabinType { get; set; }

        [Required]
        [Comment("The Deck of the Cabin.")]
        public int DeckId { get; set; }

        [ForeignKey(nameof(DeckId))]
        public Deck Deck { get; set; } = null!;

        [NotMapped]
        public string Name => $"{Deck.Name}{SequenceNumber:D3}";

        public CabinLayout? Layout { get; set; }

        [NotMapped]
        public int Capacity => CabinType switch
        {
            CabinType.Interior => 2,
            CabinType.SeaView => 2,
            CabinType.Balcony => 2,
            CabinType.Suite => 4,
            _ => 0
        };
    }
}
