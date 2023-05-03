using Microsoft.EntityFrameworkCore; // for Include
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MoviesData
{
    /// <summary>
    /// methods for working with Movie table in Movies database
    /// </summary>
    public static class MovieManager
    {

       /// <summary>
       /// retrieve  all movies
       /// </summary>
       /// <param name="db">context object</param>
       /// <returns>list of movies or null if none</returns>
        public static List<Movie> GetMovies(MovieContext db) // dependency injection
        {
            List<Movie> movies = null;
            //using(MovieContext db = new MovieContext())
            //{
                movies = db.Movies.Include(m => m.Genre).ToList();
            //}
            return movies;
        }

        /// <summary>
        /// retrieve movie genres
        /// </summary>
        /// <param name="db">context object</param>
        /// <returns>list of genres</returns>
        public static List<Genre> GetGenres(MovieContext db)
        {
            List<Genre> genres = db.Genres.OrderBy(g => g.Name).ToList();
            return genres;
        }

        /// <summary>
        /// retrieves movies of given genre, ordered by name
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="genreId">ID of the genre</param>
        /// <returns>list of movies or null</returns>
        public static List<Movie> GetMoviesByGenre(MovieContext db, string genreId)
        {
            List<Movie> movies = db.Movies.Where(m => m.GenreId == genreId).
                Include(m => m.Genre).OrderBy(m => m.Name).ToList();           
            return movies;
        }
        
        /// <summary>
        /// get movie with given ID
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="id">ID of the  movie to find</param>
        /// <returns>movie or null if not found</returns>
        public static Movie GetMovieById(MovieContext db, int id) 
        {
            Movie movie = db.Movies.Find(id);
            return movie;
        }


        /// <summary>
        /// add another movie to the table
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="movie">new movie to add</param>
        public static void AddMovie(MovieContext db, Movie movie)
        {
            if(movie != null)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// update movie with given id using new movie data
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="id">id for the movie to update</param>
        /// <param name="newMovie">new movie data</param>
        public static void UpdateMovie(MovieContext db, int id, Movie newMovie)
        {
            Movie movie = db.Movies.Find(id);
            if(movie != null)
            {
                // copy over new movie data
                movie.Name = newMovie.Name;
                movie.Year = newMovie.Year;
                movie.Rating = newMovie.Rating;
                movie.GenreId = newMovie.GenreId;
                db.SaveChanges();
            }
        }
       
        /// <summary>
        /// delete movie with given id
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="id">ID of the movie to delete</param>
        public static void DeleteMovie(MovieContext db, int id)
        {
            Movie movie = db.Movies.Find(id);
            if(movie != null)
            {
                db.Movies.Remove(movie);
                db.SaveChanges();
            }
        }
    }
}
