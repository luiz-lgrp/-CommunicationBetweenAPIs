using Microsoft.AspNetCore.Mvc;
using PokeAcademy.Refit.Controllers;
using Refit;

namespace PokeAcademy.Refit.Services
{
    public interface IExternalService
    {
        [Post("/api/random")]
        [Headers("x-LuizLinkedin: https://www.linkedin.com/in/gustavo-luiz-tech/")]
        Task<ExternalResponse> PostHeader(Ping ping, [Header("Authorization")] string Token);

    }
}
