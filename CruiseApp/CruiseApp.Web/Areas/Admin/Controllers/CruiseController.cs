using CruiseApp.Data.Models.Enums;
using CruiseApp.Web.Common;
using CruiseApp.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CruiseApp.Services.Core.Interfaces;
using CruiseApp.Services.Core.Models.Admin;

namespace CruiseApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Administrator)]
    public class CruiseController : Controller
    {
        private readonly ICruiseService cruiseService;
        private readonly IShipService shipService;

        public CruiseController(
            ICruiseService cruiseService,
            IShipService shipService)
        {
            this.cruiseService = cruiseService;
            this.shipService = shipService;
        }

        // ============================
        // LIST
        // ============================
        int cabinTypesCount = Enum.GetValues<CabinType>().Length;

        public async Task<IActionResult> Index()
        {
            var serviceModel = await cruiseService.GetAllAdminAsync();

            var viewModel = serviceModel.Select(c => new AdminCruiseListViewModel
            {
                Id = c.Id,
                ShipName = c.ShipName,
                FirstDay = c.FirstDay,
                LastDay = c.LastDay,
                CruiseLength = c.CruiseLength
            }).ToList();

            return View(viewModel);
        }

        // ============================
        // CREATE
        // ============================
        public async Task<IActionResult> Create()
        {
            await LoadShips();
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            var model = new AdminCruiseCreateViewModel
            {
                FirstDay = today,
                LastDay = today.AddDays(1), //  +1
                CabinPrices = Enum.GetValues<CabinType>()
                    .Select(ct => new AdminCruiseCabinPriceViewModel
                    {
                        CabinType = ct
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminCruiseCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadShips();
                return View(model);
            }

            if (model.CabinPrices.Count != cabinTypesCount)
            {
                ModelState.AddModelError(string.Empty, "All 4 cabin prices are required.");
                await LoadShips();
                return View(model);
            }

            if (model.CabinPrices.Select(p => p.CabinType).Distinct().Count() != cabinTypesCount)
            {
                ModelState.AddModelError(string.Empty, "Each cabin type must have exactly one price.");
                await LoadShips();
                return View(model);
            }

            var serviceModel = new AdminCruiseFormModel
            {
                ShipId = model.ShipId,
                FirstDay = model.FirstDay,
                LastDay = model.LastDay,
                Description = model.Description,
                CabinPrices = model.CabinPrices
                    .Select(p => new AdminCruiseCabinPriceFormModel
                    {
                        CabinType = p.CabinType,
                        Price = p.Price
                    })
                    .ToList()
            };



            try
            {
                await cruiseService.CreateAsync(serviceModel);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                await LoadShips();
                return View(model);
            }
        }



        // ============================
        // EDIT
        // ============================


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var serviceModel = await cruiseService.GetForEditAsync(id);
            if (serviceModel == null) return NotFound();

            var model = new AdminCruiseEditViewModel
            {
                Id = id,
                FirstDay = serviceModel.FirstDay,
                LastDay = serviceModel.LastDay,
                Description = serviceModel.Description,
                CabinPrices = serviceModel.CabinPrices
                    .OrderBy(p => p.CabinType)
                    .Select(p => new AdminCruiseCabinPriceViewModel
                    {
                        CabinType = p.CabinType, 
                        Price = p.Price
                    })
                    .ToList()
            };

            ViewBag.ShipName = serviceModel.ShipName;
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, AdminCruiseEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ShipName = (await cruiseService.GetForEditAsync(id))?.ShipName;
                return View(model);
            }

            if (model.CabinPrices.Count != cabinTypesCount)
            {
                ModelState.AddModelError(string.Empty, "All cabin prices are required.");
                ViewBag.ShipName = (await cruiseService.GetForEditAsync(id))?.ShipName;
                return View(model);
            }

            if (model.CabinPrices.Select(p => p.CabinType).Distinct().Count() != cabinTypesCount)
            {
                ModelState.AddModelError(string.Empty, "Each cabin type must have exactly one price.");
                ViewBag.ShipName = (await cruiseService.GetForEditAsync(id))?.ShipName;
                return View(model);
            }

            var serviceModel = new AdminCruiseFormModel
            {
                FirstDay = model.FirstDay,
                LastDay = model.LastDay,
                Description = model.Description,
                CabinPrices = model.CabinPrices
                    .Select(p => new AdminCruiseCabinPriceFormModel
                    {
                        CabinType = p.CabinType,
                        Price = p.Price
                    })
                    .ToList()
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
            };

            return View(viewModel);
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
