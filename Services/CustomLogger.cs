using ParkManagerAPI.Models;

namespace ParkManagerAPI.Services;

public class CustomLogger
{
    private readonly ParkManagerContext _context;

    public CustomLogger(ParkManagerContext context)
    {
        _context = context;
    }

    public async Task LogAsync(string type, string resource, string method, string message)
    {
        var log = new Log
        {
            Type = type,
            Resource = resource,
            Method = method,
            Message = message,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Logs.Add(log);
        await _context.SaveChangesAsync();
    }
}