using Microsoft.EntityFrameworkCore;

public class ParkManagerContext : DbContext
{
    public ParkManagerContext(DbContextOptions<ParkManagerContext> options) : base(options) { }
}