using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CruiseApp.Data.Models
{
    [Comment("Represents cruise in the system.")]
    public class Cruise
    {
        private Cruise() { }

        public Cruise(Route route, DateOnly firstDay, DateOnly lastDay, string? description = null)
        {
            SetRoute(route);
            SetPeriod(firstDay, lastDay);
            SetDescription(description);
            ValidateAgainstRoute();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public DateOnly FirstDay { get; private set; }

        [Required]
        public DateOnly LastDay { get; private set; }

        [Required]
        public int RouteId { get; private set; }

        [ForeignKey(nameof(RouteId))]
        public Route Route { get; private set; } = null!;

        [NotMapped]
        public Ship Ship => Route.Ship;

        [MaxLength(1000)]
        [Comment("Optional cruise description")]
        public string? Description { get; private set; }

        public int CruiseLength => LastDay.DayNumber - FirstDay.DayNumber;

        public ICollection<CruiseCabinPrice> CabinPrices { get; set; } = new List<CruiseCabinPrice>();

        private void SetRoute(Route route)
        {
            Route = route;
            RouteId = route.Id;
        }

        private void SetDescription(string? description)
        {
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        }

        public void ChangeDescription(string? description)
        {
            SetDescription(description);
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

        public void ValidateAgainstRoute()
        {
            if (Route.Days == null || !Route.Days.Any())
                throw new InvalidOperationException("Route days are not loaded.");

            var seasonStart = Route.Days.Min(d => d.Date);
            var seasonEnd = Route.Days.Max(d => d.Date);

            var firstDayRoute = Route.Days.FirstOrDefault(rd => rd.Date == FirstDay);
            var lastDayRoute = Route.Days.FirstOrDefault(rd => rd.Date == LastDay);

            if (firstDayRoute == null || lastDayRoute == null)
            {
                throw new InvalidOperationException(
                    $"Cruise dates are outside the route schedule ({seasonStart:dd.MM.yy} - {seasonEnd:dd.MM.yy})"
                    );
            }

            if (firstDayRoute.Point.IsSea)
                throw new InvalidOperationException("Cruise cannot start at sea.");

            if (lastDayRoute.Point.IsSea)
                throw new InvalidOperationException("Cruise cannot end at sea.");
        }

        public void ChangePeriod(DateOnly firstDay, DateOnly lastDay, string? description)
        {
            SetPeriod(firstDay, lastDay);
            SetDescription(description);
            ValidateAgainstRoute();
        }

        public void ChangePeriod(DateOnly firstDay, DateOnly lastDay)
        {
            ChangePeriod(firstDay, lastDay, Description);
        }
    }
}
