using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace MovieScore.Models
{
    /// <summary>
    /// Saves poster image received as API data to local folder
    /// </summary>
    public static class FetchPoster
    {
        /// <summary>
        /// Path to the folder containing posters
        /// </summary>
        public static readonly string SavePath = HttpContext.Current.Server.MapPath("~/Posters/");


        /// <summary>
        /// Saves retrieved jpg locally
        /// </summary>
        /// <param name="url">The full remote URL to the found image</param>
        /// <returns>Unique hashed image title without the file extension name</returns>
        public static string SaveImage(string url)
        {
            string hashedValue = Path.GetRandomFileName();
            string fullPath = Path.Combine(hashedValue, ".jpg");

            //string fullPath = String.Format("{0}{1}.jpg", SavePath, hashedValue);
            
            using (Stream imageStream = new WebClient().OpenRead(url))
            {
                Image img = Image.FromStream(imageStream);
                img.Save(fullPath);
            }

            return hashedValue;
        }
    }
}