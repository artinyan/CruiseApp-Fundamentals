using CruiseApp.Data.Models.Enums;
using CruiseApp.Services.Core.Interfaces;
using CruiseApp.ViewModels.Admin;
using CruiseApp.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CruiseApp.Web.Controllers
{

    [Authorize(Roles = Roles.Administrator)]

    public class AdminReservationController : Controller
    {
        private readonly IReservationService reservationService;

        public AdminReservationController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new AdminReservationEditViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(AdminReservationEditViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ReferenceNumber))
            {
                model.ErrorMessage = "Enter reference number";
                return View(model);
            }

            var serviceModel = await reservationService
                .GetByReferenceAsync(model.ReferenceNumber);

            if (serviceModel == null)
            {
                model.ErrorMessage = "Reservation not found";
                return View(model);
            }

            var reservationVM = new AdminReservationDetailsViewModel
            {
                Id = serviceModel.Id,
                CabinName = serviceModel.CabinName,
                ShipName = serviceModel.ShipName,
                Status = serviceModel.Status,
                IsPaid = serviceModel.IsPaid,
                FirstDay = serviceModel.FirstDay,
                LastDay = serviceModel.LastDay,

                Passengers = serviceModel.Passengers.Select(p => new PassengerViewModel
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Gender = p.Gender,
                    DateOfBirthString = p.DateOfBirth.HasValue ? p.DateOfBirth.Value.ToString("yyyy-MM-dd") : "",
                    Nationality = p.Nationality,
                    PassportNumber = p.PassportNumber,
                    PassportExpirationDateString = p.PassportExpirationDate.HasValue ? p.PassportExpirationDate.Value.ToString("yyyy-MM-dd") : "",
                    PassportIssuingCountry = p.PassportIssuingCountry,
                    IsCheckedIn = p.IsCheckedIn
                }).ToList()
            };

            var viewModel = new AdminReservationEditViewModel
            {
                ReferenceNumber = $"#{serviceModel.Id:D8}",
                Reservation = reservationVM
            };

            return View(viewModel);
        }



        [HttpGet]
        public IActionResult ReservationSearch()
        {
            var viewModel = new AdminReservationEditViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ReservationSearch(string reference)
        {
            var model = await reservationService.GetByReferenceAsync(reference);

            if (model == null)
            {
                ViewBag.Error = "Reservation not found";
                return View();
            }

            return View("ReservationDetails", model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, ReservationStatus status)
        {
            await reservationService.ChangeStatusAsync(id, status);
            if (status == ReservationStatus.Pending)
                throw new Exception("Setting Pending is not allowed");

            return RedirectToAction(nameof(Index));
        }
    }
}
