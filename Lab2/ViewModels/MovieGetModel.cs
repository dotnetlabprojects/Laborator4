using Lab2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.ViewModels
{
    public class MovieGetModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [EnumDataType(typeof(MovieGenre))]
        public MovieGenre MovieGenre { get; set; }
        public int Duration { get; set; }
        public int YearOfRelease { get; set; }
        public string Director { get; set; }
        public DateTime DateAdded { get; set; }
        [Range(1, 10)]
        public int Rating { get; set; }
        [EnumDataType(typeof(MovieWatched))]
        public MovieWatched MovieWatched { get; set; }
        public int NumberOfComments { get; set; }

        public User AddedBy { get; set; }
        //public List<Comment> Comments { get; set; }

       
        public static MovieGetModel fromMovie(Movie movie)
        {

            return new MovieGetModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                MovieGenre = movie.MovieGenre,
                Duration = movie.Duration,
                YearOfRelease = movie.YearOfRelease,
                Director = movie.Director,
                DateAdded = movie.DateAdded,
                Rating = movie.Rating,
                MovieWatched = movie.MovieWatched,
                NumberOfComments = movie.Comments.Count(),
                AddedBy = movie.Owner == null ?null: movie.Owner

            };
        }
    }
}
