using Domain.Entities;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace PokeAcademy.Flur.Controllers
{
    [Route("api/pokemon")]
    [ApiController]
    public class PokemonFlurController : ControllerBase
    {
        private const string ALL_POKEMON_URL = "https://pokeapi.co/api/v2/pokemon";
        private const string EXTERNAL_BASE_URL = "https://localhost:7235";
        private const string TOKEN = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

        
        [HttpGet]
        public async Task<IActionResult> GetAll(int limit)
        {
            var result = await ALL_POKEMON_URL
                .SetQueryParams(new { limit = limit })
                .GetJsonAsync<NamedAPIResourceList>();

            var viewModelList = new PokemonListViewModel //personalizando a view model para mostrar nome e id
            {
                Count = result.Count,
                Pokemons = result.Results.Select(p =>
                {
                    var lastSegment = new Uri(p.Url).Segments.Last(); // 73/
                    var id = lastSegment.Remove(lastSegment.Length - 1); // 73

                    return new PokemonListItemViewModel { Id = int.Parse(id), Name = p.Name };
                })
            };

            return Ok(viewModelList);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await $"{ALL_POKEMON_URL}/{id}"
                .GetJsonAsync<Root>();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Ping ping)// neste cenário estou enviando um cabeçalho fixo e um como parametro
        {
            var result = await $"{EXTERNAL_BASE_URL}/api/random"
                .WithHeaders(new
                {
                    Authorization = TOKEN,
                    x_LuizLinkedin = "https://www.linkedin.com/in/gustavo-luiz-tech/"
                })
                .PostJsonAsync(ping);
            var json = await result.GetJsonAsync<ExternalResponse>();

            return Ok(json);
        }

    }

    public class Ping
    {
        public string Message { get; set; }
    }

    public class ExternalResponse
    {
        public List<string> Headers { get; set; }
        public Ping Data { get; set; }
    }

}
