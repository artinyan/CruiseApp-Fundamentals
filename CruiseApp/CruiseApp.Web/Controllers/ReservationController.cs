using CruiseApp.Data.Models.Enums;
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

                PassengersCount = 1,
                Passengers = new List<PassengerFormViewModel>
        {
            new PassengerFormViewModel()
        }
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationCreateViewModel model)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();


            // ❗ проверка дали броят съвпада
            if (model.Passengers == null || model.Passengers.Count != model.PassengersCount)
            {
                ModelState.AddModelError("", "Passengers count does not match the provided data.");
            }

            // ❗ празни имена (extra защита)
            if (model.Passengers.Any(p => string.IsNullOrWhiteSpace(p.FirstName) ||
                                          string.IsNullOrWhiteSpace(p.LastName)))
            {
                ModelState.AddModelError("", "All passengers must have first and last name.");
            }
            // ❗ validation
            if (!ModelState.IsValid)
            {
                var serviceModel = await reservationService.GetCreateModelAsync(model.CabinId, model.CruiseId);

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

                    // запазваме user input
                    PassengersCount = model.PassengersCount,
                    Passengers = model.Passengers
                };

                return View(viewModel);
            }

            // ✅ mapping към ServiceModel
            var serviceModelToCreate = new ReservationCreateServiceModel
            {
                CruiseId = model.CruiseId,
                CabinId = model.CabinId,
                PassengersCount = model.PassengersCount,
                Passengers = model.Passengers
                    .Select(p => new PassengerFormServiceModel
                    {
                        FirstName = p.FirstName,
                        LastName = p.LastName
                    })
                    .ToList()
            };

            await reservationService.CreateReservationAsync(userId, serviceModelToCreate);

            return RedirectToAction("MyReservations", "User");
        }


        [HttpGet]
        public async Task<IActionResult> CheckIn(int id)
        {
            var reservation = await reservationService.GetReservationDetailsAsync(id);

            if (reservation.Status == ReservationStatus.Confirmed)
            {
                return RedirectToAction("MyReservations", "User");
            }

            if (reservation == null)
                return NotFound();

            var model = new CheckInViewModel
            {
                ReservationId = reservation.Id,
                Status = reservation.Status,

                Passengers = reservation.Passengers.Select((p, index) => new PassengerCheckInViewModel
                {
                    ReservationId = reservation.Id,
                    PassengerOrder = index + 1,   // 🔥 важно
                    PassengerId = 0,

                    FirstName = p.FirstName,
                    LastName = p.LastName,

                    // празни за попълване
                    Gender = string.Empty,
                    Nationality = string.Empty,
                    PassportNumber = string.Empty,
                    PassportIssuingCountry = string.Empty
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CheckIn(CheckInViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var serviceModel = model.Passengers.Select(p => new PassengerCheckInServiceModel
            {
                PassengerId = p.PassengerId,
                PassengerOrder = p.PassengerOrder,

                Gender = p.Gender,
                DateOfBirth = p.DateOfBirth,
                Nationality = p.Nationality,
                PassportNumber = p.PassportNumber,
                PassportExpirationDate = p.PassportExpirationDate,
                PassportIssuingCountry = p.PassportIssuingCountry
            }).ToList();

            await reservationService.CheckInAsync(model.ReservationId, serviceModel);

            return RedirectToAction("MyReservations", "User");
        }
    }
}
