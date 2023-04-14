using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz_server.DBcontext;
using Quiz_server.DTOs;
using Quiz_server.Services;
using System.Net;
using System.Security.Claims;

namespace Quiz_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        private readonly QuizGameDbContext _dbContext;

        private IUserServices _userServices;
        public UserController(ILogger<UserController> logger, QuizGameDbContext testdbContext, IUserServices userServices)
        {
            _logger = logger;
            _dbContext = testdbContext;
            _userServices = userServices;
        }

        //public static User user = new User();


        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(UserRegisterModel request)
        {

            //var user = (from u in _dbContext.Users where u.Name == request.UserName select u).FirstOrDefault();



            //if (user != null)
            //{
            //    return BadRequest("User with this userName already exists..");
            //}

            //string passwordHash
            //   = BCrypt.Net.BCrypt.HashPassword(request.Password);

            //var entity = new User
            //{
            //    Time = DateTime.Now,
            //    Name = request.UserName,
            //    Password = passwordHash
            //};

            //_dbContext.Users.Add(entity);
            //await _dbContext.SaveChangesAsync();

            await _userServices.RegisterUser(request);

            return Ok(new { msg = "User registration successful." });

        }

        [HttpPost("login")]
        public ActionResult<string> Login(UserloginModel request)
        {
            //if (request == null || request.password == null || request.userName == null)
            //{
            //    return BadRequest("Invalid client request");
            //}
            //else
            //{
            //    var user = (from u in _dbContext.Users
            //                where u.UserName == request.userName
            //                select new
            //                {
            //                    id = u.UserId,
            //                    name = u.UserName,
            //                    password = u.Password
            //                }).FirstOrDefault();

            //    if (user == null)
            //    {
            //        return BadRequest("userName or password invalid...");
            //    }

            //    if (!BCrypt.Net.BCrypt.Verify(request.password, user.password))
            //    {
            //        return BadRequest("Wrong password.");
            //    }

            //    string token = _userServices.CreateToken(user.name, user.id);

            //var response = new UserDTO
            //{
            //    UserName = user.name,
            //    token = token
            //};

            var response = _userServices.Login(request);

            return Ok(response);
            //}



        }


        [HttpGet("GetUserInfo"), Authorize]
        public ActionResult<UserDTO> GetUserInfo()
        {

            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            //var user = from u in _dbContext.Users where u.UserName == username select u;

            var user = _userServices.GetUser(username);

            return Ok(user);
        }

        [HttpGet("DeleteUser"), Authorize]
        public ActionResult<UserDTO> DeleteUser()
        {

            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            //var user = from u in _dbContext.Users where u.UserName == username select u;

            _userServices.DeleteUser(username);

            return Ok(new { msg = "User deletaion successful." });
        }

        [HttpGet("LogOut"), Authorize]
        public ActionResult<HttpStatusCode> LogOut()
        {

            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            //var user = from u in _dbContext.Users where u.UserName == username select u;

            //HttpContext.SignOutAsync().Wait();

            _userServices.LogOut();

            return Ok(new {msg = "Loggedout succesfully."});
        }
    }
}

