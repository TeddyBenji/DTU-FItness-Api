using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class ClubService
{
    private readonly ApplicationDbContext _context;

    public ClubService(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<ClubModel> CreateClubAsync(ClubModel newClub)
    {
        if (newClub == null) throw new ArgumentNullException(nameof(newClub));
        if (string.IsNullOrWhiteSpace(newClub.ClubName)) throw new ArgumentException("Club name is required.", nameof(newClub.ClubName));

        // Optionally, you can add logic here to check for existing clubs with the same name, etc.

        // Setting the ClubId to a new GUID. If you prefer the database to generate this, make sure your database is configured accordingly.
        newClub.ClubID = Guid.NewGuid();

        await _context.Clubs.AddAsync(newClub);
        await _context.SaveChangesAsync();

        return newClub; // Returning the newly created club, now including the ClubId assigned by the database if it's generated there.
    }
}
