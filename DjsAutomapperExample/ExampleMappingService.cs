using AutoMapper;
using Data;
using DjsAutomapperExample.Models;
using System;

namespace DjsAutomapperExample
{
    public class ExampleMappingService
    {
        private IMapper _mapper;
        public ExampleMappingService(IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException("Mapper cannot be empty");
        }

        public ActorViewModel mappingExample()
        {
            return _mapper.Map<ActorViewModel>(new Actor() { dateOfBirth = DateTime.Now, Id = 1, Name = "Koen" });
        }
    }
}
