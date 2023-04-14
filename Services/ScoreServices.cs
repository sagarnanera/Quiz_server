using AutoMapper;
using Quiz_server.DBcontext;
using Quiz_server.DTOs;
using Quiz_server.Helper;
using Quiz_server.Models;
using System.Text.Json;

namespace Quiz_server.Services
{
    public interface IScoreServices
    {

        Task<scoreDTO> GetScore(int quizID, int userID);


    }
    public class ScoreServices : IScoreServices
    {

        private readonly QuizGameDbContext _dbContext;

        private readonly IMapper _mapper;


        public ScoreServices(QuizGameDbContext quizGameDbContext, IMapper mapper)
        {
            _dbContext = quizGameDbContext;
            _mapper = mapper;
        }

        public async Task<scoreDTO> GetScore(int quizID, int userID)
        {
            var scoreDTO = new scoreDTO();

            var isExists = _dbContext.Scores.Any(s => s.UserId == userID && s.QuizId == quizID);

            float totalScore = _dbContext.Questions.Where(q => q.QuizId == quizID).Sum(q => q.Points);

            if (isExists)
            {

                var score = _dbContext.Scores.FirstOrDefault(s => s.UserId == userID && s.QuizId == quizID);

                scoreDTO = _mapper.Map<scoreDTO>(score);

                scoreDTO.totalScore = totalScore;

                return scoreDTO;

            }
            else
            {
                var responses = _dbContext.QuizResponses
                              .Where(r => r.QuizID == quizID && r.UserID == userID)
                              .ToList();

                //Console.WriteLine(JsonSerializer.Serialize(responses));

                if (responses.Count == 0)
                {
                    throw new AppException($"User has not attempted quiz {quizID} yet");
                }

                float userScore = 0;

                // Loop through each response and check if it is correct
                foreach (var response in responses)
                {
                    // Retrieve the corresponding question
                    var question = _dbContext.Questions
                                             .Where(q => q.QuestionId == response.QuestionID)
                                             .FirstOrDefault();

                    var correctAnswer = JsonSerializer.Deserialize<Dictionary<string, string>>(question.CorrectAnswer);
                    var userAnswer = JsonSerializer.Deserialize<Dictionary<string, string>>(response.UserResponse);

                    // Check if the user's response is correct and add points to the total if it is
                    if (correctAnswer["answer"] == userAnswer["answer"])
                    {
                        userScore += question.Points;
                    }
                }

                scoreDTO.totalScore = totalScore;
                scoreDTO.userScore = userScore;
                scoreDTO.QuizId = quizID;

                var score = _mapper.Map<Score>(scoreDTO);

                score.UserId = userID;

                _dbContext.Scores.Add(score);

                await _dbContext.SaveChangesAsync();

                return scoreDTO;
            }

        }
    }
}
