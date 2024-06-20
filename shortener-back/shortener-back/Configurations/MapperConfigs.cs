namespace shortener_back.Configurations;

public class MapperConfigs : /*AutoMapper*/ Profile
{
    public MapperConfigs()
    {
        CreateMap<Shorten, ShortenDto>();
        CreateMap<User, UserDto>();
    }
}
