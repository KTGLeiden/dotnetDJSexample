using System.Collections.Generic;

namespace DjsAutomapperExample.Models
{
    public class ActorDetailViewModel : ActorViewModel {
        public List<MovieViewModel> movies { get; set; }
    }
}
