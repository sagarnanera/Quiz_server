using AutoMapper;
using Quiz_server.DBcontext;
using Quiz_server.DTOs;
using Quiz_server.Helper;
using Quiz_server.Models;
using System.Net;
using System.Text.Json;

namespace Quiz_server.Services
{
    public interface IQuizResponseServices
    {

        Task AddResponses(int userID, int quizID, List<QuizResDTO> quizResDTOs);

    }
    public class QuizResponseServices : IQuizResponseServices
    {

        private readonly IMapper _mapper;


        private readonly QuizGameDbContext _dbContext;

        public QuizResponseServices(IMapper mapper, QuizGameDbContext quizGameDbContext)
        {
            _mapper = mapper;
            _dbContext = quizGameDbContext;
        }

        public async Task AddResponses(int userID, int quizID, List<QuizResDTO> quizResDTOs)
        {

            var uniqueRes = quizResDTOs.GroupBy(q => new { quizID, q.QuestionID, userID })
                                  .Select(g => g.First())
                                  .ToList();

            foreach (var QuizResDTO in uniqueRes)
            {

                var isQuestionExists = _dbContext.Questions.Any(q => q.QuestionId == QuizResDTO.QuestionID && q.QuizId == quizID);

                var isUnique = _dbContext.QuizResponses.Any(r => r.UserID == userID && r.QuestionID == QuizResDTO.QuestionID && r.QuizID == quizID);

                if (!isQuestionExists)
                {
                    throw new KeyNotFoundException("question doesn't exists corresponding to this response.");
                }

                if (isUnique)
                {
                    //throw new AppException("User already answered this question....");
                    throw new AppException($"User already answered question {QuizResDTO.QuestionID} for quiz {quizID}.");
                }

                var userRes = JsonSerializer.Deserialize<Dictionary<string, string>>(QuizResDTO.UserResponse);

                if (userRes.Count != 1)
                {
                    throw new AppException("Answer field must contain only one key-value pair.");
                }

                var Answer = ConvertCorrectAnswerKey(userRes);

                if (Answer["answer"] == null)
                {
                    //Console.WriteLine(Answer["answer"]);

                    throw new AppException("Answer must not be Null.");
                }

                QuizResDTO.UserResponse = JsonSerializer.Serialize(Answer);

            }

            var res = _mapper.Map<List<QuizResponse>>(uniqueRes);

            foreach (var item in res)
            {
                item.UserID = userID;
                item.QuizID = quizID;
            }

            _dbContext.QuizResponses.AddRange(res);

            await _dbContext.SaveChangesAsync();

        }


        //helper functions
        private Dictionary<string, string> ConvertCorrectAnswerKey(Dictionary<string, string> correctAnswer)
        {
            if (correctAnswer.ContainsKey("answer"))
            {
                return correctAnswer;
            }

            var convertedAnswer = new Dictionary<string, string>();

            foreach (var item in correctAnswer)
            {
                convertedAnswer["answer"] = item.Value;
            }

            return convertedAnswer;
        }
    }
}
