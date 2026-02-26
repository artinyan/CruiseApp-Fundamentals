using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CruiseApp.Data.Models
{
    [Comment("Represents ReservationPassenger")]
    public class ReservationPassenger
    {
        [Key]
        [Comment("Primary key for the ReservationPassenger.")]
        public int Id { get; set; }

        [Required]
        [Comment("CabinReservationId")]
        public int CabinReservationId { get; set; }

        [ForeignKey(nameof(CabinReservationId))]
        public CabinReservation CabinReservation { get; set; } = null!;

        [Required]
        [Comment("Id of the Passenger")]
        public int PassengerId { get; set; }

        [ForeignKey(nameof(PassengerId))]
        public Passenger Passenger { get; set; } = null!;

        [Required]
        [Comment("First Name of the Passenger")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Comment("Last Name of the Passenger")]
        public string LastName { get; set; } = null!;

        [Required]
        [Comment("Order of the assenger in the cabin")]
        public int PassengerOrder { get; set; }
    }
}
