namespace shortener_back.Configurations;

public class MapperConfigs : /*AutoMapper*/ Profile
{
    public MapperConfigs()
    {
        CreateMap<Shorten, ShortenDto>();
        CreateMap<CreateShortenDto, Shorten>();
        
        CreateMap<User, UserDto>()
            .ForMember(
                x => x.Shortens,
                opt => opt.MapFrom(src => src.Shortens)
            );
    }
}
