using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MovieScore.Models;
using MovieScore.Models.DataModels;

namespace MovieScore.Models.Repositories
{
    /// <summary>
    /// Implements IRepository and uses Entity Frameworks to work with movie database
    /// </summary>
    public class EFRepository : IRepository 
    {

        #region Fields

        /// <summary>
        /// Used by IDisposable methods
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// Using the MovieCheckContext 
        /// </summary>
        private MovieCheckContext _entities = new MovieCheckContext();

        #endregion 

        #region Properties

        private IQueryable<Movie> AllMovies { get { return _entities.Movies.AsQueryable(); } }

        #endregion

        #region IRepository methods

        #region Select methods

        /// <summary>
        /// Retrieve a specific post from the table given the MovieID.
        /// </summary>
        /// <param name="movieID">int Movie.MovieID</param>
        /// <returns>Movie object</returns>
        public Movie GetMovieByID(int movieID)
        {
            return _entities.Movies.Find(movieID);
        }

        /// <summary>
        /// Select top 100 movies from database
        /// </summary>
        /// <returns>List of Movie instances</returns>
        public IEnumerable<Movie> GetTop100()
        {
            return AllMovies.Where(m => m.Score >= 6)                                  
                            .Take(100)
                            .Distinct()
                            .OrderByDescending(m => m.Score)
                            .ToList();
        }

        /// <summary>
        /// Utilized for autocomplete function
        /// </summary>
        /// <param name="keyword">The keyword(characters to search for) in titles</param>
        /// <returns>List of Movie titles</returns>
        public IEnumerable<string> InstantSearch(string keyword)
        {
            return AllMovies.Where(m => m.Title.Contains(keyword))
                            .Select(m => m.Title)
                            .Distinct()
                            .OrderBy(m => m)
                            .ToList();
        }

        /// <summary>
        /// Returns movie for given title, if any
        /// </summary>
        /// <param name="title">Valid SearchViewModel.Keyword</param>
        /// <returns>Movie instance or null</returns>
        public Movie FindMovieByTitle(string title) 
        {
            return AllMovies.Where(m => m.Title.StartsWith(title))
                            .Distinct()
                            .FirstOrDefault();
        }
        
        #endregion

        #region Update, delete and save

        /// <summary>
        /// Add a new post of Movie type into the table.
        /// </summary>
        /// <param name="contact">Contact object</param>
        public void Add(Movie movie)
        {
            _entities.Movies.Add(movie);
        }

        /// <summary>
        /// Delete a post from the table.
        /// </summary>
        /// <param name="movie">Movie object</param>
        public void DeleteMovie(Movie movie)
        {
            _entities.Movies.Remove(movie);
        }

        /// <summary>
        /// Update a post in the table.
        /// </summary>
        /// <param name="movie">Movie object</param>
        public void Update(Movie updatedMovie)
        {
            // Update mode as suggested by Ladislav @ http://goo.gl/iBO1eP
            var movieToUpdate = _entities.Movies
                                          .Where(m => m.MovieID == updatedMovie.MovieID)
                                          .FirstOrDefault();

            if (movieToUpdate != null)
            {
                _entities.Entry(movieToUpdate).CurrentValues.SetValues(updatedMovie);
            }            
        }

        /// <summary>
        /// Save EF action
        /// </summary>
        public void Save()
        {
            _entities.SaveChanges();
        }

        #endregion

        #endregion

        #region IDisposable methods

        /// <summary>
        /// Invokes Dispose(bool disposing) and GC to release sources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Used by Dispose()
        /// </summary>
        /// <param name="disposing">bool Release or not</param>
        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _entities.Dispose();
                }
            }
            _disposed = true;
        }

        #endregion
    }
}