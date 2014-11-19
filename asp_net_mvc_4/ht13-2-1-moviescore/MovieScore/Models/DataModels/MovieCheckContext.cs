using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MovieScore.Models.DataModels
{
    internal class MovieCheckContext : DbContext
    {
        /// <summary>
        /// Type of entities used in context (here, only Movie)
        /// </summary>
        public DbSet<Movie> Movies { get; set; }

        /// <summary>
        /// Connect to the database
        /// </summary>
        public MovieCheckContext()
            : base("name=MovieCheckConnectionString")
        {
            // Empty!
        }

        /// <summary>
        /// Prevents table re-creation
        /// </summary>
        static MovieCheckContext()
        {
            Database.SetInitializer<MovieCheckContext>(null);
        }

        /// <summary>
        /// Map the entity type of movie to the app.Movie table
        /// </summary>
        /// <param name="modelBuilder">The schema mapper</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().ToTable("Movie", "app");
        }
    }
}