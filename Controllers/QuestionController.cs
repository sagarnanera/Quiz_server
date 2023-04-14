using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class QuestionController : Controller
    {

        private readonly ILogger<QuestionController> _logger;

        private readonly QuizGameDbContext _dbContext;

        private readonly IMapper _mapper;

        private readonly IQuestionServices _questionServices;
        public QuestionController(ILogger<QuestionController> logger, QuizGameDbContext testdbContext, IMapper mapper,IQuestionServices questionServices)
        {
            _logger = logger;
            _dbContext = testdbContext;
            _mapper = mapper;
            _questionServices = questionServices;
        }

        
        [HttpGet("GetQuestions/{quizID}")]
        public ActionResult<List<questionsResDTO>> GetQuestions(int quizID)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userID = (from user in _dbContext.Users where user.UserName == userName select user.UserId).FirstOrDefault();

            //_questionServices.isValidUser(quizID,userID.Value);

            var questionList = _questionServices.GetQuestions(quizID);

            //bool isValidUser = _dbContext.Quizzes.Any(quiz => quiz.QuizId == quizID && quiz.UserId == userID);

            //if (!isValidUser)
            //{
            //    return BadRequest(new { error = "No questions found with the given quiz...." });
            //}

            //var questionList = (from questions in _dbContext.Questions where questions.QuizId == quizID select questions).ToList();

            //if (questionList.Count == 0)
            //{
            //    return NotFound(new { error = "Questions not found for this quiz...." });
            //}

            //var questionDTO = _mapper.Map<List<questionsDTO>>(questionList);

            return Ok(questionList);
        }

        [HttpPost("InsertQuestions/{quizID}")]
        public async Task<ActionResult<HttpStatusCode>> InsertQuestions(int quizID, List<questionsReqDTO> questions)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userID = (from user in _dbContext.Users where user.UserName == userName select user.UserId).FirstOrDefault();

            _questionServices.isValidUser(quizID, userID.Value);

            //if (!isValidUser)
            //{
            //    return BadRequest(new { error = "No quiz found with the given ID...." });
            //}

            await _questionServices.InsertQuestions(quizID,questions);

            //var questionEntity = _mapper.Map<List<Question>>(questions);

            //_dbContext.Questions.AddRange(questionEntity);

            //await _dbContext.SaveChangesAsync();

            return Ok(new { msg = "questions inserted........" });
        }


        [HttpPost("InsertOneQuestion/{quizID}")]
        public async Task<ActionResult<HttpStatusCode>> InsertOneQuestion(int quizID, questionsReqDTO questions)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userID = (from user in _dbContext.Users where user.UserName == userName select user.UserId).FirstOrDefault();

            _questionServices.isValidUser(quizID, userID.Value);

            //if (!isValidUser)
            //{
            //    return BadRequest(new { error = "No quiz found with the given ID...." });
            //}

            //var questionEntity = _mapper.Map<Question>(questions);

            //questionEntity.QuizId = quizID;

            //_dbContext.Questions.Add(questionEntity);

            //await _dbContext.SaveChangesAsync();

            var questionId = await _questionServices.InsertOneQuestion(quizID,questions);

            return Ok(new { msg = "question inserted........",questionId });
        }

        [HttpPost("UpdateQuestion/{questionID}")]
        public async Task<ActionResult<HttpStatusCode>> UpdateQuestion(int questionID, questionsReqDTO updatedQuestion)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userID = (from user in _dbContext.Users where user.UserName == userName select user.UserId).FirstOrDefault();

            var question = _dbContext.Questions.FirstOrDefault(question => question.QuestionId == questionID);

            _questionServices.isValidUser(question.QuizId, userID.Value);


            //if (!isValidUser)
            //{
            //    return BadRequest(new { error = "No question found with given ID..." });
            //}



            //var entity = _mapper.Map<Question>(updatedQuestion);

            //entity.QuestionId = questionID;


            //_dbContext.Entry(entity).State = EntityState.Detached;

            // TODO : make services for each controller 


            // _dbContext.Questions.Update(entity);

            //await _dbContext.SaveChangesAsync();

            await _questionServices.UpdateQuestion(questionID,updatedQuestion);

            return Ok(new { msg = $"Question updated successfully" });
        }

        [HttpPost("DeleteQuestion/{questionID}")]
        public async Task<ActionResult<HttpStatusCode>> DeleteQuestion(int questionID)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userID = (from user in _dbContext.Users where user.UserName == userName select user.UserId).FirstOrDefault();

            var question = _dbContext.Questions.FirstOrDefault(question => question.QuestionId == questionID);

            _questionServices.isValidUser(question.QuizId, userID.Value);


            await _questionServices.DeleteQuestion(questionID);

            return Ok(new { msg = $"Question deleted successfully" });
        }


    }
}
