using AutoMapper;
using Jwt.Dto;
using Jwt.Entity;


namespace PlatformServiceCore.Profiles
{
    public class ClienteProfiles : Profile
    {
        public ClienteProfiles()
        {
            CreateMap<Cliente, ClienteResponse>();
            CreateMap<ClienteRequest, Cliente>();
        }
    }
}

