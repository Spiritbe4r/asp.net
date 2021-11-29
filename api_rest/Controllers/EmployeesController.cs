using api_rest.DTOs;
using api_rest.Interfaces;
using api_rest.Models;
using api_rest.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace api_rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmployeesController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ApplicationDbContext _context;

        private readonly IOptions<EmailOptionsDTO> _emailOptions;
        private readonly IEmail _email;


        public EmployeesController(UserManager<User> userManager,RoleManager<IdentityRole> roleManager, ApplicationDbContext context,
            IOptions<EmailOptionsDTO> emailOptions, IEmail email)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            _context = context;

            _email = email;
       
            _emailOptions = emailOptions;
        }



        // Post api/employers/create
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateEmployeeViewModel model)
        {
            if (!(await _roleManager.RoleExistsAsync("Employer")))
            {
                await _roleManager.CreateAsync(new IdentityRole("Employer"));
            }
            var employer = new User
            {
                UserName = model.Username,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(employer, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            //Send Email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(employer);
            var confirmEmailUrl = Request.Headers["confirmEmailUrl"];  
            //http://localhost:4200/email-confirm

            var uriBuilder = new UriBuilder(confirmEmailUrl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["token"] = token;
            query["userid"] = employer.Id;
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();

            var emailBody = $"Please confirm your email by clicking on the link below </br>{urlString}";
            await _email.Send(model.Email, emailBody, _emailOptions.Value);

            //////////////////

            var userFromDb = await _userManager.FindByNameAsync(employer.UserName);
            await _userManager.AddToRoleAsync(userFromDb, "Employer");

            return Ok(result);
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpGet]
        [Authorize(Roles="Employer")]
        public async Task<IActionResult> getEmployees()
        {
            var values = await _context.Users.ToListAsync();
            return Ok(values);
        }
    }
}
