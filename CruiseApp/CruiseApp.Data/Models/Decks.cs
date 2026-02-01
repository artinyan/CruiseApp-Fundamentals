using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CruiseApp.Data.Models
{
    [Comment("Represents Deck of the Ship in the system.")]
    public class Deck
    {
        [Key]
        [Comment("Primary key for Deck.")]
        public int Id { get; set; }
    }
}
