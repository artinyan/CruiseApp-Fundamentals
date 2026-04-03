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

            if (model.Passengers == null || model.Passengers.Count != model.PassengersCount)
                throw new Exception("Invalid passengers count");

            if (model.Passengers.Any(p =>
                string.IsNullOrWhiteSpace(p.FirstName) ||
                string.IsNullOrWhiteSpace(p.LastName)))
            {
                throw new Exception("Passenger names are required");
            }

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

            using var transaction = await db.Database.BeginTransactionAsync();

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
            await transaction.CommitAsync();
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

                    ShipName = r.Cruise.Route.Ship.Name,
                    FirstDay = r.Cruise.FirstDay,
                    LastDay = r.Cruise.LastDay,
                    Nights = r.Cruise.CruiseLength,

                    StartPoint = r.Cruise.Route.Days
                        .Where(d => d.Date == r.Cruise.FirstDay)
                        .Select(d => d.Point.Name)
                        .FirstOrDefault(),

                    DeckNumber = r.Cabin.Deck.Number,
                    CabinType = r.Cabin.CabinType,

                    CruiseDiscription = r.Cruise.Description,
                    CabinDiscription = CabinDescriptionProvider.Get(
                        r.Cabin.Deck.Ship.Name,
                        r.Cabin.CabinType),

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
                            PassengerId = p.PassengerId,
                            PassengerOrder = p.PassengerOrder,

                            FirstName = p.FirstName,
                            LastName = p.LastName,

                            IsCheckedIn = p.Passenger != null
                        })
                        .ToList()

                })
                .FirstOrDefaultAsync();
        }


        public async Task CheckInAsync(int reservationId, List<PassengerCheckInServiceModel> passengers)
        {


            using var transaction = await db.Database.BeginTransactionAsync();

            var reservation = await db.CabinReservations
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null)
                throw new Exception("Reservation not found");

            if (reservation.Status == ReservationStatus.Confirmed)
                throw new Exception("Already checked in");

            var cruise = await db.Cruises
               .Where(c => c.Id == reservation.CruiseId)
               .Select(c => new
               {
                   c.FirstDay
               })
               .FirstOrDefaultAsync();

            if (cruise == null)
            {
                throw new Exception("Cruise not found");
            }

            if (IsCheckInClosed(cruise.FirstDay))
                throw new Exception("Check-in is closed");


            var reservationPassengers = await db.ReservationPassengers
                    .Where(rp => rp.CabinReservationId == reservationId)
                    .ToListAsync();

            foreach (var p in passengers)
            {
                if (string.IsNullOrWhiteSpace(p.PassportNumber))
                    throw new Exception("Invalid passport");

                if (p.PassportExpirationDate <= DateOnly.FromDateTime(DateTime.Today))
                    throw new Exception("Passport expired");

                if (p.DateOfBirth > DateOnly.FromDateTime(DateTime.Today))
                    throw new Exception("Invalid birth date");
            }

            var passengerIds = reservationPassengers
                .Where(rp => rp.PassengerId != null)
                .Select(rp => rp.PassengerId)
                .ToList();

            var existingPassengers = await db.Passengers
                .Where(x => passengerIds.Contains(x.Id))
                .ToListAsync();

            db.Passengers.RemoveRange(existingPassengers);

            foreach (var p in passengers)
            {
                var rp = reservationPassengers
                    .FirstOrDefault(x => x.PassengerOrder == p.PassengerOrder);

                if (rp == null)
                    continue;

                var passenger = new Passenger
                {
                    Gender = p.Gender,
                    DateOfBirth = p.DateOfBirth,
                    Nationality = p.Nationality,
                    PassportNumber = p.PassportNumber,
                    PassportExpirationDate = p.PassportExpirationDate,
                    PassportIssuingCountry = p.PassportIssuingCountry
                };

                await db.Passengers.AddAsync(passenger);
                rp.Passenger = passenger;
            }

            reservation.Status = ReservationStatus.Confirmed;

            await db.SaveChangesAsync();
            await transaction.CommitAsync();
        }


        private bool IsCheckInClosed(DateOnly cruiseStart)
        {
            var cutoff = cruiseStart.AddDays(-2);
            var today = DateOnly.FromDateTime(DateTime.Today);

            return today > cutoff;
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
