using IdentityService.Api.Models;
using IdentityService.Api.Services.Abstract;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace IdentityService.Api.Services.Concrete
{
    public class IdentityService : IIdentityService
    {
        public Task<LoginResponseModel> Login(LoginRequestModel requestModel)
        {
            // DB Process will be here. Check if user information is valid and get details

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, requestModel.UserName),
                new Claim(ClaimTypes.Name, "Yunus Emre Koşar"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("cokgizlisifrecokfenayanicokgizlisifrecokfenayani"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(10);

            var token = new JwtSecurityToken(claims: claims, expires: expiry, signingCredentials: creds, notBefore: DateTime.Now);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            LoginResponseModel response = new()
            {
                Token = encodedJwt,
                UserName = requestModel.UserName
            };

            return Task.FromResult(response);
        }
    }
}