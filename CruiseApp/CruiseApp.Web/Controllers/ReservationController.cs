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

                    // 🔥 запазваме user input
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



        //[HttpGet]
        //public async Task<IActionResult> Create(int cabinId, int cruiseId)
        //{
        //    var serviceModel = await reservationService.GetCreateModelAsync(cabinId, cruiseId);

        //    if (serviceModel == null)
        //        return NotFound();

        //    var viewModel = new ReservationCreateViewModel
        //    {
        //        CruiseId = serviceModel.CruiseId,
        //        ShipName = serviceModel.ShipName,
        //        FirstDay = serviceModel.FirstDay,
        //        LastDay = serviceModel.LastDay,
        //        StartPoint = serviceModel.StartPoint,
        //        Nights = serviceModel.Nights,
        //        DeckId = serviceModel.DeckId,
        //        DeckNumber = serviceModel.DeckNumber,
        //        CabinId = serviceModel.CabinId,
        //        CabinName = serviceModel.CabinName,
        //        CabinType = serviceModel.CabinType,
        //        Capacity = serviceModel.Capacity,
        //        Price = serviceModel.Price,
        //        ImageName = serviceModel.ImageName,
        //        Description = serviceModel.Description,
        //        PassengersCount = serviceModel.PassengersCount,
        //        Passengers = new List<PassengerFormViewModel>()
        //    };

        //    return View(viewModel);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(ReservationCreateViewModel model)
        //{
        //    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        //    if (userId == null)
        //        return Unauthorized();

        //    if (!ModelState.IsValid)
        //    {
        //        // Вземаме отново данните от Service
        //        var serviceModel = await reservationService.GetCreateModelAsync(model.CabinId, model.CruiseId);

        //        // Mapping обратно към ViewModel
        //        var viewModel = new ReservationCreateViewModel
        //        {
        //            CruiseId = serviceModel.CruiseId,
        //            CabinId = serviceModel.CabinId,
        //            CabinName = serviceModel.CabinName,
        //            CabinType = serviceModel.CabinType,
        //            Capacity = serviceModel.Capacity,
        //            Price = serviceModel.Price,
        //            ShipName = serviceModel.ShipName,
        //            FirstDay = serviceModel.FirstDay,
        //            LastDay = serviceModel.LastDay,
        //            StartPoint = serviceModel.StartPoint,
        //            Nights = serviceModel.Nights,
        //            DeckId = serviceModel.DeckId,
        //            DeckNumber = serviceModel.DeckNumber,
        //            ImageName = serviceModel.ImageName,
        //            Description = serviceModel.Description,

        //            // ВАЖНО: запазваме user input-а
        //            PassengersCount = model.PassengersCount,
        //            Passengers = model.Passengers
        //        };

        //        return View(viewModel);
        //    }

        //    // Mapping към ServiceModel
        //    var reservationServiceModel = new ReservationCreateServiceModel
        //    {
        //        CruiseId = model.CruiseId,
        //        CabinId = model.CabinId,
        //        CabinType = model.CabinType,
        //        PassengersCount = model.PassengersCount,
        //        Passengers = model.Passengers
        //            .Select(p => new PassengerFormServiceModel
        //            {
        //                FirstName = p.FirstName,
        //                LastName = p.LastName
        //            })
        //            .ToList()
        //    };

        //    await reservationService.CreateReservationAsync(userId, reservationServiceModel);

        //    return RedirectToAction("MyReservations", "User");
        //}

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
