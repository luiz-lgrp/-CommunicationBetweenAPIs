using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class NamedAPIResource
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class PokemonListItemViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
