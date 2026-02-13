using CruiseApp.Web.Common;
using CruiseApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize(Roles = Roles.User)]
public class UserController : Controller
{
    private readonly ICruiseLikeService cruiseLikeService;

    public UserController(ICruiseLikeService cruiseLikeService)
    {
        this.cruiseLikeService = cruiseLikeService;
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
}
