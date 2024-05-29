using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence;

public class ProfileDbContext : DbContext
{
    public ProfileDbContext() { }
    public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options) { }

    public DbSet<Profile> Profiles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("profile");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProfileDbContext).Assembly);
    }
}