using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechnicalAssesment.Models.Request;
using TechnicalAssesment.Models.Response;
using TechnicalAssesment.Services.Encryption;
using TechnicalAssesment.Services.MovieService;

namespace TechnicalAssesment.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly  IMovieService _Mservice;
        public MovieController(IConfiguration config, IMovieService movie)
        {
            _Mservice = movie;
            _configuration = config;
            
        }


        [HttpGet("Encrypt")]
        public string EncryptValue(string value)
        {
            EncryptionHelper helper = new EncryptionHelper(_configuration);

            return helper.Encrypt(value);
        }

        [HttpPost("Season")]
        public async Task<SeasonMovieRp> Season(SeasonMovieRq value)
        {
            var resp = await _Mservice.SeasonMovieRequest(value);

            return resp;
        }

        [HttpPost("Normal")]
        public async Task<NormalRp> Normal(NormalRq value)
        {
            return await _Mservice.NormalMovieRequest(value);
        }
    }
}
