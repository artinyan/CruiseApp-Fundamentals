using CruiseApp.Data;
using CruiseApp.Data.Models;
using CruiseApp.Services.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CruiseApp.Services.Tests
{
    [TestFixture]
    public class PointServiceTests
    {
        private ApplicationDbContext db;
        private PointService service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            db = new ApplicationDbContext(options);
            service = new PointService(db);
        }

        [TearDown]
        public void TearDown()
        {
            db.Dispose();
        }

        [Test]
        public async Task GetAllAsync_WhenNoPoints_ReturnsEmpty()
        {
            var result = await service.GetAllAsync();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsPointsInOrder()
        {
            db.Points.AddRange(
                new Point { Id = 1, Name = "Zeta" },
                new Point { Id = 2, Name = "Alpha" },
                new Point { Id = 3, Name = "Gamma" }
            );
            await db.SaveChangesAsync();

            var result = (await service.GetAllAsync()).ToList();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Alpha", result[0].Name);
            Assert.AreEqual("Gamma", result[1].Name);
            Assert.AreEqual("Zeta", result[2].Name);
        }
    }
}