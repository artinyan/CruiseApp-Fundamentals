using CruiseApp.Data;
using CruiseApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Point = CruiseApp.Data.Models.Point;

//Console.WriteLine("SEED APP STARTED");
//Console.ReadLine();

var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseSqlServer(
        "Server=.;Database=CruiseAppDb;User Id=sa;Password=YourPassword!;TrustServerCertificate=True")
    .Options;


var shipsList = new List<string> { "Aurora", "Ariel", "Neptune" };

var pointsList = new List<string> { "At Sea", "Barcelona", "Marseille", "Palermo", "Kotor", "Venice", "Bari", "Corfu", "Zakynthos", "Athens", "Messina", "Genoa", "Valencia", "Santorini", "Chania" };

var decksArr = new[,] {
    { 7, 11 },
    { 8, 14 },
    { 9, 16 }};

var cabinsArr = new[,] {
    { 84, 8 },
    { 92, 6 },
    { 124, 10 }};

int[][] routesArr = {
    [ 5,9,0,4,3,14,7 ],
    [ 3,6,4,0,5,8,9,0,7,12 ],
    [ 6,11,4,3,10,0,9,7 ]};

var cruisesArr = new[,] {
    { 0, 2026, 6, 2, 2026, 6, 11 },
    { 0, 2026, 6, 30, 2026, 7, 7 },
    { 1, 2026, 7, 2, 2026, 7, 9 },
    { 1, 2026, 7, 3, 2026, 7, 10 },
    { 2, 2026, 9, 1, 2026, 9, 11 },
    { 2, 2026, 9, 3, 2026, 9, 9 } };


var seasonStart = new DateOnly(2026, 6, 1);
var seasonEnd = new DateOnly(2026, 9, 30);



using var db = new ApplicationDbContext(options);

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
    var cruises = new List<Cruise>();
    for (int i = 0; i < cruisesArr.GetLength(0); i++)
    {
        var shipRoute = db.Routes
            .Include(r => r.Ship)
            .Include(r => r.Days)
            .ThenInclude(rd => rd.Point)
            .First(r => r.Ship.Name == shipsList[cruisesArr[i, 0]]);

        var cruise = new Cruise(shipRoute,
            new DateOnly(
                 cruisesArr[i, 1],
                 cruisesArr[i, 2],
                 cruisesArr[i, 3]),
            new DateOnly(
                 cruisesArr[i, 4],
                 cruisesArr[i, 5],
                 cruisesArr[i, 6]));

        cruises.Add(cruise);
    }
    db.Cruises.AddRange(cruises);
    db.SaveChanges();
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
            var deck = new Deck
            {
                Ship = ship,
                Number = dk
            };
            db.Decks.Add(deck);

            if (cabinAmount > 0)
            {
                for (int i = 1; i <= cabinAmount; i++)
                {
                    var cabin = new Cabin
                    {
                        Deck = deck,
                        SequenceNumber = i
                    };
                    db.Cabins.Add(cabin);
                }
                cabinAmount = Math.Max(0, cabinAmount - cabinLess);
            }
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
                PointId = pointsByName[pointsList[routesArr[sh][counter]]]};

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