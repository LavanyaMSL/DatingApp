using API.Data;
using API.DTOs;
using API.Entitites;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService service;
        public AccountController(DataContext context,ITokenService tokenService)
        { 
            _context = context;
            service = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("UserName is taken");
            
                using var hmac = new HMACSHA512();

                var user = new AppUser
                {
                    UserName = registerDto.Username,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hmac.Key

                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            
            return new UserDto
            { 
            Username=user.UserName,
            Token=service.CreateToken(user)
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            
            var user = await _context.Users.SingleOrDefaultAsync(s => s.UserName == loginDto.Username);
            if (user == null) return Unauthorized("Invalid UserName");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computecash=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computecash.Length; i++)
            {
                if (computecash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return new UserDto
            {
                Username = user.UserName,
                Token = service.CreateToken(user)
            }; ;

        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(t => t.UserName.Equals(username,StringComparison.OrdinalIgnoreCase));
           // ToLower() == username.ToLower());
        }
    }
}
