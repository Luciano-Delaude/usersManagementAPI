using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using usersManagementAPI.Models;
using usersManagementAPI.Services;

using Microsoft.AspNetCore.Cors;

namespace usersManagementAPI.Controllers
{
    [EnableCors("Cors Rules")]
    [ApiController]
    [Route("UserApi/[controller]")]
    public class UserController : ControllerBase
    {
        public readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        [Route("ListAllActiveUsers")]

        public async Task<IActionResult> ListAllActiveUsers()
        {
            List<User> users = new List<User>();
            try
            {
                users = await _userService.ListAllActiveUsers();

                if (users.Count <= 0)
                {
                    return Ok(new { message = "There are no active users in the database", response = new List<User>() });
                }
                
                return StatusCode(StatusCodes.Status200OK, new { message = "Succesfully obtained the active users", response = users });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving active users", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("CreateNewUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            try 
            {
                User user = await _userService.CreateUser(userDto.UserName, userDto.UserBirthdate);
                return StatusCode(StatusCodes.Status200OK, new { message = "Succesfully created a new user", response = user });
            } 
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error - Unable to create new user", error = ex.Message });
            }
        }

        [HttpPut]
        [Route("UpdateUserState/{id}")]
        public async Task<IActionResult> UpdateUserState(int id, [FromBody] bool isActive)
        {
            try
            {
                if(await _userService.UpdateUserState(id, isActive))
                {
                    return StatusCode(StatusCodes.Status200OK, new { message = "Succesfully updated user status" });
                }
                return StatusCode(StatusCodes.Status404NotFound, new { message = "User not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                if(await _userService.DeleteUser(id))
                {
                    return StatusCode(StatusCodes.Status200OK, new { message = "Succesfully deleted user from database" });
                }
                return StatusCode(StatusCodes.Status404NotFound, new { message = "User not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
    