using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using PokeAcademy.Refit.Controllers;
using Refit;

namespace PokeAcademy.Refit.Services
{
    public interface IPokeService
    {

        [Get("/pokemon")]
        Task<NamedAPIResourceList> GetAll(int limit);

        [Get("/pokemon/{id}")]
        Task<Root> GetById(int id);
    }

}
