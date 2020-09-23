using System;
using System.Collections.Generic;

namespace Data
{
    public class Actor
    {
        public Actor()
        {
            MovieActors = new List<MovieActor>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime dateOfBirth { get; set; }
        public List<MovieActor> MovieActors { get; set; }
    }
}
