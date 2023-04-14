using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Quiz_server.DBcontext;
using Quiz_server.DTOs;
using Quiz_server.Helper;
using Quiz_server.Models;
using System.Text.Json;

namespace Quiz_server.Services
{
    public interface IQuestionServices
    {

        List<questionsResDTO> GetQuestions(int quizID);

        Task InsertQuestions(int quizID, List<questionsReqDTO> questions);

        Task<int> InsertOneQuestion(int quizID, questionsReqDTO question);

        Task UpdateQuestion(int questionID, questionsReqDTO updatedQuestion);

        Task DeleteQuestion(int questionID);

        bool isValidUser(int quizID, int userID);

    }
    public class QuestionServices : IQuestionServices
    {
        private readonly QuizGameDbContext _dbContext;

        private readonly IMapper _mapper;
        public QuestionServices(QuizGameDbContext quizGameDbContext, IMapper mapper)
        {
            _dbContext = quizGameDbContext;
            _mapper = mapper;
        }

        public List<questionsResDTO> GetQuestions(int quizID)
        {

            var questions = (from q in _dbContext.Questions where q.QuizId == quizID select q).ToList();

            var questionList = _mapper.Map<List<questionsResDTO>>(questions);

            if (questionList.Count == 0)
            {
                throw new KeyNotFoundException("Questions not found for this quiz....");
            }

            return questionList;

        }

        public async Task<int> InsertOneQuestion(int quizID, questionsReqDTO question)
        {

            var options = JsonSerializer.Deserialize<Dictionary<string, string>>(question.Options);

            if (options.Count != 4)
            {
                throw new AppException("Options field must contain exactly four options.");
            }

            // Check that the correctAnswer field has an answer within the four options
            var correctAnswer = JsonSerializer.Deserialize<Dictionary<string, string>>(question.CorrectAnswer);

            if (correctAnswer.Count != 1)
            {
                throw new AppException("Correct answer field must contain only one key-value pair.");
            }


            var letteredOptions = ConvertToLetteredKeys(options);
            var Answer = ConvertCorrectAnswerKey(correctAnswer);

            if (Answer["answer"] == null || !options.Values.Contains(Answer["answer"].ToString()))
            {
                //Console.WriteLine(Answer["answer"]);

                throw new AppException("Correct answer must be one of the options.");
            }

            question.Options = JsonSerializer.Serialize(letteredOptions);
            question.CorrectAnswer = JsonSerializer.Serialize(Answer);

            var questionEntity = _mapper.Map<Question>(question);

            questionEntity.QuizId = quizID;

            _dbContext.Questions.Add(questionEntity);

            await _dbContext.SaveChangesAsync();

            return questionEntity.QuestionId;

        }

        public async Task InsertQuestions(int quizID, List<questionsReqDTO> questions)
        {
            foreach (var question in questions)
            {
                // Check that the options field has four options
                var options = JsonSerializer.Deserialize<Dictionary<string, string>>(question.Options);

                if (options.Count != 4)
                {
                    throw new AppException("Options field must contain exactly four options.");
                }

                // Check that the correctAnswer field has an answer within the four options
                var correctAnswer = JsonSerializer.Deserialize<Dictionary<string, string>>(question.CorrectAnswer);

                if (correctAnswer.Count != 1)
                {
                    throw new AppException("Correct answer field must contain only one key-value pair.");
                }


                var letteredOptions = ConvertToLetteredKeys(options);
                var Answer = ConvertCorrectAnswerKey(correctAnswer);

                if (Answer["answer"] == null || !options.Values.Contains(Answer["answer"].ToString()))
                {
                    //Console.WriteLine(Answer["answer"]);

                    throw new AppException("Correct answer must be one of the options.");
                }

                question.Options = JsonSerializer.Serialize(letteredOptions);
                question.CorrectAnswer = JsonSerializer.Serialize(Answer);
            }

            var questionEntity = _mapper.Map<List<Question>>(questions);

            foreach (Question q in questionEntity)
            {
                q.QuizId = quizID;
            }

            _dbContext.Questions.AddRange(questionEntity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateQuestion(int questionID, questionsReqDTO updatedQuestion)
        {
            //var entity = _mapper.Map<Question>(updatedQuestion);

            //entity.QuestionId = questionID;


            ////_dbContext.Entry(entity).State = EntityState.Detached;

            //_dbContext.Questions.Update(entity);

            //await _dbContext.SaveChangesAsync();

            var entity = await _dbContext.Questions.FindAsync(questionID);

            if (entity == null)
            {
                throw new Exception($"Question with ID {questionID} not found.");
            }

            updatedQuestion.QuizId = entity.QuizId;

            _mapper.Map(updatedQuestion, entity);

            await _dbContext.SaveChangesAsync();
        }


        public async Task DeleteQuestion(int questionID)
        {

            var entity = _dbContext.Questions.FirstOrDefault(q => q.QuestionId == questionID);

            _dbContext.Questions.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        // helper functions...

        public bool isValidUser(int quizID, int userID)
        {
            var isValidUser = _dbContext.Quizzes.Any(quiz => quiz.QuizId == quizID && quiz.UserId == userID);

            if (!isValidUser)
            {
                throw new AppException("quiz not found... ");
            }

            return true;
        }

        private Dictionary<string, string> ConvertToLetteredKeys(Dictionary<string, string> options)
        {
            Dictionary<string, string> letteredOptions = new Dictionary<string, string>();
            List<string> keys = new List<string> { "A", "B", "C", "D" };

            for (int i = 0; i < keys.Count && i < options.Count; i++)
            {
                string key = keys[i];
                string value = options.ElementAt(i).Value;

                letteredOptions[key] = value;
            }

            return letteredOptions;
        }

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
