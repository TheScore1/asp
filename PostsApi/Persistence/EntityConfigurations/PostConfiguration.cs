using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;
using Persistence.ValueObjects;

namespace Persistence.EntityConfigurations;

internal class PostConfiguration : IEntityTypeConfiguration<Post>
{
	public void Configure(EntityTypeBuilder<Post> builder)
	{
		builder.HasKey(p => p.Id);

		builder.ComplexProperty(p => p.PostInfo).IsRequired();
		
		builder.ToTable("Posts");
	}
}