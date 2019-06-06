using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2.Models;
using Lab2.Service;
using Microsoft.AspNetCore.Http;
using Lab2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentService commentService;
        private IUserService userService;

        public CommentsController(ICommentService commentService, IUserService userService)
        {
            this.commentService = commentService;
            this.userService = userService;
        }


        /// <summary>
        /// GET :GET ALL COMMENTS
        /// </summary>
        /// <param name="filter">Filter by given string.</param>
        /// <param name="page"></param>
        /// <returns>A list of Comment objects.</returns>
        /// GET: api/Comments/?filter=xyz
        [HttpGet]
        [Authorize(Roles = "Regular,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public PaginatedList<CommentGetModel> Get([FromQuery]string filter, [FromQuery]int page = 1)
        {
            User addedBy = userService.GetCurrentUser(HttpContext);
            if (addedBy.UserRole == UserRole.UserManager)
            {
                return null;
            }
            page = Math.Max(page, 1);
            return commentService.GetAll(page, filter);

        }

        /// <summary>
        /// GET: GET COMMENT
        /// </summary>
        /// <param name="id">Comment id</param>
        /// <returns>The comment with the given id</returns>
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Regular,Admin")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var found = commentService.GetById(id);

            User addedBy = userService.GetCurrentUser(HttpContext);

            if (found == null && addedBy.UserRole == UserRole.UserManager)
            {
                return Forbid();
            }
            return Ok(found);
        }

        /// <summary>
        /// POST: ADD COMMENT
        /// </summary>
        ///<remarks>
        /// {
        ///  id: 3,
        ///  text: "first comment",
        ///  importan: false
        ///  }
        /// </remarks>
        /// <param name="comment">The comment to add</param>
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Regular,Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] Comment comment)
        {
            User addedBy = userService.GetCurrentUser(HttpContext);
            if (addedBy.UserRole == UserRole.UserManager)
            {
                return Forbid();
            }

            return Ok(commentService.Create(comment, addedBy));
        }
    

    /// <summary>
    ///  PUT: UPSERT COMMENT (Update/Insert Comment)
    /// </summary>
    /// <param name="id">the comment id to upsert</param>
    /// <param name="comment">Comment to upsert</param>
    /// <returns>The upsert comment</returns>
        [Authorize(Roles = "Regular,Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]

        public IActionResult Put(int id, [FromBody] Comment comment)
        {
            var result = commentService.Upsert(id, comment);
            User addedBy = userService.GetCurrentUser(HttpContext);

            if (addedBy.UserRole == UserRole.UserManager)
            {
                return Forbid();
            }

            return Ok(comment);
        }

        /// <summary>
        /// DELETE: DELETE COMMENT
        /// </summary>
        /// <param name="id">Comment id to delete</param>
        /// <returns>The deleted comment or null if there is no comment with the given id</returns>
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Regular,Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = commentService.Delete(id);

            User addedBy = userService.GetCurrentUser(HttpContext);

            if (result == null && addedBy.UserRole == UserRole.UserManager)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}