using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence;

public class ApplicationDbContext
	: DbContext
{
	public ApplicationDbContext() {}
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions) {}

	public DbSet<Post> Posts { get; set; } = null!;
	

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

		modelBuilder.HasDefaultSchema("Posts");
	}
}