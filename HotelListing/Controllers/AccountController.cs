using AutoMapper;
using HotelListing.DTOs;
using HotelListing.Models;
using HotelListing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    public class AccountController:ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
       // private readonly SignInManager<ApiUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public AccountController(UserManager<ApiUser> userManager, ILogger<AccountController> looger, IMapper mapper, IAuthManager authManager)
        {
            _userManager = userManager;
            // _signInManager = signInManager;
            _logger = looger;
            _mapper = mapper;
            _authManager = authManager;
        }
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody]UserDTO userDTO)
        {
            _logger.LogInformation($"Registeration Attempt for {userDTO.Email}.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<ApiUser>(userDTO);
                user.UserName = userDTO.Email;  
                var result = await _userManager.CreateAsync(user,userDTO.Password);
                if (!result.Succeeded)
                {
                    foreach(var err in result.Errors)
                    {
                        ModelState.AddModelError(err.Code, err.Description);
                    }
                    return BadRequest(ModelState);
                }
                await _userManager.AddToRolesAsync(user, userDTO.Roles);
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong in the Registration.");
                return Problem("Something went wrong in the Registration.", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            _logger.LogInformation($"Login Attempt for {loginUserDTO.Email}.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool result = await _authManager.ValidateUser(loginUserDTO);
                if (!result)
                {
                    return Unauthorized(loginUserDTO);
                }
                return Accepted(new { Token = await _authManager.CreateToken()} );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong in the Registration.");
                return Problem("Something went wrong in the Registration.", statusCode: 500);
            }
        }
    }
}
