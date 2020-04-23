using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using JWT_API_NETCORE.Models;
using JWT_API_NETCORE.Repository.Data;
using JWT_API_NETCORE.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWT_API_NETCORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        DynamicParameters parameters = new DynamicParameters();

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, EmployeeRepository employeeRepository, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _employeeRepository = employeeRepository;
            _roleManager = roleManager;

        }

        // api/auth/register
        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> InsertUser([FromBody] RegisterViewModel model)
        {

            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            
            var result = await _userManager.CreateAsync(user, model.Password);
            
            
            
            if (result.Succeeded)
            {

                // insert new employee
                await _employeeRepository.Create(model);

                // add role
                await _userManager.AddToRoleAsync(user, "Employee");
            }
            return Ok(new { Email = user.Email });
        }

        [Route("login")] // api/auth/login
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);

            // create sp to get role name
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var procName = "SP_GetRole";
                parameters.Add("@Email", user.Email);
                IEnumerable<LoginViewModel> data = connection.Query<LoginViewModel>(procName, parameters, commandType: CommandType.StoredProcedure);
                foreach (LoginViewModel users in data)
                {
                    model.Role = users.Role;
                }
            }


            // check login
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                

                var claim = new[] {
                    new Claim("Email", user.Email),
                    new Claim("Role", model.Role)
                };

                var signinKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

                int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                  issuer: _configuration["Jwt:Site"],
                  audience: _configuration["Jwt:Site"],
                  claim,
                  expires: DateTime.UtcNow.AddDays(1),
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                //return Ok(
                //  new
                //  {
                //      token = new JwtSecurityTokenHandler().WriteToken(token),
                //      expiration = token.ValidTo
                //  });
            }

            return Unauthorized();
        }
    }
}