using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz_server.DBcontext;
using Quiz_server.DTOs;
using Quiz_server.Models;
using Quiz_server.Services;
using System.Net;
using System.Security.Claims;

namespace Quiz_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuizController : Controller
    {
        private readonly ILogger<QuizController> _logger;

        private readonly QuizGameDbContext _dbContext;

        private readonly IQuizServices _quizServices;
        public QuizController(ILogger<QuizController> logger, QuizGameDbContext testdbContext, IQuizServices quizServices)
        {
            _logger = logger;
            _dbContext = testdbContext;
            _quizServices = quizServices;
        }

        [HttpGet("GetQuizs")]
        public ActionResult<List<QuizDTO>> GetQuizzes()
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userID = (from user in _dbContext.Users where user.UserName == userName select user.UserId).FirstOrDefault();

            //var quizzes = (from quiz in _dbContext.Quizzes where quiz.UserId == userID select quiz).ToList();
            var quizzes = new List<QuizDTO>();


            quizzes = _quizServices.GetQuizs(userID.Value);


            //if (quizzes.Count == 0)
            //{
            //    return NotFound(new {error = "No quizes found for this user..."});
            //}

            return Ok(quizzes);
        }

        [HttpGet("GetQuiz/{quizID}")]
        public ActionResult<QuizDTO> GetQuiz(int quizID)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userID = (from user in _dbContext.Users where user.UserName == userName select user.UserId).FirstOrDefault();

            var quiz = _quizServices.GetQuiz(quizID, userID.Value);


            //var quizze = (from quiz in _dbContext.Quizzes where quiz.QuizId == quizID && quiz.UserId == userID select quiz).FirstOrDefault();

            //if (quizze == null)
            //{
            //    return NotFound(new { error = "Quiz not found..." });
            //}

            //var quizDTO = _mapper.Map<QuizDTO>(quizze);



            return Ok(quiz);
        }

        [HttpPost("CreatQuiz")]
        public async Task<ActionResult<HttpStatusCode>> CreateQuiz(QuizDTO quiz)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userID = (from user in _dbContext.Users where user.UserName == userName select user.UserId).FirstOrDefault();

            var quizID = await _quizServices.InsertQuiz(userID.Value, quiz);

            //var entity = new Quiz
            //{
            //    CreatedOn = DateTime.Now,
            //    EndDate = quiz.EndDate,
            //    UserId = userID.Value,
            //    QuizName = quiz.QuizName,
            //    IsActive = quiz.IsActive,
            //    StartDate = quiz.StartDate
            //};

            //_dbContext.Quizzes.Add(entity);
            //await _dbContext.SaveChangesAsync();

            return Ok(new { res = "quiz created.....", quizID });

        }

        [HttpPost("UpdateQuiz/{quizID}")]
        public async Task<ActionResult<HttpStatusCode>> UpdateQuiz(int quizID, QuizDTO quiz)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userID = (from user in _dbContext.Users where user.UserName == userName select user.UserId).FirstOrDefault();

            await _quizServices.UpdateQuiz(quizID, userID.Value, quiz);

            return Ok(new { res = "quiz updated succesfully." });

        }

        [HttpPost("DeleteQuiz")]
        public async Task<ActionResult<HttpStatusCode>> DeleteQuiz(int quizID)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userID = (from user in _dbContext.Users where user.UserName == userName select user.UserId).FirstOrDefault();

            await _quizServices.DeleteQuiz(quizID, userID.Value);

            return Ok(new { res = "quiz deleted succesfully." });

        }
    }
}
