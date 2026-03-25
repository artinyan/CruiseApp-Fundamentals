using CruiseApp.Services.Core.Interfaces;
using CruiseApp.ViewModels;
using CruiseApp.ViewModels.Reservation;
using CruiseApp.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize(Roles = Roles.User)]
public class UserController : Controller
{
    private readonly ICruiseLikeService cruiseLikeService;
    private readonly IReservationService reservationService;

    public UserController(
        ICruiseLikeService cruiseLikeService,
        IReservationService reservationService)
    {
        this.cruiseLikeService = cruiseLikeService;
        this.reservationService = reservationService;
    }


    [HttpGet]
    public async Task<IActionResult> MyLikedCruises()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var cruises = await cruiseLikeService.GetLikedCruisesAsync(userId);

        var model = cruises.Select(c => new CruiseListItemViewModel
        {
            Id = c.Id,
            ShipName = c.Ship.Name,
            StartPoint = c.Route.Days
                    .Where(d => d.Date == c.FirstDay)
                    .Select(d => d.Point.Name)
                    .FirstOrDefault() ?? "default",
            EndPoint = c.Route.Days
                    .Where(d => d.Date == c.LastDay)
                    .Select(d => d.Point.Name)
                    .FirstOrDefault() ?? "default",
            FirstDay = c.FirstDay,
            LastDay = c.LastDay,
            Nights = c.CruiseLength,
            Destinations = string.Join(" • ",
        c.Route.Days
            .Where(d => d.Date >= c.FirstDay && d.Date <= c.LastDay)
            .OrderBy(d => d.Date)
            .Select(d => d.Point.Name))
        }).ToList();


        return View(model);
    }

    public async Task<IActionResult> MyReservations()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized();

        var serviceModels = await reservationService.GetUserReservationsAsync(userId);

        var viewModels = serviceModels.Select(r => new MyReservationViewModel
        {
            Id = r.Id,
            CruiseId = r.CruiseId,
            CabinName = r.CabinName,
            Price = r.Price,
            Status = r.Status,
            IsPaid = r.IsPaid,


            ShipName = r.ShipName,
            StartPoint = r.StartPoint,
            FirstDay = r.FirstDay,
            LastDay = r.LastDay,
            Nights = r.Nights,
            CruiseDiscription = r.CruiseDiscription,
            DeckNumber = r.DeckNumber,
            CabinType = r.CabinType,
            CabinDiscription = r.CabinDiscription,
            Destinations = r.Destinations,
        }).ToList();

        return View(viewModels);
    }
}
