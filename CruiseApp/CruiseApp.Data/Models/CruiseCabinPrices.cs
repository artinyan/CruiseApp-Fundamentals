using CruiseApp.Data.Models;
using CruiseApp.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CruiseCabinPrice
{
    [Key]
    [Comment("Primary key for the CabinCruisePrice.")]
    public int Id { get; set; }

    [Required]
    [Comment("Cruise for which this price applies")]
    public int CruiseId { get; set; }

    //[NotMapped]
    //public Cruise? Cruise { get; set; }

    [ForeignKey(nameof(CruiseId))]
    public Cruise Cruise { get; set; } = null!;

    [Required]
    [Comment("Type of the Cabin")]
    public CabinType CabinType { get; set; }

    [Required]
    [Comment("Price for the cabin type on this cruise")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
}