using CruiseApp.Data;
using CruiseApp.Data.Models;
using CruiseApp.Data.Models.Enums;
using CruiseApp.Services.Core.Interfaces;
using CruiseApp.Services.Core.Models.Admin;
using CruiseApp.Services.Core.Models.Cruise;
using Microsoft.EntityFrameworkCore;

namespace CruiseApp.Services.Core.Services
{
    public class CruiseService : ICruiseService
    {
        private readonly ApplicationDbContext db;

        public CruiseService(ApplicationDbContext db)
        {
            this.db = db;
        }

        // ============================
        // PUBLIC
        // ============================

        public async Task<IEnumerable<Cruise>> SearchCruisesAsync(
            int? shipId, DateOnly? startDate, int? startPointId)
        {
            var query = db.Cruises
                .AsNoTracking()
                .Include(c => c.Route)
                    .ThenInclude(r => r.Ship)
                .Include(c => c.Route)
                    .ThenInclude(r => r.Days)
                        .ThenInclude(d => d.Point)
                .AsQueryable();

            if (shipId.HasValue)
                query = query.Where(c => c.Route.ShipId == shipId.Value);

            if (startDate.HasValue)
                query = query.Where(c => c.FirstDay == startDate.Value);

            if (startPointId.HasValue)
            {
                query = query.Where(c =>
                    c.Route.Days.Any(d =>
                        d.Date == c.FirstDay &&
                        d.PointId == startPointId.Value));
            }

            return await query
                .OrderBy(c => c.FirstDay)
                .ToListAsync();
        }

        public async Task<Cruise?> GetByIdAsync(int id)
        {
            return await db.Cruises
                .Include(c => c.Route)
                    .ThenInclude(r => r.Ship)
                .Include(c => c.Route)
                    .ThenInclude(r => r.Days)
                        .ThenInclude(d => d.Point)
                .FirstOrDefaultAsync(c => c.Id == id);
        }


        public async Task<CabinsServiceModel?> GetCabinsAsync(int cruiseId)
        {
            var cruise = await db.Cruises
                .AsNoTracking()
                .Where(c => c.Id == cruiseId)
                .Select(c => new
                {
                    c.Id,
                    ShipName = c.Route.Ship.Name,

                    StartPoint = c.Route.Days
                        .Where(d => d.Date == c.FirstDay)
                        .Select(d => d.Point.Name)
                        .FirstOrDefault(),

                    c.FirstDay,
                    c.LastDay,

                    Nights = c.LastDay.DayNumber - c.FirstDay.DayNumber,

                    CabinPrices = c.CabinPrices
                        .Select(p => new
                        {
                            p.CabinType,
                            p.Price
                        })
                        .ToList(),

                    ShipId = c.Route.Ship.Id
                })
                .FirstOrDefaultAsync();

            var decksByType = await db.Decks
                .AsNoTracking()
                .Where(d => d.ShipId == cruise.ShipId)
                .Select(d => new
                {
                    d.Id,
                    d.Number,

                    CabinTypes = d.Cabins
                        .Select(c => c.CabinType)
                        .Distinct()
                        .ToList()
                })
                .ToListAsync();

            var result = new CabinsServiceModel
            {
                CruiseId = cruise.Id,
                ShipName = cruise.ShipName,
                StartPoint = cruise.StartPoint,
                FirstDay = cruise.FirstDay,
                LastDay = cruise.LastDay,
                Nights = cruise.Nights,

                Cabins = cruise.CabinPrices
                    .OrderBy(p => p.CabinType)
                    .Select(p => new CabinCardServiceModel
                    {
                        CabinType = p.CabinType,
                        Price = p.Price,

                        Decks = decksByType
                            .Where(d => d.CabinTypes.Contains(p.CabinType))
                            .OrderBy(d => d.Number)
                            .Select(d => new DeckButtonServiceModel
                            {
                                Id = d.Id,
                                Name = $"Deck {d.Number}"
                            })
                            .ToList()
                    })
                    .ToList()
            };

            return result;
        }

