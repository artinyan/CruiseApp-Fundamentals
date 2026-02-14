using CruiseApp.Services.Interfaces;
using CruiseApp.Services.Models.Admin;
using CruiseApp.Web.Common;
using CruiseApp.Web.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CruiseApp.Web.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class AdminCruiseController : Controller
    {
        private readonly ICruiseService cruiseService;
        private readonly IShipService shipService;

        public AdminCruiseController(
            ICruiseService cruiseService,
            IShipService shipService)
        {
            this.cruiseService = cruiseService;
            this.shipService = shipService;
        }

        // ============================
        // LIST
        // ============================


        public async Task<IActionResult> Index()
        {
            var serviceModel = await cruiseService.GetAllAdminAsync();

            // Мап към ViewModel
            var viewModel = serviceModel.Select(c => new AdminCruiseListViewModel
            {
                Id = c.Id,
                ShipName = c.ShipName,
                FirstDay = c.FirstDay,
                LastDay = c.LastDay,
                CruiseLength = c.CruiseLength
            }).ToList();

            return View(viewModel); // View получава точно това, което очаква
        }

        // ============================
        // CREATE
        // ============================
        public async Task<IActionResult> Create()
        {
            await LoadShips();
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(AdminCruiseFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadShips();
                return View(model);
            }

            var serviceModel = new AdminCruiseFormModel
            {
                ShipId = model.ShipId,
                FirstDay = model.FirstDay,
                LastDay = model.LastDay
            };

            try
            {
                await cruiseService.CreateAsync(serviceModel);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                // ❗ Domain validation → UI validation
                ModelState.AddModelError(string.Empty, ex.Message);

                await LoadShips();
                return View(model);
            }
        }



        // ============================
        // EDIT
        // ============================


        // GET
        public async Task<IActionResult> Edit(int id)
        {
            var serviceModel = await cruiseService.GetForEditAsync(id);
            if (serviceModel == null) return NotFound();

            var model = new AdminCruiseFormViewModel
            {
                Id = id,
                FirstDay = serviceModel.FirstDay,
                LastDay = serviceModel.LastDay
            };

            ViewBag.ShipName = serviceModel.ShipName; // вече ще има стойност
            return View(model);
        }


        // POST
        [HttpPost]
        public async Task<IActionResult> Edit(int id, AdminCruiseFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ShipName = (await cruiseService.GetForEditAsync(id))?.ShipName;
                return View(model);
            }

            var serviceModel = new AdminCruiseFormModel
            {
                FirstDay = model.FirstDay,
                LastDay = model.LastDay
            };

            try
            {
                await cruiseService.UpdateAsync(id, serviceModel);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.ShipName = (await cruiseService.GetForEditAsync(id))?.ShipName;
                return View(model);
            }
        }


        // ============================
        // DELETE
        // ============================

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var serviceModel = await cruiseService.GetForDeleteAsync(id);
            if (serviceModel == null) return NotFound();

            // Map към ViewModel
            var viewModel = new AdminCruiseListViewModel
            {
                Id = serviceModel.Id,
                ShipName = serviceModel.ShipName,
                FirstDay = serviceModel.FirstDay,
                LastDay = serviceModel.LastDay,
                CruiseLength = serviceModel.CruiseLength
                //CruiseLength = serviceModel.LastDay.DayNumber - serviceModel.FirstDay.DayNumber
            };

            return View(viewModel); // ✅ View получава точно това, което очаква
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await cruiseService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // ============================
        // HELPERS
        // ============================
        private async Task LoadShips()
        {
            ViewBag.Ships = (await shipService.GetAllAsync())
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString()
                });
        }
    }
}
