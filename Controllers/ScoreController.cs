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
    public class ScoreController : Controller
    {
        private readonly ILogger<ScoreController> _logger;

        private readonly QuizGameDbContext _dbContext;

        private readonly IScoreServices _scoreServices;

        public ScoreController(ILogger<ScoreController> logger, QuizGameDbContext testdbContext, IScoreServices scoreServices)
        {
            _logger = logger;
            _dbContext = testdbContext;
            _scoreServices = scoreServices;
        }

        [HttpGet("getScore")]
        public async Task<scoreDTO> GetScore(int quizID)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userID = (from user in _dbContext.Users where user.UserName == userName select user.UserId).FirstOrDefault();

            var scoreDTO = await _scoreServices.GetScore(quizID,userID.Value);

            return scoreDTO;
        }
    }
}
