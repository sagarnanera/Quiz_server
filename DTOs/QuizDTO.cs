namespace Quiz_server.DTOs
{
    public class QuizDTO
    {
        public int QuizId { get; set; }

        public string QuizName { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public int UserId { get; set; }

        public bool IsActive { get; set; }
    }
}
