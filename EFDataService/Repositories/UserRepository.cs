using System;
using System.IdentityModel.Tokens.Jwt;
using EFCore;
using EFCore.Entities;
using EFDataService.Repositories.Interfaces;
using System.Linq;
using System.Security.Claims;
using System.Text;
using EFDataService.Helper;
using EFDataService.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace EFDataService.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected readonly EFCoreContext Context;
        private readonly EfCoreSettings efCoreSettings;
        public UserRepository(EFCoreContext context, IOptions<EfCoreSettings> options)
        {
            this.Context = context;
            this.efCoreSettings = options.Value;
        }
        public User Get(string email)
        {
            return Context.Users.FirstOrDefault(m => m.Email.Equals(email));
        }
        public UserDTO Login(LoginArgument arg)
        {
            var user = Context.Users.FirstOrDefault(m => m.Email.Equals(arg.Email));
            if (user == null)
            {
                return null;
            }

            var encryptedPass = SecurityHandler.EncryptString(SecurityHandler.DefaultKey, arg.Password);
            if (user.Password.Equals(encryptedPass))
            {
                return new UserDTO
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    Fullname = user.Fullname,
                    Token = GenerateToken(user)
                };
            }
            return null;
        }
        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(efCoreSettings.JWTSecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Fullname),
                    new Claim(ClaimTypes.UserData, user.Id.ToString()),
                    new Claim("PromoCodes", JsonConvert.SerializeObject(user.PromoCodes))
                }),
                Expires = DateTime.UtcNow.AddHours(efCoreSettings.JWTTokenExpiryTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
