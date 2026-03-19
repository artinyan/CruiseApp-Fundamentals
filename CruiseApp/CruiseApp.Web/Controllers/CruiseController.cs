using CruiseApp.Data.Models;
using CruiseApp.Data.Models.Enums;
using CruiseApp.Services.Interfaces;
using CruiseApp.Web.Common;
using CruiseApp.Web.ViewModels;
using CruiseApp.Web.ViewModels.Cruise;
using CruiseApp.Web.ViewModels.Deck;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            IPointService pointService,
            ICruiseLikeService cruiseLikeService)
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
            // Ships dropdown
            model.Ships = (await shipService.GetAllAsync())
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                })
                .ToList();

            // Start points dropdown (without "At Sea")
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

        public async Task<IActionResult> Cabins(int cruiseId)
        {
            var serviceModel = await cruiseService.GetCabinsAsync(cruiseId);

            if (serviceModel == null)
                return NotFound();

            var model = new CabinsViewModel
            {
                CruiseId = serviceModel.CruiseId,
                ShipName = serviceModel.ShipName,
                StartPoint = serviceModel.StartPoint,
                FirstDay = serviceModel.FirstDay,
                LastDay = serviceModel.LastDay,
                Nights = serviceModel.Nights,

                Cabins = serviceModel.Cabins
                    .Select(c => new CabinCardViewModel
                    {
                        ShipName = serviceModel.ShipName,
                        CabinType = c.CabinType,
                        CruiseId = serviceModel.CruiseId,
                        Price = c.Price,

                        Decks = c.Decks.Select(d => new DeckButtonViewModel
                        {
                            Id = d.Id,
                            Name = d.Name
                        })
                    })
                    .ToList()
            };

            return View(model);
        }

      

        public async Task<IActionResult> Deck(int cruiseId, int deckId, CabinType cabinType)
        {
            var serviceModel = await cruiseService
                .GetDeckCabinsAsync(cruiseId, deckId, cabinType);

            if (serviceModel == null)
                return NotFound();

            var model = new DeckCabinsViewModel
            {
                CruiseId = serviceModel.CruiseId,
                ShipName = serviceModel.ShipName,
                DeckId = serviceModel.DeckId,
                DeckName = serviceModel.DeckName,
                DeckNumber = int.Parse(serviceModel.DeckName),
                DeckImage = serviceModel.DeckImage,
                CabinType = serviceModel.CabinType,
                Cabins = serviceModel.Cabins
                    .Select(c => new CabinButtonViewModel
                    {
                        Id = c.Id,
                        Number = c.Number,
                        Name = c.Name,
                        CabinType = c.CabinType,
                        PosX = c.PosX,
                        PosY = c.PosY,
                        IsAvailable = c.IsAvailable
                    })
                    .ToList()
            };
            return View(model);
        }


    }
}
