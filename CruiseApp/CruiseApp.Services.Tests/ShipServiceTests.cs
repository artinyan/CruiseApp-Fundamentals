using CruiseApp.Data;
using CruiseApp.Data.Models;
using CruiseApp.Services.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CruiseApp.Services.Tests
{
    [TestFixture]
    public class ShipServiceTests
    {
        private ApplicationDbContext db;
        private ShipService shipService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ShipTestDb")
                .Options;

            db = new ApplicationDbContext(options);

            db.Ships.AddRange(new List<Ship>
            {
                new Ship { Name = "Titanic" },
                new Ship { Name = "Queen Mary" }
            });
            db.SaveChanges();

            shipService = new ShipService(db);
        }

        [TearDown]
        public void TearDown()
        {
            db.Database.EnsureDeleted();
            db.Dispose();
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllShips()
        {
            var result = await shipService.GetAllAsync();

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(s => s.Name == "Titanic"));
            Assert.IsTrue(result.Any(s => s.Name == "Queen Mary"));
        }
    }
}