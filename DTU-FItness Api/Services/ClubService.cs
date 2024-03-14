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



    public async Task<string> FindUserIdByUsernameAsync(string username)
{
    var identityUserId = await _context.UserProfiles
        .Where(u => u.Username == username)
        .Select(u => u.IdentityUserID.ToString()) // Adjusted to reflect the correct property name
        .FirstOrDefaultAsync();

    return identityUserId; // This will be null if the user is not found
}



public async Task<ClubModel> CreateClubAsync(ClubModel newClub)
{
    if (newClub == null) throw new ArgumentNullException(nameof(newClub));
    if (string.IsNullOrWhiteSpace(newClub.ClubName)) throw new ArgumentException("Club name is required.", nameof(newClub.ClubName));
    if (string.IsNullOrWhiteSpace(newClub.OwnerUsername)) throw new ArgumentException("Owner username is required.", nameof(newClub.OwnerUsername));

    // Look up the OwnerUserId (IdentityUserID) based on the OwnerUsername
    var ownerUserId = await FindUserIdByUsernameAsync(newClub.OwnerUsername);
    if (string.IsNullOrWhiteSpace(ownerUserId))
    {
        throw new ArgumentException("User not found.", nameof(newClub.OwnerUsername));
    }

    // Convert the ownerUserId from string to Guid and assign it to OwnerUserId
    // This step may need adjustment based on the actual data type of OwnerUserId in your ClubModel and database
    Guid ownerUserGuid;
    if (!Guid.TryParse(ownerUserId, out ownerUserGuid) && !string.IsNullOrEmpty(ownerUserId))
    {
        // Assuming OwnerUserId in your ClubModel and database is a Guid. If it's another type, adjust accordingly.
        throw new ArgumentException("Invalid user ID format.", nameof(ownerUserId));
    }

    // Since we're working directly with ClubModel and assuming it's the entity model as well,
    // directly assign the OwnerUserId from the looked-up value
    newClub.OwnerUserId = ownerUserGuid; // Assign the GUID here
    newClub.ClubID = Guid.NewGuid(); // Ensure a new GUID is generated for ClubID
    newClub.CreationDate = DateTime.UtcNow; // Ensure the creation date is set to the current UTC time

    // Add the newClub instance to the context and save changes
    await _context.Clubs.AddAsync(newClub);
    await _context.SaveChangesAsync();

    return newClub; // Return the newly created club model
}





}
