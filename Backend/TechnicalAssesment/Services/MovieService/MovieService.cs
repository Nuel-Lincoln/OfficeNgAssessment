using Newtonsoft.Json;
using System.Net.Http;
using TechnicalAssesment.Models.Request;
using TechnicalAssesment.Models.Response;
using TechnicalAssesment.Services.Encryption;

namespace TechnicalAssesment.Services.MovieService
{
    public class MovieService : IMovieService
    {
        private readonly IConfiguration _config;
        public MovieService(IConfiguration config)
        {
            _config = config;
             
        }

        public async Task<Models.Response.SeasonMovieRp> SeasonMovieRequest(Models.Request.SeasonMovieRq seasonMovie)
        {
            Models.Response.SeasonMovieRp resp = new Models.Response.SeasonMovieRp();
            EncryptionHelper helper = new EncryptionHelper(_config);
            string Url = _config.GetSection("Base").Value + seasonMovie.Title + "&Season="
                + seasonMovie.Season + "&Episode=" + seasonMovie.Episod + "&apiKey="
                + helper.Decrypt(_config.GetSection("IgnoreAgain").Value);
            var httpClient = new HttpClient();

            Serilog.Log.Information($"About to make call for Season movie of name : {seasonMovie.Title}, Season : {seasonMovie.Season}, and episode : {seasonMovie.Episod}");
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(Url);

                if (response.IsSuccessStatusCode)
                {
                    Serilog.Log.Information("Request was a success ");
                    var jsonString = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<Models.Response.SeasonMovieRp>(jsonString);

                    return resp;
                }
                else
                {
                    Serilog.Log.Information("Request was not a success ");
                    // resp.Title = response.StatusCode.ToString();

                    return resp;
                }
            }catch (Exception ex)
            {
                Serilog.Log.Information($"Request was not a success, an exception occurred :  {ex.Message}, stacktrace : {ex.StackTrace}, inner exception : {ex.InnerException}");
                resp.Title = "Error " + ex.Message;
                return resp;
            }

        }


        public async Task<Models.Response.NormalRp> NormalMovieRequest(Models.Request.NormalRq seasonMovie)
        {
            Models.Response.NormalRp resp = new Models.Response.NormalRp();
            EncryptionHelper helper = new EncryptionHelper(_config);
            string Url = _config.GetSection("Base").Value + seasonMovie.Title  + "&apiKey="
                + helper.Decrypt(_config.GetSection("IgnoreAgain").Value);
            Serilog.Log.Information($"About to make call for Normal movie of name : {seasonMovie.Title}, ");


            var httpClient = new HttpClient();
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(Url);

                if (response.IsSuccessStatusCode)
                {
                    Serilog.Log.Information("Request was a success ");
                    var jsonString = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<Models.Response.NormalRp>(jsonString);

                    return resp;
                }
                else
                {
                    Serilog.Log.Information("Request was not a success ");
                    //resp.Title = response.StatusCode.ToString();

                    return resp;
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Information($"Request was not a success, an exception occurred :  {ex.Message}, stacktrace : {ex.StackTrace}, inner exception : {ex.InnerException}");
                resp.Title = "Error Occurred "+ex.Message;
                return resp;
            }

        }

    }
}
