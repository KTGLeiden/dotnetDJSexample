using System.Collections.Generic;

namespace Data
{
    public class Movie
    {
        public Movie () {
            MovieActors = new List<MovieActor>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public List<MovieActor> MovieActors {get;set;}
    }
}
