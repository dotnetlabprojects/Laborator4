using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Models
{
    public class MoviesDbSeeder
    {

        public static void Initialize(MoviesDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any products.
            if (context.Movies.Any())
            {
                return;   // DB has been seeded
            }

            context.Movies.AddRange(
                new Movie
                {
                    Title = "Titanic",
                    Description = "Ship sinks",
                    MovieGenre = MovieGenre.action,
                    Duration = 195,
                    YearOfRelease = 1997,
                    Director = "James Cameron",
                    DateAdded = DateTime.Parse("3/4/2019 19:53"),
                    Rating = 9,
                    MovieWatched = MovieWatched.yes
                },
                new Movie
                {
                    Title = "2001: A Space Odyssey",
                    Description = "Black structure provides connection between past and future",
                    MovieGenre = MovieGenre.action,
                    Duration = 164,
                    YearOfRelease = 1968,
                    Director = "Stanley Kubrick",
                    DateAdded = DateTime.Parse("1/5/2019 12:53"),
                    Rating = 9,
                    MovieWatched = MovieWatched.no
                }
            );
            context.SaveChanges();
        }
    }
}
