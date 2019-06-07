using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2.Models;
using Lab2.Service;
using Lab2.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        private IUserService userService;

        public MoviesController(IMovieService movieService, IUserService userService)
        {
            this.movieService = movieService;
            this.userService = userService;
        }

        /// <summary>
        /// GET ALL MOVIES.
        /// </summary>
        /// <param name="from">Optional, filter by minimum DateAdded.</param>
        /// <param name="to">Optional, filter by maximum DateAdded.</param>
        /// <returns>A list of Movie objects.</returns>
        // GET: api/Movies
        [HttpGet]
        [Authorize(Roles = "Regular,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public PaginatedList<MovieGetModel> Get([FromQuery]DateTime? from, [FromQuery]DateTime? to, [FromQuery]int page = 1)
        {
            User addedBy = userService.GetCurrentUser(HttpContext);
            page = Math.Max(page, 1);
            if (addedBy.UserRole == UserRole.UserManager)
            {
                return null;
            }
            return movieService.GetAll(page, from, to);

        }


        /// <summary>
        ///  GET: GET MOVIE
        /// </summary>
        /// <param name="id">Movie id</param>
        /// <returns>Movie</returns>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Regular,Admin")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var found = movieService.GetById(id);

            User addedBy = userService.GetCurrentUser(HttpContext);

            if (found == null && addedBy.UserRole != UserRole.Regular)
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
        [Authorize(Roles = "Regular,Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] Movie movie)
        {
            User addedBy = userService.GetCurrentUser(HttpContext);

          Movie result =  movieService.Create(movie, addedBy);

            if (addedBy.UserRole == UserRole.UserManager)
            {
                return Forbid();
            }
               return Ok();
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
        [Authorize(Roles = "Regular,Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Movie movie)
        {
            var result = movieService.Upsert(id, movie);

            User addedBy = userService.GetCurrentUser(HttpContext);

            if (addedBy.UserRole == UserRole.UserManager)
            {
                return Forbid();
            }

            return Ok(result);
        }

        /// <summary>
        /// DELETE MOVIE
        /// </summary>
        /// <param name="id">Movie id</param>
        /// <returns>Deleted Movie</returns>
        [Authorize(Roles = "Regular,Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var result = movieService.Delete(id);

            User addedBy = userService.GetCurrentUser(HttpContext);

            if (result == null && addedBy.UserRole == UserRole.UserManager)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}