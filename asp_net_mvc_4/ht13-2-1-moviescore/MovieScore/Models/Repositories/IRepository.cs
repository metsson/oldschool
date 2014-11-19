using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieScore.Models.Repositories
{
    public interface IRepository : IDisposable
    {
        void Add(Movie movie);
        void DeleteMovie(Movie movie);
        Movie GetMovieByID(int movieID);
        Movie FindMovieByTitle(string title);
        IEnumerable<Movie> GetTop100();
        void Save();
        void Update(Movie movie);
        IEnumerable<string> InstantSearch(string keyword);
    }
}
