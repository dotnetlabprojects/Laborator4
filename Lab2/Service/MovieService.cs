using Lab2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Service
{
    public interface IMovieService
    {

        IEnumerable<Movie> GetAll(DateTime? from = null, DateTime? to = null);
        Movie GetById(int id);
        Movie Create(Movie movie);
        Movie Upsert(int id, Movie movie);
        Movie Delete(int id);
    }
    public class MovieService : IMovieService
    {
        private MoviesDbContext context;
        public MovieService(MoviesDbContext context)
        {
            this.context = context;
        }

        public Movie Create(Movie movie)
        {
            context.Movies.Add(movie);
            context.SaveChanges();
            return movie;
        }

        public Movie Delete(int id)
        {
            var existing = context.Movies
                .Include(m => m.Comments)
                .FirstOrDefault(movie => movie.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Movies.Remove(existing);
            context.SaveChanges();
            return existing;
        }

        public IEnumerable<Movie> GetAll(DateTime? from = null, DateTime? to = null)
        {
            IQueryable<Movie> result = context.Movies.Include(m => m.Comments);
            if (from == null && to == null)
            {
                return result;
            }
            if (from != null)
            {
                result = result.Where(m => m.DateAdded >= from);
            }
            if (to != null)
            {
                result = result.Where(m => m.DateAdded <= to);
            }
            result = result.AsQueryable().OrderByDescending(m => m.YearOfRelease);

            return result;
        }


        public Movie GetById(int id)
        {
            // sau context.Movies.Find()
            return context.Movies
                .Include(m => m.Comments)
                .FirstOrDefault(m => m.Id == id);
        }

        public Movie Upsert(int id, Movie movie)
        {
            var existing = context.Movies.AsNoTracking().FirstOrDefault(m => m.Id == id);
            if (existing == null)
            {
                context.Movies.Add(movie);
                context.SaveChanges();
                return movie;
            }
            movie.Id = id;
            context.Movies.Update(movie);
            context.SaveChanges();
            return movie;
        }
    }
}
