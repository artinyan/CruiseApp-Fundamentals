using CruiseApp.Common.Infrastructure;
using CruiseApp.Data;
using CruiseApp.Data.Models;
using CruiseApp.Data.Models.Enums;
using CruiseApp.Services.Core.Interfaces;
using CruiseApp.Services.Core.Models.Reservation;
using Microsoft.EntityFrameworkCore;

namespace CruiseApp.Services.Core.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext db;

        public ReservationService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task CreateReservationAsync(string userId, ReservationCreateServiceModel model)
        {
            var cabin = await db.Cabins.FindAsync(model.CabinId);

            if (cabin == null)
                throw new Exception("Cabin not found");

            if (model.PassengersCount > cabin.Capacity)
                throw new Exception("Too many passengers");

            bool isTaken = await db.CabinReservations
                .AnyAsync(r => r.CabinId == model.CabinId
                            && r.CruiseId == model.CruiseId
                            && r.Status != ReservationStatus.Cancelled);

            if (isTaken)
                throw new Exception("Cabin is already reserved");

            var price = await db.CruiseCabinPrices
                .Where(p => p.CruiseId == model.CruiseId && p.CabinType == cabin.CabinType)
                .Select(p => p.Price)
                .FirstOrDefaultAsync();

            var reservation = new CabinReservation
            {
                CruiseId = model.CruiseId,
                CabinId = model.CabinId,
                CabinType = cabin.CabinType,
                UserId = userId,
                PassengersCount = model.PassengersCount,
                Status = ReservationStatus.Pending,

                PricePaid = price,
                IsPaid = true
            };

            await db.CabinReservations.AddAsync(reservation);
            await db.SaveChangesAsync();

            int order = 1;

            foreach (var p in model.Passengers)
            {
                var rp = new ReservationPassenger
                {
                    CabinReservationId = reservation.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    PassengerOrder = order++
                };

                await db.ReservationPassengers.AddAsync(rp);
            }

            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<MyReservationServiceModel>> GetUserReservationsAsync(string userId)
        {
            return await db.CabinReservations
                .Where(r => r.UserId == userId)
                .Select(r => new MyReservationServiceModel
                {
                    Id = r.Id,
                    CruiseId = r.CruiseId,
                    CabinName = r.Cabin.Deck.Name + r.Cabin.SequenceNumber.ToString("D3"),
                    Price = r.PricePaid,
                    Status = r.Status,
                    IsPaid = r.IsPaid,

                    // Cruise info
                    ShipName = r.Cruise.Route.Ship.Name,
                    FirstDay = r.Cruise.FirstDay,
                    LastDay = r.Cruise.LastDay,
                    Nights = r.Cruise.CruiseLength,

                    // StartPoint
                    StartPoint = r.Cruise.Route.Days
                        .Where(d => d.Date == r.Cruise.FirstDay)
                        .Select(d => d.Point.Name)
                        .FirstOrDefault(),

                    // Deck + Cabin
                    DeckNumber = r.Cabin.Deck.Number,
                    CabinType = r.Cabin.CabinType,

                    // Descriptions
                    CruiseDiscription = r.Cruise.Description,
                    CabinDiscription = CabinDescriptionProvider.Get(
                        r.Cabin.Deck.Ship.Name,
                        r.Cabin.CabinType),

                    // Destinations
                    Destinations = string.Join(" • ",
                        r.Cruise.Route.Days
                            .Where(d => d.Date >= r.Cruise.FirstDay && d.Date <= r.Cruise.LastDay)
                            .OrderBy(d => d.Date)
                            .Select(d => d.Point.Name))
                })
                .ToListAsync();
        }

        public async Task<ReservationDetailsServiceModel?> GetReservationDetailsAsync(int reservationId)
        {
            return await db.CabinReservations
                .Where(r => r.Id == reservationId)
                .Select(r => new ReservationDetailsServiceModel
                {
                    Id = r.Id,
                    CruiseId = r.CruiseId,
                    CabinName = r.Cabin.Deck.Name + r.Cabin.SequenceNumber.ToString("D3"),
                    Price = r.PricePaid,
                    Status = r.Status,
                    IsPaid = r.IsPaid,
                    Passengers = r.ReservationPassengers
                        .Select(p => new PassengerDetailsServiceModel
                        {
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            IsCheckedIn = p.Passenger.PassportNumber != null
                        }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public Task CheckInAsync(int reservationId, List<PassengerCheckInServiceModel> passengers)
        {
            throw new NotImplementedException();
        }
        public Task<bool> IsCabinAvailableAsync(int cruiseId, int cabinId)
        {
            throw new NotImplementedException();
        }



        public async Task<ReservationCreateServiceModel> GetCreateModelAsync(int cabinId, int cruiseId)
        {
            var cabin = await db.Cabins
                .Include(c => c.Deck)
                .ThenInclude(d => d.Ship)
                .FirstOrDefaultAsync(c => c.Id == cabinId);

            if (cabin == null)
                throw new Exception("Cabin not found");

            bool isTaken = await db.CabinReservations
                .AnyAsync(r => r.CabinId == cabinId
                            && r.CruiseId == cruiseId
                            && r.Status != ReservationStatus.Cancelled);

            if (isTaken)
                throw new Exception("Cabin is already reserved");

            var price = await db.CruiseCabinPrices
                .Where(p => p.CruiseId == cruiseId && p.CabinType == cabin.CabinType)
                .Select(p => p.Price)
                .FirstOrDefaultAsync();

            var description = CabinDescriptionProvider.Get(cabin.Deck.Ship.Name, cabin.CabinType);

            var cruise = await db.Cruises
                .Include(c => c.Route)
                    .ThenInclude(r => r.Days)
                        .ThenInclude(rd => rd.Point)
                .FirstOrDefaultAsync(c => c.Id == cruiseId);

            var startPoint = cruise.Route.Days
                .FirstOrDefault(rd => rd.Date == cruise.FirstDay)?
                .Point.Name;

            return new ReservationCreateServiceModel
            {
                CruiseId = cruiseId,
                ShipName = cabin.Deck.Ship.Name,
                FirstDay = cruise.FirstDay,
                LastDay = cruise.LastDay,
                StartPoint = startPoint,
                Nights = cruise.CruiseLength,
                DeckId = cabin.DeckId,
                DeckNumber = cabin.Deck.Number,
                CabinId = cabinId,
                CabinName = cabin.Deck.Name + cabin.SequenceNumber.ToString("D3"),
                CabinType = cabin.CabinType,
                Capacity = cabin.Capacity,
                Price = price,
                ImageName = $"{cabin.Deck.Ship.Name}{cabin.CabinType}.jpg",
                Description = description,
                PassengersCount = 1,
                Passengers = new List<PassengerFormServiceModel>()
            };
        }
    }
}
