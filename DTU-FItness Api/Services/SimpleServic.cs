using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class SimpleService
{
    private readonly ApplicationDbContext _context;

    public SimpleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddIntegerAsync(SimpleModel model)
    {
        _context.SimpleTable.Add(model);
        await _context.SaveChangesAsync();
    }
}
