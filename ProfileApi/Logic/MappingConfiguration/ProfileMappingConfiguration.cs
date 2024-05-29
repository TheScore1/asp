using Logic.Managers.Profile.Dto;
using Mapster;
using Persistence.Entities;

namespace Logic.MappingConfiguration;

public sealed class ProfileMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Profile, GetProfileResponse>()
            .Map(dest => dest.Email, src => src.Email.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Surname, src => src.Surname.Value);

        config.NewConfig<Profile, GetProfileByIdResponse>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Surname, src => src.Surname.Value);
    }
}