using CruiseApp.Data.Models.Enums;
using CruiseApp.Services.Core.Interfaces;
using CruiseApp.ViewModels.Cruise;
using CruiseApp.ViewModels.Deck;
using Microsoft.AspNetCore.Mvc;

namespace CruiseApp.Web.Controllers
{
    public class CabinController : Controller
    {
        private readonly ICruiseService cruiseService;

        public CabinController(ICruiseService cruiseService)
        {
            this.cruiseService = cruiseService;
        }

        // ============================
        // GET: /Cabin/Cabins
        // ============================
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

        // ============================
        // GET: /Cabin/Deck
        // ============================
        public async Task<IActionResult> Deck(int cruiseId, int deckId, CabinType cabinType)
        {
            var serviceModel = await cruiseService.GetDeckCabinsAsync(cruiseId, deckId, cabinType);

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