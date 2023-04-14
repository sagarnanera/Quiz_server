using Quiz_server.Models;

namespace Quiz_server.DTOs
{
    public class questionsReqDTO
    {
        public string QuestionString { get; set; } = null!;

        public int QuestionType { get; set; }

        public string Options { get; set; } = null!;

        public string CorrectAnswer { get; set; }
        public float Points { get; set; }

        public int QuizId { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
