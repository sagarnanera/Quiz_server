namespace Quiz_server.DTOs
{
    public class questionsResDTO
    {
        public int QuestionId { get; set; }
        public string QuestionString { get; set; } = null!;

        public int QuestionType { get; set; }

        public string Options { get; set; } = null!;

        public float Points { get; set; }

        public int QuizId { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
