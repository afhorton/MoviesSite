using Microsoft.AspNetCore.Mvc;
using MoviesData;

namespace MoviesMVCApp.Components
{
    public class MoviesByGenreViewComponent: ViewComponent
    {

        private MovieContext db; // context object
        // constructor with dependency injection
        public MoviesByGenreViewComponent(MovieContext context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id) // genre id
        {
            List<Movie> movies = null;
            if(id == "All")
            {
               movies = MovieManager.GetMovies(db);
            }
            else // specific genre
            {

                movies = MovieManager.GetMoviesByGenre(db,id);

            }

            return View(movies); // in Views/Shared/Components/MoviesByGenre/Default.cshtml
        }
    }
}
