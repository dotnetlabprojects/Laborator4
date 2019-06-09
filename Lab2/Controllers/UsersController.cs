using Lab2.Models;
using Lab2.Service;
using Lab2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// LOG IN USERS
        /// </summary>
        /// <param name="loggin"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LogginPostModel loggin)
        {
            var user = _userService.Authenticate(loggin.Username, loggin.Password);
 
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
        /// <summary>
        /// POST : CREATE A NEW USER
        /// </summary>
        /// <param name="registerModel">registerModel the user to create</param>
        /// <returns></returns>
        [Authorize(Roles = "UserManager,Admin")]
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterPostModel registerModel)
        {
            var date = DateTime.Now;

            User addedBy = _userService.GetCurrentUser(HttpContext);

            if ((addedBy.UserRole == UserRole.Regular) || (addedBy.UserRole == UserRole.UserManager && addedBy.RegistrationDate.AddMonths(6) > date && registerModel.UserRole == UserRole.UserManager))
            {
                return Forbid();
            }

            var user = _userService.Register(registerModel);
           

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
           
           
            return Ok(user);
        }

        [HttpGet]
        [Authorize(Roles = "UserManager,Admin")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var date = DateTime.Now;

            User addedBy = _userService.GetCurrentUser(HttpContext);

           if(addedBy.UserRole == UserRole.Regular)
            {
                return Forbid();
            }

           if (addedBy.UserRole == UserRole.UserManager && addedBy.RegistrationDate.AddMonths(6) > date )
            {
                List<UserGetModel> result = new List<UserGetModel>();

                foreach( UserGetModel user in users)
                {
                    if (!user.UserRole.ToString().Equals("UserManager"))
                    {
                       result.Add(user);
                    }
                }

                return Ok(result);
            }

            else
            {
                return Ok(users);
            }
        }


        /// <summary>
        ///  PUT: UPSERT USER (Update/Insert User)
        /// </summary>
        /// <param name="id">the user id to upsert</param>
        /// <param name="user">User to upsert</param>
        /// <returns>The upsert user</returns>

        [HttpPut("{id}")]
        [Authorize(Roles = "UserManager,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]

        public IActionResult Put(int id, [FromBody] User user)
        {
           
            User addedBy = _userService.GetCurrentUser(HttpContext);

            var date = DateTime.Now;

            if ((addedBy.UserRole == UserRole.Regular) || (addedBy.UserRole == UserRole.UserManager && addedBy.RegistrationDate.AddMonths(6) > date && user.UserRole == UserRole.UserManager))
            {
                return Forbid();
            }

            var result = _userService.Upsert(id, user);

            return Ok(user);
        }
        /// <summary>
        /// DELETE: DELETE USER
        /// </summary>
        /// <param name="id">User id to delete</param>
        /// <returns>The deleted user or null if there is no user with the given id</returns>
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        [Authorize(Roles = "UserManager,Admin")]
        public IActionResult Delete(int id)
        {
            var date = DateTime.Now;

            User addedBy = _userService.GetCurrentUser(HttpContext);

            IEnumerable<UserGetModel> users = _userService.GetAll();

            UserGetModel user = users.FirstOrDefault(u => u.Id == id);

            if ((addedBy.UserRole == UserRole.Regular) || (user!=null && addedBy.UserRole == UserRole.UserManager && addedBy.RegistrationDate.AddMonths(6) > date && user.UserRole == UserRole.UserManager))
            {
                return Forbid();
            }
            var result = _userService.Delete(id);

            if (result == null)
            {
                return NotFound();
            }
           
         
            return Ok(result);
        }
    }
}