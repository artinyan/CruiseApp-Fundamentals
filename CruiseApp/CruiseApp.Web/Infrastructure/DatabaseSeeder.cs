using CruiseApp.Data;
using CruiseApp.Data.Models;
using CruiseApp.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Point = CruiseApp.Data.Models.Point;

namespace CruiseApp.Web.Infrastructure;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        var shipsList = new List<string> { "Aurora", "Ariel", "Neptune" };

        var pointsList = new List<string>
        {
            "At Sea","Barcelona","Marseille","Palermo","Kotor","Venice","Bari",
            "Corfu","Zakynthos","Athens","Messina","Genoa","Valencia","Santorini","Chania"
        };

        var decksArr = new[,]
        {
            { 7, 11 },
            { 8, 14 },
            { 9, 16 }
        };

        var cabinsArr = new[,]
        {
            { 84, 8 },
            { 92, 4 },
            { 124, 8 }
        };

        int[][] routesArr =
        {
            [5,9,0,4,3,14,7,13,1,10,2,12,6,11,8],
            [3,6,4,0,5,8,9,0,7,12,1,14,2,13,11,10],
            [6,11,4,3,10,0,9,7,14,2,13,1,8,12,5]
        };

        var cruisesArr = new[,]
        {
            { 0,2026,6,5, 2026,6,12 },
            { 0,2026,6,30, 2026,7,9 },
            { 1,2026,7,2, 2026,7,9 },
            { 1,2026,7,5, 2026,7,12 },
            { 2,2026,9,1, 2026,9,11 },
            { 2,2026,9,3, 2026,9,9 }
        };

        var seasonStart = new DateOnly(2026, 6, 1);
        var seasonEnd = new DateOnly(2026, 9, 30);

        var cabinCoefficient = new[] { 1, 1.2, 1.3, 1.6 };

        decimal initialPrice = 150.23m;
        decimal minimalPrice = 198.87m;
        double seasonHighCoefficient = 1.51;
        int seasonMiddle = seasonEnd.DayNumber - seasonStart.DayNumber;

        using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            if (!db.Points.Any())
                SeedPoints(db, pointsList);

            if (!db.Ships.Any())
                SeedShips(db, shipsList, decksArr, cabinsArr, routesArr, seasonStart, seasonEnd, pointsList);

            if (!db.Cruises.Any())
                SeedCruises(db, cruisesArr, shipsList, cabinCoefficient,
                    initialPrice, minimalPrice, seasonHighCoefficient, seasonMiddle);

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        Console.WriteLine("Database seeded.");
    }

    static void SeedPoints(ApplicationDbContext db, List<string> pointsList)
    {
        foreach (var pt in pointsList)
        {
            db.Points.Add(new Point { Name = pt, IsSea = pt == "At Sea" });
        }

        db.SaveChanges();
    }

    static void SeedShips(
        ApplicationDbContext db,
        List<string> shipsList,
        int[,] decksArr,
        int[,] cabinsArr,
        int[][] routesArr,
        DateOnly seasonStart,
        DateOnly seasonEnd,
        List<string> pointsList)
    {
        var pointsByName = db.Points.AsNoTracking()
            .ToDictionary(p => p.Name, p => p.Id);

        for (int sh = 0; sh < shipsList.Count; sh++)
        {
            var ship = new Ship { Name = shipsList[sh] };
            db.Ships.Add(ship);
            db.SaveChanges();

            int deckLow = decksArr[sh, 0];
            int deckUp = decksArr[sh, 1];
            int cabinAmount = cabinsArr[sh, 0];
            int cabinLess = cabinsArr[sh, 1];

            for (int dk = deckLow; dk <= deckUp; dk++)
            {
                int deckElement = dk % 3;

                var deck = new Deck
                {
                    Ship = ship,
                    Number = dk
                };

                db.Decks.Add(deck);

                if (cabinAmount <= 0)
                    continue;

                for (int i = 1; i <= cabinAmount; i++)
                {
                    int element = i % 4;

                    CabinType cabinType;

                    if (element == 1 || element == 2)
                    {
                        cabinType = CabinType.Interior;
                    }
                    else
                    {
                        cabinType = deckElement switch
                        {
                            1 => CabinType.SeaView,
                            2 => CabinType.Balcony,
                            _ => CabinType.Suite
                        };
                    }

                    db.Cabins.Add(new Cabin
                    {
                        Deck = deck,
                        SequenceNumber = i,
                        CabinType = cabinType
                    });
                }

                cabinAmount = Math.Max(0, cabinAmount - cabinLess);
            }

            db.SaveChanges();

            var route = new Data.Models.Route { Ship = ship };
            db.Routes.Add(route);
            db.SaveChanges();

            int counter = 0;

            for (DateOnly date = seasonStart; date <= seasonEnd; date = date.AddDays(1))
            {
                db.RouteDays.Add(new RouteDay
                {
                    Date = date,
                    Route = route,
                    PointId = pointsByName[pointsList[routesArr[sh][counter]]]
                });

                counter++;

                if (counter >= routesArr[sh].Length)
                    counter = 0;
            }

            db.SaveChanges();
        }
    }

    static void SeedCruises(
        ApplicationDbContext db,
        int[,] cruisesArr,
        List<string> shipsList,
        double[] cabinCoefficient,
        decimal initialPrice,
        decimal minimalPrice,
        double seasonHighCoefficient,
        int seasonMiddle)
    {
        var cruiseDescriptions = new[]
        {
        "A Mediterranean journey through iconic ports.",
        "A summer cruise with vibrant cities.",
        "A relaxing Adriatic cruise.",
        "Discover Southern Europe.",
        "An early autumn cruise.",
        "A short but enriching cruise."
    };

        var cruises = new List<Cruise>();

        for (int i = 0; i < cruisesArr.GetLength(0); i++)
        {
            var shipRoute = db.Routes
                .Include(r => r.Ship)
                .Include(r => r.Days)
                .ThenInclude(d => d.Point)
                .First(r => r.Ship.Name == shipsList[cruisesArr[i, 0]]);

            var cruise = new Cruise(
                shipRoute,
                new DateOnly(cruisesArr[i, 1], cruisesArr[i, 2], cruisesArr[i, 3]),
                new DateOnly(cruisesArr[i, 4], cruisesArr[i, 5], cruisesArr[i, 6]),
                cruiseDescriptions[i]);

            cruises.Add(cruise);
        }

        db.Cruises.AddRange(cruises);
        db.SaveChanges();


        // ============================
        // ADD CABIN PRICES (Липсваше!)
        // ============================

        var cruisePrices = new List<CruiseCabinPrice>();

        foreach (var cruise in cruises)
        {
            int cruiseMiddle =
                ((cruise.LastDay.DayNumber - cruise.FirstDay.DayNumber) / 2)
                + cruise.FirstDay.DayNumber;

            double cruiseCoefficient =
                ((Math.Abs(seasonMiddle - cruiseMiddle) / (double)cruiseMiddle)
                * (seasonHighCoefficient - 1)) + 1;

            cruisePrices.AddRange(new[]
            {
            new CruiseCabinPrice
            {
                CruiseId = cruise.Id,
                CabinType = CabinType.Interior,
                Price = initialPrice + (minimalPrice * (decimal)(seasonHighCoefficient * cabinCoefficient[0] * cruise.CruiseLength))
            },
            new CruiseCabinPrice
            {
                CruiseId = cruise.Id,
                CabinType = CabinType.SeaView,
                Price = initialPrice + (minimalPrice * (decimal)(seasonHighCoefficient * cabinCoefficient[1] * cruise.CruiseLength))
            },
            new CruiseCabinPrice
            {
                CruiseId = cruise.Id,
                CabinType = CabinType.Balcony,
                Price = initialPrice + (minimalPrice * (decimal)(seasonHighCoefficient * cabinCoefficient[2] * cruise.CruiseLength))
            },
            new CruiseCabinPrice
            {
                CruiseId = cruise.Id,
                CabinType = CabinType.Suite,
                Price = initialPrice + (minimalPrice * (decimal)(seasonHighCoefficient * cabinCoefficient[3] * cruise.CruiseLength))
            }
        });
        }

        db.CruiseCabinPrices.AddRange(cruisePrices);
        db.SaveChanges();
    }

}