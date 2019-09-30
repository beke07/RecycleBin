using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RecycleBin.DAL.Constants;
using RecycleBin.Helpers;
using RecycleBin.Model;
using RecycleBin.ViewModels;
using RecycleBin.ViewModels.User;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecycleBin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AppSettings appSettings;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<AppSettings> appSettings)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.appSettings = appSettings.Value;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Guid>> Post([FromBody] CreateViewModel createViewModel)
        {
            var user = new ApplicationUser()
            {
                Email = createViewModel.Email,
                UserName = createViewModel.UserName
            };

            var result = await userManager.CreateAsync(user, createViewModel.Password);
                         await userManager.AddToRoleAsync(user, RoleNames.Customer);

            if (result.Succeeded)
            {
                return Ok(user.Id);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Post([FromBody] LoginViewModel loginViewModel)
        {
            var user = await userManager.FindByNameAsync(loginViewModel.UserName);

            if (user == null || !(await userManager.CheckPasswordAsync(user, loginViewModel.Password)))
            {
                return Unauthorized("Invalid username or password!");
            }

            var roles = await userManager.GetRolesAsync(user);
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Secret));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDiscriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(JwtRegisteredClaimNames.Sub, loginViewModel.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                    new Claim("LoggedInDate", DateTime.Now.ToString())
                }),
                
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = appSettings.Site,
                Audience = appSettings.Audience,
                Expires = DateTime.Now.AddMinutes(Convert.ToDouble(appSettings.ExpireTime))  
            };

            var token = tokenHandler.CreateToken(tokenDiscriptor);

            return Ok(new { token = tokenHandler.WriteToken(token), expiration = token.ValidTo, userName = user.UserName, userRole = roles.FirstOrDefault()});
        }
    }
}
