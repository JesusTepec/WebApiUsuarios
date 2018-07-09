using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiUsuarios.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace WebApiUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly ApplicationDbContext _context;


        public AccountController(ApplicationDbContext context)
        {
            _context = context;

            if (_context.Usuarios.Count() == 0)
            {
                _context.Usuarios.Add(new Usuario { Nombre = "Jesus", UserName = "tepec", Password = "1234" });
                _context.SaveChanges();
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Usuario userInfo)
        {
            if (ModelState.IsValid)
            {
                var usuario = _context.Usuarios.Where(x => x.UserName.Equals( userInfo.UserName)).First();

                if (usuario.Password.Equals(userInfo.Password))
                {
                    return BuildToken(userInfo);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private IActionResult BuildToken(Usuario userInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SFMyNTY.U21WemRYTlVaWEJsWXc9PSMjT0RRMw.QmyRK69Z6XaKNynEE7iy1fRxxx3qZuaVSpWeRnd21mI"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "jesustepec.com",
               audience: "jesustepec.com",
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = expiration
            });

        }
    }
}