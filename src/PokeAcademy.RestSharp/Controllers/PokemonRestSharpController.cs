using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace PokeAcademy.RestSharp.Controllers
{
    [Route("api/pokemon")]
    [ApiController]
    public class PokemonRestSharpController : ControllerBase
    {
        private const string ALL_POKEMON_URL = "https://pokeapi.co/api/v2/pokemon";
        private const string EXTERNAL_BASE_URL = "https://localhost:7235";
        private const string TOKEN = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";


        [HttpGet]
        public async Task<IActionResult> GetAll(int limit)
        {
            var client = new RestClient(ALL_POKEMON_URL);

            var request = new RestRequest("", Method.Get)
                .AddQueryParameter("limit", limit);

            var result = await client.GetAsync<NamedAPIResourceList>(request);

            var viewModel = MapToViewModel(result);


            return Ok(viewModel);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = new RestClient($"{ALL_POKEMON_URL}/{id}");

            var request = new RestRequest("", Method.Get);

            var result = await client.GetAsync<Root>(request);


            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Post(Ping ping)// neste cenário estou enviando um cabeçalho fixo e um como parametro
        {
            var client = new RestClient($"{EXTERNAL_BASE_URL}/api/random");

            var request = new RestRequest("", Method.Post)
                .AddJsonBody(ping)
                .AddHeader("Autohrization", TOKEN)
                .AddHeader("x-LuizLinkedin", "https://www.linkedin.com/in/gustavo-luiz-tech/");

            var result = await client.PostAsync(request);

            var json = JsonConvert.DeserializeObject<ExternalResponse>(result.Content);
           
            return Ok(json);
        }


        private PokemonListViewModel MapToViewModel(NamedAPIResourceList resourceList)
        {
            return new PokemonListViewModel
            {
                Count = resourceList.Count,
                Pokemons = resourceList.Results.Select(p =>
                {
                    var lastSegment = new Uri(p.Url).Segments.Last();
                    var id = lastSegment.Remove(lastSegment.Length - 1);

                    return new PokemonListItemViewModel { Id = int.Parse(id), Name = p.Name };
                })
            };
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
