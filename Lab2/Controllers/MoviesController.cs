using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2.Models;
using Lab2.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private IMovieService movieService;
        public MoviesController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        /// <summary>
        /// GET ALL MOVIES.
        /// </summary>
        /// <param name="from">Optional, filter by minimum DateAdded.</param>
        /// <param name="to">Optional, filter by maximum DateAdded.</param>
        /// <returns>A list of Movie objects.</returns>
        // GET: api/Movies
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<Movie> Get([FromQuery]DateTime? from, [FromQuery]DateTime? to)
        {
            return movieService.GetAll(from, to);
        }


        /// <summary>
        /// GET MOVIE BY ID
        /// </summary>
        /// <param name="id">Movie id</param>
        /// <returns>Movie</returns>
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var found = movieService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }


        /// <summary>
        /// ADD A MOVIE.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /movies
        ///    {
        ///        "title": "Cloud Atlas",
        ///        "description": "Explores how the actions and consequences of individual lives impact one another throughout the past, the present and the future.",
        ///        "movieGenre": 3,
        ///        "duration": 172,
        ///        "yearOfRelease": 2012,
        ///        "director": "Wachowski",
        ///        "dateAdded": "2017-03-04T11:40:00",
        ///        "rating": 9,
        ///        "movieWatched": 0,
        ///        "comments": [
        ///            {
        ///                "text": "a good movie",
        ///                "important": false
        ///            },
        ///            {
        ///                "text": "a thrilling movie",
        ///                "important": true
        ///            }
        ///        ]
        ///    }         
        ///</remarks>
        /// <param name="movie">The movie to add.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public void Post([FromBody] Movie movie)
        {
            movieService.Create(movie);
        }

        /// <summary>
        /// UPSERT MOVIE (Update/Insert Movie)
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /movies
        ///    {
        ///        "id": 4,
        ///        "title": "Psycho",
        ///        "description": "It stars Anthony Perkins, Janet Leigh, John Gavin, Vera Miles, and Martin Balsam, and was based on the 1959 novel of the same name by Robert Bloch.",
        ///        "movieGenre": 3,
        ///        "duration": 109,
        ///        "yearOfRelease": 1960,
        ///        "director": "Alfred Hitchcock",
        ///        "dateAdded": "2016-07-04T09:30:00",
        ///        "rating": 9,
        ///        "movieWatched": 0,
        ///        "comments": [
        ///            {
        ///                "id": 3,
        ///                "text": "an intense movie",
        ///                "important": false
        ///            },
        ///            {
        ///                "id": 4,
        ///                "text": "thrilling scenes",
        ///                "important": true
        ///            }
        ///        ]
        ///    }
        ///</remarks>
        /// <param name="id">Movie id</param>
        /// <param name="movie">The Movie to update/insert</param>
        /// <returns>Updated/Inserted Movie</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Movie movie)
        {
            var result = movieService.Upsert(id, movie);
            return Ok(result);
        }

        /// <summary>
        /// DELETE MOVIE
        /// </summary>
        /// <param name="id">Movie id</param>
        /// <returns>Deleted Movie</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var result = movieService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}