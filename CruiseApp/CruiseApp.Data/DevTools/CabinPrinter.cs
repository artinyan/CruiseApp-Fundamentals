using CruiseApp.Data;
using Microsoft.EntityFrameworkCore;

public static class CabinPrinter
{
    public static async Task PrintCabinsForShipAsync(
        ApplicationDbContext db,
        int shipId)
    {
        var ship = await db.Ships
            .Include(s => s.Decks)
                .ThenInclude(d => d.Cabins)
            .FirstOrDefaultAsync(s => s.Id == shipId);

        if (ship == null)
        {
            Console.WriteLine("❌ Ship not found.");
            return;
        }

        Console.WriteLine($"🛳 Ship: {ship.Name}");

        foreach (var deck in ship.Decks.OrderBy(d => d.Name))
        {
            Console.WriteLine($"Deck {deck.Name}:");

            foreach (var cabin in deck.Cabins.OrderBy(c => c.SequenceNumber))
            {
                Console.WriteLine(cabin.Name);
            }
        }
    }
}
