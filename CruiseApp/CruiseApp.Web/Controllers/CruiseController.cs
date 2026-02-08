using CruiseApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CruiseApp.Web.ViewModels;

namespace CruiseApp.Web.Controllers
{
    public class CruiseController : Controller
    {
        private readonly ICruiseService cruiseService;
        private readonly IShipService shipService;
        private readonly IPointService pointService;

        public CruiseController(
            ICruiseService cruiseService,
            IShipService shipService,
            IPointService pointService)
        {
            this.cruiseService = cruiseService;
            this.shipService = shipService;
            this.pointService = pointService;
        }

        // ============================
        // GET: /Cruise
        // ============================
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CruiseSearchViewModel model)
        {
            // Ships dropdown
            model.Ships = (await shipService.GetAllAsync())
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                })
                .ToList();

            // Start points dropdown (без "At Sea")
            model.StartPoints = (await pointService.GetAllAsync())
                .Where(p => !p.IsSea)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                })
                .ToList();

            // Search cruises (map от Cruise -> CruiseListItemViewModel)
            var cruises = await cruiseService.SearchCruisesAsync(
                model.ShipId,
                model.StartDate,
                model.StartPointId);

            //model.Cruises = cruises.Select(c =>
            //{
            //    var firstDayRoute = c.Route.Days.FirstOrDefault(rd => rd.Date == c.FirstDay);
            //    var lastDayRoute = c.Route.Days.FirstOrDefault(rd => rd.Date == c.LastDay);

            //    return new CruiseListItemViewModel
            //    {
            //        Id = c.Id,
            //        ShipName = c.Route.Ship.Name,
            //        RouteName = $"{c.Route.Ship.Name} Route",
            //        FirstDay = c.FirstDay,
            //        LastDay = c.LastDay,
            //        Nights = c.CruiseLength,
            //        StartPoint = firstDayRoute?.Point.Name ?? "Unknown",
            //        EndPoint = lastDayRoute?.Point.Name ?? "Unknown",
            //        IsLiked = false
            //    };
            //}).ToList();






            model.Cruises = cruises.Select(c => new CruiseListItemViewModel
            {
                Id = c.Id,
                ShipName = c.Ship.Name,
                FirstDay = c.FirstDay,
                LastDay = c.LastDay,
                Nights = c.CruiseLength,

                StartPoint = c.Route.Days
                    .Where(rd => rd.Date >= c.FirstDay && rd.Date <= c.LastDay)
                    .OrderBy(rd => rd.Date)
                    .Select(rd => rd.Point.Name)
                    .FirstOrDefault() ?? string.Empty,


                // Покажи целия маршрут като редица от имена, разделени с "•"
                Destinations = string.Join(" • ", c.Route.Days
                    .Where(rd => rd.Date >= c.FirstDay && rd.Date <= c.LastDay)
                    .OrderBy(rd => rd.Date)
                    .Select(rd => rd.Point.Name)),
                EndPoint = string.Empty 
            }).ToList();












            return View(model);
        }
    }
}
