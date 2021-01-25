using System;
using EFDataService.Model;
using EFDataService.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PromoCodeManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginArgument arg)
        {
            try
            {
                var userDTO = _userRepository.Login(arg);
                if (userDTO == null)
                {
                    return Unauthorized();
                }
                return Ok(userDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Authorize]
        [HttpGet]
        [Route("Get")]
        public IActionResult Get(string email)
        {
            try
            {
                var user = _userRepository.Get(email);
                user.Password = null;
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
