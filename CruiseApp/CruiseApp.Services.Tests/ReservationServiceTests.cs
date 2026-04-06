using CruiseApp.Data;
using CruiseApp.Data.Models;
using CruiseApp.Data.Models.Enums;
using CruiseApp.Services.Core.Models.Reservation;
using CruiseApp.Services.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CruiseApp.Services.Tests
{
    [TestFixture]
    public class ReservationServiceTests
    {
        private ApplicationDbContext db;
        private ReservationService service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            db = new ApplicationDbContext(options);
            service = new ReservationService(db);
        }

        [TearDown]
        public void TearDown()
        {
            db.Dispose();
        }

        [Test]
        public void CreateReservationAsync_WhenPassengerCountInvalid_Throws()
        {
            var model = new ReservationCreateServiceModel
            {
                CabinId = 1,
                CruiseId = 1,
                PassengersCount = 2,
                Passengers = new List<PassengerFormServiceModel>
                {
                    new PassengerFormServiceModel { FirstName = "A", LastName = "B" }
                }
            };

            Assert.ThrowsAsync<Exception>(() =>
                service.CreateReservationAsync("user", model));
        }

        [Test]
        public void CreateReservationAsync_WhenPassengerNameMissing_Throws()
        {
            var model = new ReservationCreateServiceModel
            {
                CabinId = 1,
                CruiseId = 1,
                PassengersCount = 1,
                Passengers = new List<PassengerFormServiceModel>
                {
                    new PassengerFormServiceModel { FirstName = "", LastName = "B" }
                }
            };

            Assert.ThrowsAsync<Exception>(() =>
                service.CreateReservationAsync("user", model));
        }

        [Test]
        public void CreateReservationAsync_WhenCabinNotFound_Throws()
        {
            var model = new ReservationCreateServiceModel
            {
                CabinId = 0,
                CruiseId = 1,
                PassengersCount = 1,
                Passengers = new List<PassengerFormServiceModel>
                {
                    new PassengerFormServiceModel { FirstName = "A", LastName = "B" }
                }
            };

            Assert.ThrowsAsync<Exception>(() =>
                service.CreateReservationAsync("user", model));
        }

        [Test]
        public void ChangeStatusAsync_WhenSetConfirmed_Throws()
        {
            Assert.ThrowsAsync<Exception>(() =>
                service.ChangeStatusAsync(1, ReservationStatus.Confirmed));
        }

        [Test]
        public void ChangeStatusAsync_WhenSetPending_Throws()
        {
            Assert.ThrowsAsync<Exception>(() =>
                service.ChangeStatusAsync(1, ReservationStatus.Pending));
        }

        [Test]
        public void ChangeStatusAsync_WhenNotFound_Throws()
        {
            Assert.ThrowsAsync<Exception>(() =>
                service.ChangeStatusAsync(999, ReservationStatus.Canceled));
        }

        [Test]
        public async Task ChangeStatusAsync_WhenValid_Works()
        {
            var reservation = new CabinReservation
            {
                Id = 1,
                UserId = "test-user",
                Status = ReservationStatus.Pending
            };

            db.CabinReservations.Add(reservation);
            await db.SaveChangesAsync();

            await service.ChangeStatusAsync(1, ReservationStatus.Canceled);

            Assert.That(reservation.Status, Is.EqualTo(ReservationStatus.Canceled));
        }
    }
}