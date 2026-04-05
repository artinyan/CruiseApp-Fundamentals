
using System.IO.Compression;
using System.Text.Json;
using CruiseApp.Data;
using CruiseApp.Data.Models;
using CruiseApp.Data.Models.Enums;
using CruiseApp.Services.Core.DTOs;
using CruiseApp.Services.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ShipImportService : IShipImportService
{
    private readonly ApplicationDbContext db;

    public ShipImportService(ApplicationDbContext db)
    {
        this.db = db;
    }



    public async Task ImportShipAsync(string zipPath)
    {

        var extractPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        ZipFile.ExtractToDirectory(zipPath, extractPath);

        var shipJsonPath = Path.Combine(extractPath, "ship.json");
        var routeJsonPath = Path.Combine(extractPath, "route.json");

        if (!File.Exists(shipJsonPath) || !File.Exists(routeJsonPath))
            throw new Exception("Missing ship.json or route.json");

        var shipJson = await File.ReadAllTextAsync(shipJsonPath);
        var routeJson = await File.ReadAllTextAsync(routeJsonPath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var shipModel = JsonSerializer.Deserialize<ShipImportDto>(shipJson, options);
        var routeModel = JsonSerializer.Deserialize<RouteImportDto>(routeJson, options);

        if (shipModel == null || routeModel == null)
            throw new Exception("Invalid JSON");

        ValidateImport(shipModel, routeModel);

        if (await db.Ships.AnyAsync(s => s.Name == shipModel.Name))
            throw new Exception("Ship already exists");

        var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/decks");

        foreach (var deck in shipModel.Decks)
        {
            var source = Path.Combine(extractPath, deck.Image);
            var destination = Path.Combine(imagesPath, deck.Image);

            if (!File.Exists(source))
                throw new Exception($"Missing image: {deck.Image}");

            File.Copy(source, destination, true);
        }

        var cabinsImagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/cabins");
        Directory.CreateDirectory(cabinsImagesPath);

        var cabinTypes = Enum.GetNames(typeof(CabinType));

        foreach (var type in cabinTypes)
        {
            var fileName = $"{shipModel.Name}{type}.jpg";
            var sourcePath = Path.Combine(extractPath, fileName);
            var destPath = Path.Combine(cabinsImagesPath, fileName);

            if (!File.Exists(sourcePath))
                throw new Exception($"Missing cabin image: {fileName}");

            if (!File.Exists(destPath))
            {
                File.Copy(sourcePath, destPath);
            }
        }

        using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            var ship = new Ship { Name = shipModel.Name };
            db.Ships.Add(ship);
            await db.SaveChangesAsync();

            foreach (var deckDto in shipModel.Decks)
            {
                var deck = new Deck
                {
                    ShipId = ship.Id,
                    Number = deckDto.Number,
                    DeckPlanImage = deckDto.Image
                };

                db.Decks.Add(deck);
                await db.SaveChangesAsync();

                foreach (var cabinDto in deckDto.Cabins)
                {
                    if (!Enum.TryParse<CabinType>(cabinDto.Type, out var cabinType))
                        throw new Exception($"Invalid cabin type: {cabinDto.Type}");

                    var cabin = new Cabin
                    {
                        DeckId = deck.Id,
                        SequenceNumber = cabinDto.SequenceNumber,
                        CabinType = cabinType
                    };

                    db.Cabins.Add(cabin);
                    await db.SaveChangesAsync();

                    db.CabinLayouts.Add(new CabinLayout
                    {
                        DeckId = deck.Id,
                        CabinId = cabin.Id,
                        PosX = cabinDto.PosX,
                        PosY = cabinDto.PosY
                    });
                }
            }

            await db.SaveChangesAsync();

            // Route
            var route = new Route
            {
                ShipId = ship.Id
            };

            db.Routes.Add(route);
            await db.SaveChangesAsync();

            // RouteDays
            foreach (var day in routeModel.RouteDays)
            {
                var point = await db.Points
                    .FirstOrDefaultAsync(p => p.Name == day.Point);

                if (point == null)
                    throw new Exception($"Point not found: {day.Point}");

                db.RouteDays.Add(new RouteDay
                {
                    RouteId = route.Id,
                    Date = DateOnly.Parse(day.Date),
                    PointId = point.Id
                });
            }

            await db.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            if (Directory.Exists(extractPath))
                Directory.Delete(extractPath, true);
        }
    }

    private void ValidateImport(ShipImportDto shipModel, RouteImportDto routeModel)
    {
        if (routeModel.ShipName != shipModel.Name)
            throw new Exception("Ship name mismatch");

        if (shipModel.Decks.GroupBy(d => d.Number).Any(g => g.Count() > 1))
            throw new Exception("Duplicate deck numbers");

        foreach (var deck in shipModel.Decks)
        {
            if (deck.Cabins.GroupBy(c => c.SequenceNumber).Any(g => g.Count() > 1))
                throw new Exception($"Duplicate cabins in deck {deck.Number}");
        }
    }
}
