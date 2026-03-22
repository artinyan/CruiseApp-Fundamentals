using CruiseApp.Services.Core.Interfaces;
using CruiseApp.Services.Core.Models.Reservation;
using CruiseApp.ViewModels.Reservation;
using Microsoft.AspNetCore.Mvc;

namespace CruiseApp.Web.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService reservationService;

        public ReservationController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await reservationService.GetReservationDetailsAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int cabinId, int cruiseId)
        {
            var serviceModel = await reservationService.GetCreateModelAsync(cabinId, cruiseId);

            if (serviceModel == null)
                return NotFound();

            var viewModel = new ReservationCreateViewModel
            {
                CruiseId = serviceModel.CruiseId,

                ShipName = serviceModel.ShipName,
                FirstDay = serviceModel.FirstDay,
                LastDay = serviceModel.LastDay,
                StartPoint = serviceModel.StartPoint,
                Nights = serviceModel.Nights,
                DeckId = serviceModel.DeckId,
                DeckNumber = serviceModel.DeckNumber,
                CabinId = serviceModel.CabinId,
                CabinName = serviceModel.CabinName,
                CabinType = serviceModel.CabinType,
                Capacity = serviceModel.Capacity,
                Price = serviceModel.Price,
                ImageName = serviceModel.ImageName,
                Description = serviceModel.Description,
                PassengersCount = serviceModel.PassengersCount,
                Passengers = new List<PassengerFormViewModel>()
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Create(ReservationCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var serviceModel = new ReservationCreateServiceModel
            {
                CruiseId = model.CruiseId,
                CabinId = model.CabinId,
                PassengersCount = model.PassengersCount,
                Passengers = model.Passengers
                    .Select(p => new PassengerFormServiceModel
                    {
                        FirstName = p.FirstName,
                        LastName = p.LastName
                    }).ToList()
            };
            await reservationService.CreateReservationAsync(userId!, serviceModel);
            return RedirectToAction("MyReservations", "User");
        }


        [HttpGet]
        public async Task<IActionResult> CheckIn(int id)
        {
            var model = await reservationService.GetReservationDetailsAsync(id);

            if (model == null)
                return NotFound();

            // mapping към ViewModel (тук ще го направим после)
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CheckIn(PassengerCheckInViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // mapping към ServiceModel
            // await reservationService.CheckInAsync(...)

            return RedirectToAction("MyReservations", "User");
        }
    }
}
