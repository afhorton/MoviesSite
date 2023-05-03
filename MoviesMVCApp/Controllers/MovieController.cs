using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoviesData;

namespace MoviesMVCApp.Controllers
{
    public class MovieController : Controller
    {
        private MovieContext _context {  get; set; } // auto-implemented property

        // context get  injected to the constructor
        public MovieController(MovieContext context)
        {
            _context = context;
        }


        // GET: MovieController
        // list of movies
        public ActionResult Index()
        {
            List<Movie> movies = null;
            try
            {
                movies = MovieManager.GetMovies(_context);
            }
            catch 
            {
                TempData["Message"] = "Database connection error. Try again later.";
                TempData["IsError"] = true;
            }
                
            return View(movies);
        }

        // filter movies by genre - we will invoke the view controller
        public ActionResult FilterByGenre(string id = "All")
        {
            try
            {
                // prepare list of genres for the drop down list
                       List<Genre> genres = MovieManager.GetGenres(_context);
                       var list = new SelectList(genres, "GenreId", "Name").ToList();
                       list.Insert(0, new SelectListItem("All", "All")); // add All as first option
                       ViewBag.Genres = list;
            }
            catch
            {
                TempData["Message"] = "Database connection error. Try again later.";
                TempData["IsError"] = true;
            }
            return View(); // Ajax function in the view calls the view component to get  movies
        }

        // method from Ajax - invokes view component 
        public ActionResult GetMoviesByGenre(string id) // genre ID
        {
            return ViewComponent("MoviesByGenre", id); // calls view component and returns its Default view
        }


        //public ActionResult FilteredList()
        //{
        //    List<Movie> movies = null;
        //    try
        //    {
        //        // prepare list of genres for the drop down list
        //        List<Genre> genres = MovieManager.GetGenres(_context);
        //        var list = new SelectList(genres, "GenreId", "Name").ToList();
        //        list.Insert(0, new SelectListItem("All", "All")); // add All as first option
        //        ViewBag.Genres = list;

        //        movies = MovieManager.GetMovies(_context); // all movies
        //    }
        //    catch
        //    {
        //        TempData["Message"] = "Database connection error. Try again later.";
        //        TempData["IsError"] = true;
        //    }
        //    return View(movies);
        //}

        //[HttpPost]
        //public ActionResult FilteredList(string id = "All")
        //{
        //    List<Movie> movies = null;
        //    try
        //    {
        //        // retail drop down genres and selected item
        //        List<Genre> genres = MovieManager.GetGenres(_context);
        //        var list = new SelectList(genres, "GenreId", "Name").ToList();
        //        list.Insert(0, new SelectListItem("All", "All")); // add All as first option
        //        foreach (var item in list)// find selected item
        //        {
        //            if (item.Value == id)
        //            {
        //                item.Selected = true;
        //                break;
        //            }
        //        }
        //        ViewBag.Genres = list;

        //        if (id == "All")
        //        {
        //            movies = MovieManager.GetMovies(_context); // all movies
        //        }
        //        else // a genre is selected
        //        {
        //            movies = MovieManager.GetMoviesByGenre(_context, id); // filtered movies
        //        }
        //    }
        //    catch
        //    {
        //        TempData["Message"] = "Database connection error. Try again later.";
        //        TempData["IsError"] = true;
        //    }
        //    return View(movies);
        //}


        // GET: MovieController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                Movie movie = MovieManager.GetMovieById(_context, id);
                return View(movie);
            }
            catch
            {
                TempData["Message"] = "Database connection error. Try again later.";
                TempData["IsError"] = true;
                return View(null);
            }
        }

        // GET: MovieController/Create
        public ActionResult Create()
        {
            try
            {
                // prepare list of genres for the drop down list
                List<Genre> genres = MovieManager.GetGenres(_context);
                var list = new SelectList(genres, "GenreId", "Name");
                ViewBag.Genres = list;
            }
            catch
            {
                TempData["Message"] = "Database connection error. Try again later.";
                TempData["IsError"] = true;
            }
            Movie movie = new Movie(); // empty movie object
            return View(movie);
        }

        // POST: MovieController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Movie newMovie) // data collected from the form
        {
            try
            {
                if(ModelState.IsValid)
                {
                    MovieManager.AddMovie(_context, newMovie);
                    TempData["Message"] = $"Successfully added movie {newMovie.Name}";
                    // do not set TempData["IsError"]
                    return RedirectToAction(nameof(Index));
                }
                else // validation errors
                {
                    return View(newMovie);
                }
            }
            catch
            {
                TempData["Message"] = "Database connection error. Try again later.";
                TempData["IsError"] = true;
                return View(newMovie);
            }
        }

        // GET: MovieController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                // prepare list of genres for the drop down list
                List<Genre> genres = MovieManager.GetGenres(_context);
                var list = new SelectList(genres, "GenreId", "Name");
                ViewBag.Genres = list;
                Movie movie = MovieManager.GetMovieById(_context, id);
                return View(movie);
            }
            catch
            {
                TempData["Message"] = "Database connection error. Try again later.";
                TempData["IsError"] = true;
                return View(null);
            }
        }

        // POST: MovieController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Movie newMovie)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MovieManager.UpdateMovie(_context, id, newMovie);
                    TempData["Message"] = $"Successfully updated movie {newMovie.Name}";
                    // do not set TempData["IsError"]
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(newMovie);
                }
            }
            catch
            {
                TempData["Message"] = "Database connection error. Try again later.";
                TempData["IsError"] = true;
                return View(newMovie);
            }
        }

        // GET: MovieController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                Movie movie = MovieManager.GetMovieById(_context, id);
                return View(movie);
            }
            catch
            {
                TempData["Message"] = "Database connection error. Try again later.";
                TempData["IsError"] = true;
                return View(null);
            }
        }

        // POST: MovieController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Movie movie) // there is no collection on the form
        {
            try
            {
                Movie oldMovie = MovieManager.GetMovieById(_context, id);
                if(oldMovie != null)
                {
                    MovieManager.DeleteMovie(_context, id);
                    TempData["Message"] = $"Successfully deleted movie {oldMovie.Name}";
                    // do not set TempData["IsError"]
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Message"] = "Database connection error. Try again later.";
                TempData["IsError"] = true;
                return View();
            }
        }
    }
}
