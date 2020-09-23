
using AutoMapper;
using Data;
using DjsAutomapperExample.Models;
using System.Linq;

namespace DjsAutomapperExample.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Actor, ActorViewModel>();
            CreateMap<Actor, ActorDetailViewModel>()
                .ForMember(m => m.movies, o => o.MapFrom(s => s.MovieActors.Select(ma => ma.Movie)));
            CreateMap<Movie, MovieViewModel>();
            CreateMap<Movie, MovieDetailViewModel>()
                .ForMember(m => m.actors, o => o.MapFrom(s => s.MovieActors.Select(ma => ma.Actor)));
        }

    }
}
