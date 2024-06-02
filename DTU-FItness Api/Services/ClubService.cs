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
            .Select(u => u.IdentityUserID) 
            .FirstOrDefaultAsync();

        return identityUserId; 
    }

    public async Task<ClubModel> CreateClubAsync(ClubModel newClub)
{
    if (newClub == null)
        throw new ArgumentNullException(nameof(newClub));
    if (string.IsNullOrWhiteSpace(newClub.ClubName))
        throw new ArgumentException("Club name is required.", nameof(newClub.ClubName));
    if (string.IsNullOrWhiteSpace(newClub.OwnerUsername))
        throw new ArgumentException("Owner username is required.", nameof(newClub.OwnerUsername));

    
    var ownerUserId = await FindUserIdByUsernameAsync(newClub.OwnerUsername);
    if (string.IsNullOrWhiteSpace(ownerUserId))
    {
        throw new ArgumentException("User not found.", nameof(newClub.OwnerUsername));
    }

    
    newClub.OwnerUserId = ownerUserId; 
    newClub.ClubID = Guid.NewGuid().ToString(); 
    newClub.CreationDate = DateTime.UtcNow; 

   
    _context.Clubs.Add(newClub);
    
    await _context.SaveChangesAsync();

    return newClub; 
}


    public async Task<(bool Success, string Message)> RegisterMemberAsync(string username, string clubName)
{
    
    var userProfile = await _context.UserProfiles
        .FirstOrDefaultAsync(u => u.Username == username);
    if (userProfile == null)
        return (false, "User not found.");

   
    var club = await _context.Clubs
        .FirstOrDefaultAsync(c => c.ClubName == clubName);
    if (club == null)
        return (false, "Club not found.");

    
    var isMember = await _context.ClubMembers
    .AnyAsync(cm => cm.MemberId == userProfile.IdentityUserID && cm.ClubId == club.ClubID);
if (isMember)
    return (false, "User is already a member of the club.");

    
    var clubMember = new ClubMember
    {
        ClubId = club.ClubID,
        MemberId = userProfile.IdentityUserID.ToString()
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
        ClubID = club.ClubID, 
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
        return true; 
    }
    return false;
}


public async Task<bool> UpdateClubDescriptionAsync(string clubName, string newDescription)
{
    var club = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubName == clubName);
    if (club == null)
    {
        return false; 
    }

    club.Description = newDescription;
    await _context.SaveChangesAsync();
    return true; 
}

public async Task<bool> UpdateClubOwnerByUsernameAsync(string clubName, string newOwnerUsername)
{
    
    var newUser = await _context.UserProfiles
                                .FirstOrDefaultAsync(u => u.Username == newOwnerUsername);
    if (newUser == null)
    {
        return false; 
    }

    
    var club = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubName == clubName);
    if (club == null)
    {
        return false; 
    }

    club.OwnerUserId = newUser.IdentityUserID;
    await _context.SaveChangesAsync();
    return true; 
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

    public async Task<ClubModel> GetClubDetailsByNameAsync(string clubName)
    {
        return await _context.Clubs
            .Include(c => c.ClubMembers)
            .ThenInclude(cm => cm.UserProfile)
            .Include(c => c.Events)
            .FirstOrDefaultAsync(c => c.ClubName == clubName);
    }





}

