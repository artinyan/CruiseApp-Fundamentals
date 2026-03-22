using CruiseApp.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CruiseApp.Data.Models
{
    [Comment("Represents CabinReservation")]
    public class CabinReservation
    {
        [Key]
        [Comment("Primary key for the CabinReservation.")]
        public int Id { get; set; }

        [Required]
        [Comment("CruiseId")]
        public int CruiseId { get; set; }

        [ForeignKey(nameof(CruiseId))]
        public Cruise Cruise { get; set; } = null!;

        [Required]
        [Comment("CabinId")]
        public int CabinId { get; set; }

        [ForeignKey(nameof(CabinId))]
        public Cabin Cabin { get; set; } = null!;

        [Required]
        [Comment("UserId")]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;

        [Required]
        [Comment("ReservationStatus")]
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        [Required]
        [Comment("CreatedOn")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Required]
        [Comment("Cabin type at the time of reservation (snapshot)")]
        public CabinType CabinType { get; set; }

        [Required]
        [Comment("Numbers of the passengers in the cabin")]
        public int PassengersCount { get; set; }

        [Comment("The amount paid for this reservation")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePaid { get; set; } = 0m;

        [Comment("Indicates whether the reservation has been paid")]
        public bool IsPaid { get; set; } = false;

        public ICollection<ReservationPassenger> ReservationPassengers { get; set; } 
            = new List<ReservationPassenger>();

        /// <summary>
        /// Simulated payment for the reservation (pseudo-payment for course project)
        /// </summary>
        public void Pay(decimal amount)
        {
            PricePaid = amount;
            IsPaid = true;
        }

         //PassengersCount <= CabinTypeConstants.GetCapacity(cabin.Type
         //Reservation.Passengers.Count == PassengersCount
    }
}
