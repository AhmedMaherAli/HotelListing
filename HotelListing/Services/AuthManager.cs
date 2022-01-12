using HotelListing.DTOs;
using HotelListing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Services
{
    public class AuthManager : IAuthManager
    {

        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration configuration;
        private  ApiUser _user;
        public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<string> CreateToken()
        {
            SigningCredentials signinCredentials = GetSigninCredentials();
            var claims = await GetClaims();
            var token = GenerateTokenOptions(signinCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signinCredentials, IList<Claim> claims)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("lifetime").Value));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: expiration,
                signingCredentials:signinCredentials
                );
            return token;
        }

        private async Task<IList<Claim> >GetClaims()
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,_user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);

            foreach(var role in roles ){
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private SigningCredentials GetSigninCredentials()
        {
            var key=Environment.GetEnvironmentVariable("KEY");
            var encodedKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This Is My First Key To Be used For Jwt 123456 6879 1123 6"));
            return new SigningCredentials(encodedKey, SecurityAlgorithms.HmacSha256);
        }

        public async Task<bool> ValidateUser(LoginUserDTO userDTO)
        {
            _user = await _userManager.FindByNameAsync(userDTO.Email);

            return (_user != null && await _userManager.CheckPasswordAsync(_user, userDTO.Password));
        }
    }
}
