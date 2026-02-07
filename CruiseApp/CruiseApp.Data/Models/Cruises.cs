using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CruiseApp.Data.Models
{
    [Comment("Repersents cruise in the system.")]
    public class Cruise
    {
        private Cruise() { }

        public Cruise(Route route, DateOnly firstDay, DateOnly lastDay)
        {
            SetRoute(route);
            SetPeriod(firstDay, lastDay);
            ValidateAgainstRoute();
        }

        [Key]
        [Comment("Primary key for Cruise.")]
        public int Id { get; set; }

        [Required]
        [Comment("Embarkation day of the cruise")]
        public DateOnly FirstDay { get; private set; }

        [Required]
        [Comment("Disembarkation day of the cruise")]
        public DateOnly LastDay { get; private set; }

        [Required]
        [Comment("The route of the cruise")]
        public int RouteId { get; private set; }

        [ForeignKey(nameof(RouteId))]
        public Route Route { get; private set; } = null!;

        [NotMapped]
        public Ship Ship => Route.Ship;

        public int CruiseLength => LastDay.DayNumber - FirstDay.DayNumber;

        private void SetRoute(Route route)
        {
            Route = route;
            RouteId = route.Id;
        }

        public void SetPeriod(DateOnly firstDay, DateOnly lastDay)
        {
            if (firstDay >= lastDay)
                throw new InvalidOperationException("Last day must be after first day.");

            if (lastDay.DayNumber - firstDay.DayNumber > 14)
                throw new InvalidOperationException("Cruise cannot exceed 14 days.");

            FirstDay = firstDay;
            LastDay = lastDay;
        }

        private void ValidateAgainstRoute()
        {
            if (Route.Days == null || !Route.Days.Any())
                throw new InvalidOperationException("Route days are not loaded.");

            var firstDayRoute = Route.Days.FirstOrDefault(rd => rd.Date == FirstDay);
            var lastDayRoute = Route.Days.FirstOrDefault(rd => rd.Date == LastDay);

            if (firstDayRoute == null || lastDayRoute == null)
                throw new InvalidOperationException("Cruise dates are outside the route schedule.");

            if (firstDayRoute.Point.IsSea)
                throw new InvalidOperationException("Cruise cannot start at sea.");

            if (lastDayRoute.Point.IsSea)
                throw new InvalidOperationException("Cruise cannot end at sea.");
        }
    }
}
