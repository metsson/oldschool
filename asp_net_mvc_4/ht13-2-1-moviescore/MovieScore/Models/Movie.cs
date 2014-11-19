using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieScore.Models
{
    /// <summary>
    /// Contains movie properties reflecting table columns
    /// </summary>
    public partial class Movie
    {
        [Key]
        public int MovieID { get; set; }
        public double Score { get; set; }
        public System.DateTime LastCheck { get; set; }
        public string IMDbID { get; set; }
        public string Title { get; set; }
        public byte Credibility { get; set; }
        public string Voters { get; set; }
        public string Plot { get; set; }
        public string Poster { get; set; }

        /// <summary>
        /// Returns full path to movie poster
        /// </summary>
        public string PosterPath { get { return String.Format("Posters/{0}.jpg", this.Poster); } }
    }
}