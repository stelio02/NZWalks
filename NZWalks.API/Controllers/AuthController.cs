﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        //REGISTER USER
        //POST: /api/auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequestDTO registerRequestDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.UserName,
                Email = registerRequestDTO.UserName,
            };

            var identityResult= await userManager.CreateAsync(identityUser,registerRequestDTO.Password);

            if (identityResult.Succeeded)
            {
                //Add roles to user
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
                {
                    identityResult= await userManager.AddToRolesAsync(identityUser, registerRequestDTO.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was created succesfully! Please log in.");
                    }

                }
                
            }
            return BadRequest("Wrong input!");
        }


        //LOGIN USER
        //POST: /api/auth/Login
        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody]LoginRequestDTO loginRequestDTO)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDTO.Username);

            if (user != null)
            {
                var checkPasswordResult= await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

                if (checkPasswordResult)
                {
                    //Get roles for this user
                    var roles= await userManager.GetRolesAsync(user);
                    if (roles != null) 
                    {
                        //Create Token
                        var jwtToken= tokenRepository.CreateJWTToken(user,roles.ToList());

                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken,
                        };

                        return Ok(response);
                    }


                      
                }

            }

            return BadRequest("Username or password incorrect!");
        }
    }
}
