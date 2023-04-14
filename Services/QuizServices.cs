using AutoMapper;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Quiz_server.DBcontext;
using Quiz_server.DTOs;
using Quiz_server.Helper;
using Quiz_server.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Quiz_server.Services
{

    public interface IQuizServices
    {

        List<QuizDTO> GetQuizs(int userID);

        QuizDTO GetQuiz(int quizID, int userID);
        Task<int> InsertQuiz(int userID, QuizDTO quizDTO);

        Task UpdateQuiz(int quizID, int userID, QuizDTO quizDTO);

        Task DeleteQuiz(int quizID, int userID);

    }
    public class QuizServices : IQuizServices
    {
        private readonly QuizGameDbContext _dbContext;

        private readonly IMapper _mapper;

        public QuizServices(QuizGameDbContext quizGameDbContext, IMapper mapper)
        {
            _dbContext = quizGameDbContext;
            _mapper = mapper;
        }

        public async Task DeleteQuiz(int quizID, int userID)
        {
            var quiz = _dbContext.Quizzes.FirstOrDefault(q => q.QuizId == quizID && q.UserId == userID);

            if (quiz == null) throw new KeyNotFoundException($"Quiz with ID {quizID} not found.");

            _dbContext.Quizzes.Remove(quiz);

            await _dbContext.SaveChangesAsync();
        }

        public QuizDTO GetQuiz(int quizID, int userID)
        {
            var quiz = _dbContext.Quizzes.FirstOrDefault(q => q.QuizId == quizID && q.UserId == userID);

            if (quiz == null) throw new KeyNotFoundException($"Quiz with ID {quizID} not found.");

            var quizDTo = _mapper.Map<QuizDTO>(quiz);

            return quizDTo;

        }

        public List<QuizDTO> GetQuizs(int userID)
        {

            var quizzes = _dbContext.Quizzes.Where(q => q.UserId == userID).ToList();

            if (quizzes.Count == 0) throw new KeyNotFoundException("Quizs not found for this user.");

            var quizList = _mapper.Map<List<QuizDTO>>(quizzes);

            return quizList;

        }

        public async Task<int> InsertQuiz(int userID, QuizDTO quizDTO)
        {
            var quiz = _mapper.Map<Quiz>(quizDTO);

            quiz.UserId = userID;

            _dbContext.Quizzes.Add(quiz);

            await _dbContext.SaveChangesAsync();

            return quiz.QuizId;
        }

        public async Task UpdateQuiz(int quizID, int userID, QuizDTO quizDTO)
        {
            var isValidUser = _dbContext.Quizzes.Any(quiz => quiz.QuizId == quizID && quiz.UserId == userID);


            if (!isValidUser)
            {
                throw new AppException("No quiz found.... ");
            }

            var quiz = _mapper.Map<Quiz>(quizDTO);



            quiz.QuizId = quizID;
            quiz.UserId=userID;

            //Console.WriteLine(JsonSerializer.Serialize(quiz));

            _dbContext.Quizzes.Update(quiz);

            await _dbContext.SaveChangesAsync();
        }
    }
}
