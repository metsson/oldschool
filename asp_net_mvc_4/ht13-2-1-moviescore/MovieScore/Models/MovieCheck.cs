using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using MovieScore.Models.Repositories;
using MovieScore.Webservices;

namespace MovieScore.Models
{
    /// <summary>
    /// The core class of the application. Handling user request for a certain movie.
    /// Manages chaching, webservices and algorithm for movie score.
    /// </summary>
    public class MovieCheck
    {
        #region Fields and properties

        private IRepository _repository;
        private WebserviceFactory _webservices;
        private static readonly int _cacheTimeInDays = 1;

        #endregion

        #region Constructors

        /// <summary>
        /// The repository to use when caching, injected in the HomeController
        /// </summary>
        /// <param name="repository">Type of repository</param>
        public MovieCheck(IRepository repository)
        {
            _repository = repository;
            _webservices = new WebserviceFactory();
        }

        #endregion

        #region Movie scoring methods

        /// <summary>
        /// Get requested movie data if cached else make API calls.
        /// </summary>
        /// <param name="title">The search term entered by the user</param>
        /// <returns>Movie object</returns>
        public Movie GetMovie(string title)
        {
            var searchResult = _repository.FindMovieByTitle(title);
            var cacheTime = DateTime.Now;

            // Is the requested movie cached?
            if (searchResult != null)
            {
                // Is the cached version too old?
                if (cacheTime.Subtract(searchResult.LastCheck).Days > _cacheTimeInDays)
                {
                    return NewCache(searchResult);
                }
                // The cache is young and healthy
                return searchResult;
            }
            // The movie is not cached - let's make a new one
            return CalculateNewScore(title);
        }

        /// <summary>
        /// Used to re-cache an existing movie post
        /// </summary>
        /// <param name="movie">Instance of movie found as entity</param>
        /// <returns></returns>
        private Movie NewCache(Movie movie) 
        {            
            // Stripping away (release year) from movie title
            // RegEx from http://goo.gl/faCJ7h
            string regex = "(\\[.*\\])|(\".*\")|('.*')|(\\(.*\\))";
            string strippedTitle = Regex.Replace(movie.Title, regex, String.Empty);

            return CalculateNewScore(strippedTitle, movie.MovieID, movie.Poster);
        }

        /// <summary>
        /// Make a new movie scoring calculation given the title
        /// </summary>
        /// <param name="title">Search query from user</param>
        /// <param name="movieID">If given, movie post is updated instead</param>
        /// <returns>Movie instance</returns>
        private Movie CalculateNewScore(string title, int movieID = 0, string posterPath = null)
        {
            var APIData = _webservices.OpenMovieDatabase.MakeNewRequest(title);

            // The OpenMovieDatabase API response contains data
            if (APIData.ProcessingStatus)
            {
                // Get movie data even from MovieZine (SW)
                APIData = _webservices.MovieZine.MakeNewRequest(APIData);
            }
            else
            {
                throw new ApplicationException("No movie found or something else went wrong, notify user!");
            }

            var assembledMovieData = ProcessAPIData(APIData, movieID, posterPath);
            CacheMovie(assembledMovieData);
            return assembledMovieData;
        }

        #endregion 

        #region Movie scoring algorithms

        /// <summary>
        /// Processes gathered data from APIs and returns an instance of Movie
        /// </summary>
        /// <param name="APIData">Gathered data from APIs</param>
        /// <param name="movieID">If not null, there will be a post update</param>
        /// <param name="APIData">Do not save image poster again if it already exists</param>
        /// <returns>Movie instance</returns>
        private Movie ProcessAPIData(APIDataProcessor APIData, int movieID, string posterPath = null)
        {
            string poster = posterPath ?? FetchPoster.SaveImage(APIData.Poster);

            return new Movie
            {
                MovieID = movieID,
                IMDbID = APIData.ImdbID,
                Plot = APIData.Plot,
                Title = APIData.Title,
                Score = APIData.Score,
                LastCheck = DateTime.Now,
                Voters = APIData.Voters,
                Credibility = APIData.Credibility,
                Poster = poster
            };
        }
        
        #endregion

        #region Movie cache

        /// <summary>
        /// Save or update movie cache
        /// </summary>
        /// <param name="movie">Instance of Movie</param>
        /// <returns>Instance of Movie</returns>
        private Movie CacheMovie(Movie movie) 
        {
            if (movie.MovieID == 0)
            {
                _repository.Add(movie);
            }
            else
            {
                _repository.Update(movie);
            }
                      
            _repository.Save();
            return movie;
        }

        #endregion
    }
}