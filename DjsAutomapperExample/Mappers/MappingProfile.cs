
using AutoMapper;
using Data;
using DjsAutomapperExample.Models;

namespace DjsAutomapperExample.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Actor, ActorViewModel>();
            CreateMap<Movie, MovieViewModel>();
        }

    }
}
