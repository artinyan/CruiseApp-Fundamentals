using CruiseApp.Data;
using CruiseApp.Data.Models;
using CruiseApp.Data.Models.Enums;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Point = CruiseApp.Data.Models.Point;

// 1️. Find solution root depending current exe directory
var exeDir = AppContext.BaseDirectory; // the folder where starts .exe
var solutionRoot = Path.GetFullPath(Path.Combine(exeDir, "..", "..", "..", ".."));
// 4 times ".." because bin/Debug/net8.0 is on 4 levels under solution root

// 2️. Path to .env in CruiseApp.Web
var envPath = Path.Combine(solutionRoot, "CruiseApp.Web", ".env");

// 3️. Load .env
Env.Load(envPath);


var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found in environment variables.");

var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseSqlServer(connectionString)
    .Options;

using var db = new ApplicationDbContext(options);



var shipsList = new List<string> { "Aurora", "Ariel", "Neptune" };

var pointsList = new List<string> { "At Sea", "Barcelona", "Marseille", "Palermo", "Kotor", "Venice", "Bari", "Corfu", "Zakynthos", "Athens", "Messina", "Genoa", "Valencia", "Santorini", "Chania" };

var decksArr = new[,] {
    { 7, 11 },
    { 8, 14 },
    { 9, 16 }};

var cabinsArr = new[,] {
    { 84, 8 },
    { 92, 4 },
    { 124, 8 }};

int[][] routesArr = {
    [ 5,9,0,4,3,14,7,13,1,10,2,12,6,11,8 ],
    [ 3,6,4,0,5,8,9,0,7,12,1,14,2,13,11,10 ],
    [ 6,11,4,3,10,0,9,7,14,2,13,1,8,12,5 ]};

var cruisesArr = new[,] {
    { 0, 2026, 6, 5, 2026, 6, 12 },
    { 0, 2026, 6, 30, 2026, 7, 9 },
    { 1, 2026, 7, 2, 2026, 7, 9 },
    { 1, 2026, 7, 5, 2026, 7, 12 },
    { 2, 2026, 9, 1, 2026, 9, 11 },
    { 2, 2026, 9, 3, 2026, 9, 9 }
};


var seasonStart = new DateOnly(2026, 6, 1);
var seasonEnd = new DateOnly(2026, 9, 30);
var cabinCoefficient = new[] { 1, 1.2, 1.3, 1.6 };

decimal initialPrice = 150.23m;
decimal minimalPrice = 198.87m;
double seasonHighCoefficient = 1.51;
int seasonMiddle = seasonEnd.DayNumber - seasonStart.DayNumber;

db.Database.Migrate();


using var transaction = db.Database.BeginTransaction();

try
{
    if (!db.Points.Any())
    {
        SeedPoints(pointsList);
    }

    if (!db.Ships.Any())
    {
        SeedShips(shipsList, decksArr, cabinsArr, seasonStart, seasonEnd);
    }

    if (!db.Cruises.Any())
    {
        SeedCruises(cruisesArr);
    }

    transaction.Commit();
}
catch
{
    transaction.Rollback();
    throw;
}

void SeedCruises(int[,] cruisesArr)
{
    var cruiseDescriptions = new[]
    {
        "A Mediterranean journey through iconic ports, combining culture, history, and breathtaking coastlines.",
        "An unforgettable summer cruise featuring vibrant cities, crystal-clear waters, and authentic cuisine.",
        "A relaxing Adriatic and Ionian Sea cruise with charming old towns and scenic island views.",
        "Discover Southern Europe with a perfect balance of leisure days at sea and cultural excursions.",
        "An early autumn cruise offering mild weather, stunning sunsets, and historic Mediterranean destinations.",
        "A short but enriching cruise ideal for first-time travelers and weekend explorers."
    };

    var cruises = new List<Cruise>();

    for (int i = 0; i < cruisesArr.GetLength(0); i++)
    {
        var shipRoute = db.Routes
            .Include(r => r.Ship)
            .Include(r => r.Days)
                .ThenInclude(rd => rd.Point)
            .First(r => r.Ship.Name == shipsList[cruisesArr[i, 0]]);

        var cruise = new Cruise(
            shipRoute,
            new DateOnly(
                cruisesArr[i, 1],
                cruisesArr[i, 2],
                cruisesArr[i, 3]),
            new DateOnly(
                cruisesArr[i, 4],
                cruisesArr[i, 5],
                cruisesArr[i, 6]),
            cruiseDescriptions[i]
        );

        cruises.Add(cruise);
    }

    db.Cruises.AddRange(cruises);
    db.SaveChanges();


    // Add Price
    var cruisePrices = new List<CruiseCabinPrice>();

    foreach (var cruise in cruises)
    {
        int cruiseMiddle = ((cruise.LastDay.DayNumber - cruise.FirstDay.DayNumber) / 2) + cruise.FirstDay.DayNumber;
        double cruiseCoefficient = ((Math.Abs(seasonMiddle - cruiseMiddle) / cruiseMiddle) * (seasonHighCoefficient - 1)) + 1;

        cruisePrices.AddRange(new[]
        {
            new CruiseCabinPrice
            {
                CruiseId = cruise.Id,
                CabinType = CabinType.Interior,
                Price = CalculatePrice(cabinCoefficient[0], cruise.CruiseLength)
            },
            new CruiseCabinPrice
            {
                CruiseId = cruise.Id,
                CabinType = CabinType.SeaView,
                Price = CalculatePrice(cabinCoefficient[1], cruise.CruiseLength)
            },
            new CruiseCabinPrice
            {
                CruiseId = cruise.Id,
                CabinType = CabinType.Balcony,
                Price = CalculatePrice(cabinCoefficient[2], cruise.CruiseLength)
            },
            new CruiseCabinPrice
            {
                CruiseId = cruise.Id,
                CabinType = CabinType.Suite,
                Price = CalculatePrice(cabinCoefficient[3], cruise.CruiseLength)
            }
        });
    }

    db.CruiseCabinPrices.AddRange(cruisePrices);
    db.SaveChanges();
}

decimal CalculatePrice(double cabinCoeff, int cruiseLength)
{
    return initialPrice + (minimalPrice * (decimal)(seasonHighCoefficient * cabinCoeff * cruiseLength));
}

void SeedPoints(List<string> pointsList)
{
    foreach (var pt in pointsList)
    {
        db.Points.Add(new Point { Name = pt, IsSea = pt == "At Sea" });
    }
    db.SaveChanges();
}

void SeedShips(List<string> shipsList, int[,] decksArr, int[,] cabinsArr, DateOnly seasonStart, DateOnly seasonEnd)
{
    var pointsByName = db.Points
    .AsNoTracking()
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

                var cabin = new Cabin
                {
                    Deck = deck,
                    SequenceNumber = i,
                    CabinType = cabinType
                };

                db.Cabins.Add(cabin);
            }

            cabinAmount = Math.Max(0, cabinAmount - cabinLess);
        }
        db.SaveChanges();

        var route = new Route { Ship = ship };
        db.Routes.Add(route);
        db.SaveChanges();
        int counter = 0;
        for (DateOnly date = seasonStart; date <= seasonEnd; date = date.AddDays(1))
        {
            var routeDay = new RouteDay
            {
                Date = date,
                Route = route,
                PointId = pointsByName[pointsList[routesArr[sh][counter]]]
            };

            db.RouteDays.Add(routeDay);

            counter++;
            if (counter >= routesArr[sh].Length)
            {
                counter = 0;
            }
        }
        db.SaveChanges();
    }
}

Console.WriteLine("Seeding completed.");