using CruiseApp.Data;
using CruiseApp.Data.Models;
using CruiseApp.Data.Models.Enums;
using CruiseApp.Services.Core.Models.Admin;
using CruiseApp.Services.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CruiseApp.Services.Tests
{
    [TestFixture]
    public class CruiseServiceTests
    {
        private ApplicationDbContext db;
        private CruiseService service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            db = new ApplicationDbContext(options);
            service = new CruiseService(db);
        }

        [TearDown]
        public void TearDown()
        {
            db.Dispose();
        }


        [Test]
        public async Task SearchCruisesAsync_WhenNoFilters_ReturnsAll()
        {
            var ship = new Ship { Id = 1, Name = "Test Ship" };
            db.Ships.Add(ship);

            var route = new Route { Id = 1, ShipId = ship.Id, Ship = ship, Days = new List<RouteDay>() };
            db.Routes.Add(route);

            var startDate = DateOnly.FromDateTime(DateTime.Today);
            var endDate = startDate.AddDays(5);

            for (int i = 0; i <= 5; i++)
            {
                var point = new Point { Id = i + 1, Name = $"Point{i}" };
                db.Points.Add(point);

                var routeDay = new RouteDay
                {
                    Id = i + 1,
                    RouteId = route.Id,
                    Route = route,
                    Date = startDate.AddDays(i),
                    Point = point,
                    PointId = point.Id
                };
                route.Days.Add(routeDay);
            }

            await db.SaveChangesAsync();

            var cruise = new Cruise(route, startDate, endDate);
            db.Cruises.Add(cruise);
            await db.SaveChangesAsync();

            var result = await service.SearchCruisesAsync(null, null, null);

            Assert.That(result.Count(), Is.EqualTo(1));
        }


        [Test]
        public async Task GetByIdAsync_WhenExists_ReturnsCruise()
        {
            var ship = new Ship { Id = 1, Name = "Test Ship" };
            db.Ships.Add(ship);

            var route = new Route { Id = 1, ShipId = ship.Id, Ship = ship, Days = new List<RouteDay>() };
            db.Routes.Add(route);

            var startDate = DateOnly.FromDateTime(DateTime.Today);
            var endDate = startDate.AddDays(3);

            for (int i = 0; i <= 3; i++)
            {
                var point = new Point { Id = i + 1, Name = $"Point{i}" };
                db.Points.Add(point);

                var routeDay = new RouteDay
                {
                    Id = i + 1,
                    RouteId = route.Id,
                    Route = route,
                    Date = startDate.AddDays(i),
                    Point = point,
                    PointId = point.Id
                };
                route.Days.Add(routeDay);
            }

            await db.SaveChangesAsync();

            var cruise = new Cruise(route, startDate, endDate);
            db.Cruises.Add(cruise);
            await db.SaveChangesAsync();

            var result = await service.GetByIdAsync(cruise.Id);

            Assert.IsNotNull(result);
        }


        [Test]
        public void CreateAsync_WhenRouteNotFound_ThrowsException()
        {
            var model = new AdminCruiseFormModel
            {
                ShipId = 1,
                FirstDay = DateOnly.FromDateTime(DateTime.Today),
                LastDay = DateOnly.FromDateTime(DateTime.Today.AddDays(3)),
                CabinPrices = new List<AdminCruiseCabinPriceFormModel>()
            };

            Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(model));
        }

        [Test]
        public async Task CreateAsync_WhenValid_AddsCruise()
        {
            var ship = new Ship { Id = 1, Name = "Ship" };
            db.Ships.Add(ship);

            var route = new Route { Id = 1, ShipId = ship.Id, Ship = ship, Days = new List<RouteDay>() };
            db.Routes.Add(route);

            var startDate = DateOnly.FromDateTime(DateTime.Today);
            var endDate = startDate.AddDays(3);

            for (int i = 0; i <= 3; i++)
            {
                var point = new Point { Id = i + 1, Name = $"Point{i}" };
                db.Points.Add(point);

                var routeDay = new RouteDay
                {
                    Id = i + 1,
                    RouteId = route.Id,
                    Route = route,
                    Date = startDate.AddDays(i),
                    Point = point,
                    PointId = point.Id
                };

                route.Days.Add(routeDay);
            }

            await db.SaveChangesAsync();

            var model = new AdminCruiseFormModel
            {
                ShipId = ship.Id,
                FirstDay = startDate,
                LastDay = endDate,
                CabinPrices = Enum.GetValues<CabinType>()
                    .Select(ct => new AdminCruiseCabinPriceFormModel { CabinType = ct, Price = 100 })
                    .ToList()
            };

            await service.CreateAsync(model);

            Assert.That(db.Cruises.Count(), Is.EqualTo(1));
        }


        [Test]
        public void UpdateAsync_WhenNotFound_ThrowsException()
        {
            var model = new AdminCruiseFormModel();

            Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(1, model));
        }


        [Test]
        public async Task DeleteAsync_RemovesCruise()
        {
            var ship = new Ship { Id = 1, Name = "Test Ship" };
            db.Ships.Add(ship);

            var route = new Route { Id = 1, ShipId = ship.Id, Ship = ship, Days = new List<RouteDay>() };
            db.Routes.Add(route);

            var startDate = DateOnly.FromDateTime(DateTime.Today);
            var endDate = startDate.AddDays(3);

            for (int i = 0; i <= 3; i++)
            {
                var point = new Point { Id = i + 1, Name = $"Point{i}" };
                db.Points.Add(point);

                var routeDay = new RouteDay
                {
                    Id = i + 1,
                    RouteId = route.Id,
                    Route = route,
                    Date = startDate.AddDays(i),
                    Point = point,
                    PointId = point.Id
                };
                route.Days.Add(routeDay);
            }

            await db.SaveChangesAsync();

            var cruise = new Cruise(route, startDate, endDate);
            db.Cruises.Add(cruise);
            await db.SaveChangesAsync();

            await service.DeleteAsync(cruise.Id);

            Assert.That(db.Cruises.Count(), Is.EqualTo(0));
        }


        [Test]
        public async Task EnsureUniqueCruiseAsync_WhenDuplicate_ThrowsException()
        {
            var ship = new Ship { Id = 1, Name = "Test Ship" };
            db.Ships.Add(ship);

            var route = new Route { Id = 1, ShipId = ship.Id, Ship = ship, Days = new List<RouteDay>() };
            db.Routes.Add(route);

            var startDate = DateOnly.FromDateTime(DateTime.Today);
            var endDate = startDate.AddDays(3);

            for (int i = 0; i <= 3; i++)
            {
                var point = new Point { Id = i + 1, Name = $"Point{i}" };
                db.Points.Add(point);

                var routeDay = new RouteDay
                {
                    Id = i + 1,
                    RouteId = route.Id,
                    Route = route,
                    Date = startDate.AddDays(i),
                    Point = point,
                    PointId = point.Id
                };

                route.Days.Add(routeDay);
            }

            await db.SaveChangesAsync();

            var cruise = new Cruise(route, startDate, endDate);
            db.Cruises.Add(cruise);
            await db.SaveChangesAsync();

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await service.EnsureUniqueCruiseAsync(ship.Id, startDate, endDate));
        }
    }
}