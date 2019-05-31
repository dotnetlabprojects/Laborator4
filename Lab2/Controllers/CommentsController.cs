using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2.Models;
using Lab2.Service;
using Microsoft.AspNetCore.Http;
using Lab2.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentService commentService;
        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }


        /// <summary>
        /// GET ALL COMMENTS.
        /// </summary>
        /// <param name="filter">Filter by given string.</param>
        /// <returns>A list of Comment objects.</returns>
        // GET: api/Comments/?filter=xyz
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<CommentGetModel> Get([FromQuery]string filter)
        {
            return commentService.GetAll(filter);
        }

    }
}