        public async Task<DeckCabinsServiceModel> GetDeckCabinsAsync(
    int cruiseId, int deckId, CabinType cabinType)
        {
            var deck = await db.Decks
                .Where(d => d.Id == deckId)
                .Select(d => new DeckCabinsServiceModel
                {
                    DeckId = d.Id,
                    DeckName = d.Name,
                    DeckNumber = d.Number,
                    CabinType = cabinType,
                    CruiseId = cruiseId,
                    DeckImage = $"{d.Ship.Name}-deck-{d.Number}.png",

                    Cabins = d.CabinLayouts
                        .Where(cl => cl.Cabin.CabinType == cabinType)
                        .OrderBy(cl => cl.Cabin.SequenceNumber)
                        .Select(cl => new CabinButtonServiceModel
                        {
                            Id = cl.CabinId,
                            Number = cl.Cabin.SequenceNumber.ToString(),
                            Name = cl.Cabin.Name,
                            CabinType = cl.Cabin.CabinType,

                            PosX = cl.PosX,
                            PosY = cl.PosY,

                            IsAvailable = !db.CabinReservations
                                .Any(r => r.CruiseId == cruiseId
                                       && r.CabinId == cl.CabinId
                                       && r.Status != ReservationStatus.Canceled)
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return deck!;
        }


        // ============================
        // ADMIN
        // ============================

        public async Task<IEnumerable<AdminCruiseListModel>> GetAllAdminAsync()
        {
            return await db.Cruises
                .Include(c => c.Route)
                    .ThenInclude(r => r.Ship)
                .OrderByDescending(c => c.FirstDay)
                .Select(c => new AdminCruiseListModel
                {
                    Id = c.Id,
                    ShipName = c.Route.Ship.Name,
                    FirstDay = c.FirstDay,
                    LastDay = c.LastDay,
                    CruiseLength = c.CruiseLength
                })
                .ToListAsync();
        }


        public async Task CreateAsync(AdminCruiseFormModel model)
        {
            var route = await db.Routes
                .Include(r => r.Days)
                .ThenInclude(d => d.Point)
                .FirstOrDefaultAsync(r => r.ShipId == model.ShipId);

            if (route == null)
                throw new InvalidOperationException("Route not found for ship.");

            bool exists = await db.Cruises.AnyAsync(c =>
                c.Route.ShipId == model.ShipId &&
                c.FirstDay == model.FirstDay &&
                c.LastDay == model.LastDay);

            if (exists)
                throw new InvalidOperationException("A cruise with the same ship and dates already exists.");

            if (model.CabinPrices.Count != 4)
            {
                throw new InvalidOperationException("All 4 cabin prices are required.");
            }

            if (model.CabinPrices.Select(p => p.CabinType).Distinct().Count() != 4)
            {
                throw new InvalidOperationException("Each cabin type must have exactly one price.");
            }

            var cruise = new Cruise(route, model.FirstDay, model.LastDay);

            cruise.ChangeDescription(model.Description);

            db.Cruises.Add(cruise);
            await db.SaveChangesAsync();

            var prices = model.CabinPrices.Select(p => new CruiseCabinPrice
            {
                CruiseId = cruise.Id,
                CabinType = p.CabinType,
                Price = p.Price,
            });

            db.CruiseCabinPrices.AddRange(prices);
            await db.SaveChangesAsync();
        }


        public async Task<AdminCruiseFormModel?> GetForEditAsync(int id)
        {
            return await db.Cruises
                .Include(c => c.Route)
                    .ThenInclude(r => r.Ship)
                .Include(c => c.CabinPrices)
                .Where(c => c.Id == id)
                .Select(c => new AdminCruiseFormModel
                {
                    ShipId = c.Route.ShipId,
                    ShipName = c.Route.Ship.Name,
                    FirstDay = c.FirstDay,
                    LastDay = c.LastDay,
                    Description = c.Description, // ✅ Връщаме Description
                    CabinPrices = c.CabinPrices
                        .Select(p => new AdminCruiseCabinPriceFormModel
                        {
                            CabinType = p.CabinType,
                            Price = p.Price
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(int id, AdminCruiseFormModel model)
        {
            var cruise = await db.Cruises
                .Include(c => c.Route)
                    .ThenInclude(r => r.Ship)
                .Include(c => c.Route)
                    .ThenInclude(r => r.Days)
                        .ThenInclude(d => d.Point)
                .Include(c => c.CabinPrices) // <- задължително за update на цените
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cruise == null)
                throw new InvalidOperationException("Cruise not found.");

            bool exists = await db.Cruises.AnyAsync(c =>
                c.Id != id &&
                c.Route.ShipId == cruise.Route.ShipId &&
                c.FirstDay == model.FirstDay &&
                c.LastDay == model.LastDay);

            if (exists)
                throw new InvalidOperationException("A cruise with the same ship and dates already exists.");

            cruise.ChangePeriod(model.FirstDay, model.LastDay);
            cruise.ValidateAgainstRoute();

            cruise.ChangeDescription(model.Description);

            foreach (var price in cruise.CabinPrices)
            {
                var newPrice = model.CabinPrices
                    .First(p => p.CabinType == price.CabinType);

                price.Price = newPrice.Price;
            }

            await db.SaveChangesAsync();
        }

        public async Task<AdminCruiseListModel?> GetForDeleteAsync(int id)
        {
            return await db.Cruises
                .Include(c => c.Route)
                    .ThenInclude(r => r.Ship)
                .Where(c => c.Id == id)
                .Select(c => new AdminCruiseListModel
                {
                    Id = c.Id,
                    ShipName = c.Route.Ship.Name,
                    FirstDay = c.FirstDay,
                    LastDay = c.LastDay,
                    CruiseLength = c.CruiseLength
                })
                .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cruise = await db.Cruises.FindAsync(id);

            if (cruise != null)
            {
                db.Cruises.Remove(cruise);
                await db.SaveChangesAsync();
            }
        }


        public async Task EnsureUniqueCruiseAsync(int shipId, DateOnly firstDay, DateOnly lastDay, int? ignoreCruiseId = null)
        {
            var exists = await db.Cruises
                .Include(c => c.Route)
                .AnyAsync(c =>
                    c.Route.ShipId == shipId &&
                    c.FirstDay == firstDay &&
                    c.LastDay == lastDay &&
                    (!ignoreCruiseId.HasValue || c.Id != ignoreCruiseId.Value));

            if (exists)
            {
                throw new InvalidOperationException("A cruise with the same ship and dates already exists.");
            }
        }
    }
}
