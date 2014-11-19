using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieScore.Models.ViewModels
{
    /// <summary>
    /// Utilized by the main movie search box
    /// </summary>
    public class SearchViewModel
    {
        [Required(ErrorMessage="You must enter a movie title in order to search.")]
        [MinLength(3, ErrorMessage=("The movie title is too short."))]
        [MaxLength(200, ErrorMessage=("The movie title is too long."))]
        public string Keyword { get; set; }
    }
}