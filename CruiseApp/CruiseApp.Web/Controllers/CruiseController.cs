using CruiseApp.Services.Core.Interfaces;
using CruiseApp.ViewModels;
using CruiseApp.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CruiseApp.Web.Controllers
{
    public class CruiseController : Controller
    {
        private readonly ICruiseService cruiseService;
        private readonly IShipService shipService;
        private readonly IPointService pointService;
        private readonly ICruiseLikeService cruiseLikeService;

        public CruiseController(
            ICruiseService cruiseService,
            IShipService shipService,
            ICruiseLikeService cruiseLikeService,
            IPointService pointService
            )
        {
            this.cruiseService = cruiseService;
            this.shipService = shipService;
            this.cruiseService = cruiseService;
            this.cruiseLikeService = cruiseLikeService;
            this.pointService = pointService;
        }

        // ============================
        // GET: /Cruise
        // ============================
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CruiseSearchViewModel model)
        {
            model.Ships = (await shipService.GetAllAsync())
                .Select(s => new SelectOptionViewModel
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                })
                .ToList();

            model.StartPoints = (await pointService.GetAllAsync())
                .Where(p => !p.IsSea)
                .Select(p => new SelectOptionViewModel
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                })
                .ToList();

            var cruises = await cruiseService.SearchCruisesAsync(
                model.ShipId,
                model.StartDate,
                model.StartPointId);

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

                Destinations = string.Join(" • ", c.Route.Days
                    .Where(rd => rd.Date >= c.FirstDay && rd.Date <= c.LastDay)
                    .OrderBy(rd => rd.Date)
                    .Select(rd => rd.Point.Name)),

                EndPoint = string.Empty
            }).ToList();

            return View(model);
        }

        //// ============================
        //// Cruise
        //// ============================

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var cruise = await cruiseService.GetByIdAsync(id);

            if (cruise == null)
            {
                return NotFound();
            }

            var model = new CruiseDetailsViewModel
            {
                Id = cruise.Id,
                ShipName = cruise.Ship.Name,
                FirstDay = cruise.FirstDay,
                LastDay = cruise.LastDay,
                Nights = cruise.CruiseLength,

                Description = cruise.Description,

                StartPoint = cruise.Route.Days
                    .Where(rd => rd.Date >= cruise.FirstDay && rd.Date <= cruise.LastDay)
                    .OrderBy(rd => rd.Date)
                    .Select(rd => rd.Point.Name)
                    .FirstOrDefault() ?? string.Empty,

                Destinations = string.Join(" → ", cruise.Route.Days
                    .Where(rd => rd.Date >= cruise.FirstDay && rd.Date <= cruise.LastDay)
                    .OrderBy(rd => rd.Date)
                    .Select(rd => rd.Point.Name)),
                EndPoint = string.Empty
            };

            if (User.Identity?.IsAuthenticated == true && User.IsInRole(Roles.User))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId != null)
                {
                    model.IsLiked = await cruiseLikeService.IsLikedAsync(cruise.Id, userId);
                }
            }

            model.LikesCount = await cruiseLikeService.GetLikesCountAsync(cruise.Id);

            return View(model);
        }

        [Authorize(Roles = Roles.User)]
        [HttpPost]
        public async Task<IActionResult> Like(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await cruiseLikeService.LikeAsync(id, userId);
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = Roles.User)]
        [HttpPost]
        public async Task<IActionResult> Unlike(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await cruiseLikeService.UnlikeAsync(id, userId);
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
