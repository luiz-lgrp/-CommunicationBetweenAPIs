using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PokeAcademy.Refit.Services;

namespace PokeAcademy.Refit.Controllers
{
    [Route("api/pokemon")]
    [ApiController]
    public class PokemonRefitController : ControllerBase
    {

        private const string TOKEN = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

        private readonly IPokeService _pokeService;
        private readonly IExternalService _externalService;

        public PokemonRefitController(IPokeService pokeService, IExternalService externalService)
        {
            _pokeService = pokeService;
            _externalService = externalService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(int limit)
        {
            var result = await _pokeService.GetAll(limit);

            var viewModelList = MapToViewModel(result);

            return Ok(viewModelList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _pokeService.GetById(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Ping ping)
        {
            var result = await _externalService.PostHeader(ping, TOKEN);

            return Ok(result);
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
