using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using DtuFitnessApi.Models;
using DTU_FItness_Api.Models.ClubModels;

namespace DtuFitnessApi.Services;
public class ClubService
{
    private readonly ApplicationDbContext _context;
    private readonly NotificationService _notificationService;

    public ClubService(ApplicationDbContext context, NotificationService notificationService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _notificationService = notificationService;
    }

    public async Task<string> FindUserIdByUsernameAsync(string username)
    {
        var identityUserId = await _context.UserProfiles
            .Where(u => u.Username == username)
            .Select(u => u.IdentityUserID) // Now a string, no need to call ToString()
            .FirstOrDefaultAsync();

        return identityUserId; // This will be null if the user is not found
    }

    public async Task<ClubModel> CreateClubAsync(ClubModel newClub)
{
    if (newClub == null)
        throw new ArgumentNullException(nameof(newClub));
    if (string.IsNullOrWhiteSpace(newClub.ClubName))
        throw new ArgumentException("Club name is required.", nameof(newClub.ClubName));
    if (string.IsNullOrWhiteSpace(newClub.OwnerUsername))
        throw new ArgumentException("Owner username is required.", nameof(newClub.OwnerUsername));

    // Use OwnerUsername to find the user's IdentityUserID
    var ownerUserId = await FindUserIdByUsernameAsync(newClub.OwnerUsername);
    if (string.IsNullOrWhiteSpace(ownerUserId))
    {
        throw new ArgumentException("User not found.", nameof(newClub.OwnerUsername));
    }

    // Assign the IdentityUserID to the OwnerUserId property
    newClub.OwnerUserId = ownerUserId; // Now it is a string from the UserProfiles table
    newClub.ClubID = Guid.NewGuid().ToString(); // Generate a new GUID as a string for ClubID
    newClub.CreationDate = DateTime.UtcNow; // Set the current UTC time as the creation date

    // Note: OwnerUsername is marked with [NotMapped] so it won't be stored in the database
    // We only use it to look up the OwnerUserId

    // Add the newClub instance to the context
    _context.Clubs.Add(newClub);
    // Save changes to the database
    await _context.SaveChangesAsync();

    return newClub; // Return the newly created club model
}


    public async Task<(bool Success, string Message)> RegisterMemberAsync(string username, string clubName)
{
    // Get the UserProfile using the username
    var userProfile = await _context.UserProfiles
        .FirstOrDefaultAsync(u => u.Username == username);
    if (userProfile == null)
        return (false, "User not found.");

    // Get the Club using the clubName
    var club = await _context.Clubs
        .FirstOrDefaultAsync(c => c.ClubName == clubName);
    if (club == null)
        return (false, "Club not found.");

    // Check if the user is already a member of the club
    var isMember = await _context.ClubMembers
    .AnyAsync(cm => cm.MemberId == userProfile.IdentityUserID && cm.ClubId == club.ClubID);
if (isMember)
    return (false, "User is already a member of the club.");

    // Register the user as a member of the club
    var clubMember = new ClubMember
    {
        ClubId = club.ClubID, // Assign the ClubID as a string
        MemberId = userProfile.IdentityUserID.ToString() // Assign the IdentityUserID as a string
    };

    _context.ClubMembers.Add(clubMember);
    await _context.SaveChangesAsync();

    return (true, "User registered to club successfully.");
}

    public async Task<Event> CreateEventAsync(EventCreationDto eventDto)
{
    var club = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubName == eventDto.ClubName);
    if (club == null)
    {
        throw new ArgumentException("Club not found.");
    }

    var newEvent = new Event
    {
        ClubID = club.ClubID, // ClubID found based on ClubName
        Title = eventDto.Title,
        Description = eventDto.Description,
        EventDate = eventDto.EventDate
    };

    _context.Events.Add(newEvent);
    await _context.SaveChangesAsync();

    await _notificationService.CreateNotificationsForEvent(newEvent);

    return newEvent;
}




    public async Task<bool> DeleteClubByNameAsync(string clubName)
{
    var club = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubName == clubName);
    if (club != null)
    {
        _context.Clubs.Remove(club);
        await _context.SaveChangesAsync();
        return true;  // Return true if the club was found and deleted
    }
    return false;  // Return false if no club was found to delete
}


public async Task<bool> UpdateClubDescriptionAsync(string clubName, string newDescription)
{
    var club = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubName == clubName);
    if (club == null)
    {
        return false;  // Club not found
    }

    club.Description = newDescription;
    await _context.SaveChangesAsync();
    return true;  // Successfully updated
}

public async Task<bool> UpdateClubOwnerByUsernameAsync(string clubName, string newOwnerUsername)
{
    // First find the user by username to get the IdentityUserID
    var newUser = await _context.UserProfiles
                                .FirstOrDefaultAsync(u => u.Username == newOwnerUsername);
    if (newUser == null)
    {
        return false; // New owner user not found
    }

    // Then find the club and update its owner
    var club = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubName == clubName);
    if (club == null)
    {
        return false; // Club not found
    }

    club.OwnerUserId = newUser.IdentityUserID;
    await _context.SaveChangesAsync();
    return true; // Successfully updated
}

    public async Task<List<ClubDto>> GetAllClubsAsync()
    {
        var clubs = await _context.Clubs
            .Select(c => new ClubDto
            {
                ClubID = c.ClubID,
                ClubName = c.ClubName,
                Description = c.Description,
                OwnerUserId = c.OwnerUserId,
                CreationDate = c.CreationDate
            })
            .ToListAsync();

        return clubs;
    }

    public async Task<List<ClubMemberDto>> GetAllClubMembersAsync(string clubId)
    {
        return await _context.ClubMembers
            .Where(cm => cm.ClubId == clubId)
            .Select(cm => new ClubMemberDto
            {
                ClubMemberId = cm.ClubMemberId,
                ClubId = cm.ClubId,
                MemberId = cm.MemberId,
                MemberName = cm.UserProfile.Username
            })
            .ToListAsync();
    }



}

