using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CruiseApp.Data.Models
{
    [Comment("Represents CHECK-IN DATA")]
    public class Passenger
    {
        [Key]
        [Comment("Primary key for the Passenger.")]
        public int Id { get; set; }

        [Required]
        [Comment("Gender of the Passenger")]
        [MaxLength(10)]
        public string Gender { get; set; } = null!;

        [Required]
        [Comment("Date of Birth of the Passenger")]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [Comment("Nationality of the Passenger")]
        [MaxLength(50)]
        public string Nationality { get; set; } = null!;

        [Required]
        [Comment("Passport Number of the Passenger")]
        [MaxLength(20)]
        public string PassportNumber { get; set; } = null!;

        [Required]
        [Comment("Passport Exparation Date of the Passenger")]
        public DateOnly PassportExpirationDate { get; set; }

        [Required]
        [Comment("Passport Issuing Country of the Passenger")]
        [MaxLength(50)]
        public string PassportIssuingCountry { get; set; } = null!;
    }
}
