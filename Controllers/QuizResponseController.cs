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
    [Authorize]
    public class QuizResponseController : Controller
    {
        private readonly ILogger<QuizResponseController> _logger;

        private readonly QuizGameDbContext _dbContext;

        private readonly IQuizResponseServices _quizResponseServices;
        public QuizResponseController(ILogger<QuizResponseController> logger, QuizGameDbContext testdbContext, IQuizResponseServices quizResponseServices)
        {
            _logger = logger;
            _dbContext = testdbContext;
            _quizResponseServices = quizResponseServices;
        }

        [HttpPost("AddResponses/{quizID}")]
        public async Task<ActionResult<HttpStatusCode>> AddResponse(int quizID, List<QuizResDTO> quizResDTOs)
        {

            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userID = (from user in _dbContext.Users where user.UserName == userName select user.UserId).FirstOrDefault();

            await _quizResponseServices.AddResponses(userID.Value, quizID, quizResDTOs);

            //Console.WriteLine(JsonSerializer.Serialize(resID));

            return Ok(new { res = "Responses added succesfully....." });

        }
    }
}
