using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;
using Persistence.ValueObjects;
using Persistence.ValueObjects.Profile;

namespace Persistence.EntitiesConfigurations;

public sealed class ProfileConfiguration: IEntityTypeConfiguration<Profile>
{
	public void Configure(EntityTypeBuilder<Profile> builder)
	{
		builder.HasKey(e => e.Id);

		builder.Property(e => e.Email)
			.HasConversion(e => e.Value,
				value => Email.Create(value).Value)
			.HasMaxLength(Email.EMAIL_MAX_LENGTH);

		builder.Property(e => e.Name)
			.HasConversion(n => n.Value,
				value => Name.Create(value).Value)
			.HasMaxLength(Name.NAME_MAX_LENGTH);

		builder.Property(e => e.Surname)
			.HasConversion(s => s.Value,
				value => Surname.Create(value).Value)
			.HasMaxLength(Surname.SURNAME_MAX_LENGTH);

		builder.ToTable("Profiles");
	}
}