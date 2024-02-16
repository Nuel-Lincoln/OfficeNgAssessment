namespace TechnicalAssesment.Services.MovieService
{
    public interface IMovieService
    {
        public Task<Models.Response.SeasonMovieRp> SeasonMovieRequest(Models.Request.SeasonMovieRq seasonMovie);

        public Task<Models.Response.NormalRp> NormalMovieRequest(Models.Request.NormalRq NormalMovie);
    }
}
