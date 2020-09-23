using System.Collections.Generic;

namespace DjsAutomapperExample.Models
{
    public class MovieDetailViewModel : MovieViewModel
    {
        public List<ActorViewModel> actors { get; set; }
    }
}
