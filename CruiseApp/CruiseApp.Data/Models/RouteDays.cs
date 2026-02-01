using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CruiseApp.Data.Models
{
    [Comment("Represents RouteDay in the system")]
    public class RouteDay
    {
        [Key]
        [Comment("Primary key for RouteDay.")]
        public int Id { get; set; }
    }
}