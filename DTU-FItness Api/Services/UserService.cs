using DtuFitnessApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Mysqlx;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<bool> UpdateBio(string userId, string newBio)
    {
    var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.IdentityUserID == userId);
    if (user == null)
    {
        return false;
    }

    user.Bio = newBio;
    _context.UserProfiles.Update(user);
    await _context.SaveChangesAsync();
    return true;
    }


    public async Task<string> GetBio(string userId)
    {
    var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.IdentityUserID == userId);
    return user?.Bio;  // This will return null if the user is not found, which is handled in the controller.
    }








}