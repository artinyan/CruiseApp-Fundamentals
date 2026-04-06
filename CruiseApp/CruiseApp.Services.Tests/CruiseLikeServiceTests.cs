using Microsoft.EntityFrameworkCore;
using CruiseApp.Data;
using CruiseApp.Data.Models;

[TestFixture]
public class CruiseLikeServiceTests
{
    private ApplicationDbContext db;
    private CruiseLikeService service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "CruiseLikeDb_" + Guid.NewGuid())
            .Options;

        db = new ApplicationDbContext(options);
        service = new CruiseLikeService(db);
    }

    [Test]
    public async Task LikeAsync_WhenNotAlreadyLiked_ShouldAddLike()
    {
        await service.LikeAsync(1, "user1");

        var count = db.CruiseLikes.Count();
        Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public async Task LikeAsync_ShouldNotAddDuplicateLike()
    {
        await service.LikeAsync(1, "user1");
        await service.LikeAsync(1, "user1");

        var count = db.CruiseLikes.Count();
        Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public async Task UnlikeAsync_WhenExists_ShouldRemoveLike()
    {
        db.CruiseLikes.Add(new CruiseLike { CruiseId = 1, UserId = "user1" });
        await db.SaveChangesAsync();

        await service.UnlikeAsync(1, "user1");

        var count = db.CruiseLikes.Count();
        Assert.That(count, Is.EqualTo(0));
    }

    [Test]
    public async Task UnlikeAsync_WhenLikeDoesNotExist_ShouldDoNothing()
    {
        await service.UnlikeAsync(1, "user1");

        var count = db.CruiseLikes.Count();
        Assert.That(count, Is.EqualTo(0));
    }

    [Test]
    public async Task IsLikedAsync_WhenLiked_ShouldReturnTrue()
    {
        db.CruiseLikes.Add(new CruiseLike { CruiseId = 1, UserId = "user1" });
        await db.SaveChangesAsync();

        var result = await service.IsLikedAsync(1, "user1");

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task IsLikedAsync_WhenNotLiked_ShouldReturnFalse()
    {
        var result = await service.IsLikedAsync(1, "user1");

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task GetLikesCountAsync_ShouldReturnCorrectCount()
    {
        db.CruiseLikes.AddRange(
            new CruiseLike { CruiseId = 1, UserId = "u1" },
            new CruiseLike { CruiseId = 1, UserId = "u2" },
            new CruiseLike { CruiseId = 2, UserId = "u3" }
        );

        await db.SaveChangesAsync();

        var result = await service.GetLikesCountAsync(1);

        Assert.That(result, Is.EqualTo(2));
    }


    [TearDown]
    public void TearDown()
    {
        db.Dispose();
    }
}