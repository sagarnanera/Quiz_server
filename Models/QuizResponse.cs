using System.ComponentModel.DataAnnotations;

namespace Quiz_server.Models
{
    public class QuizResponse
    {
        public int responseID { get; set; }
        public int QuizID { get; set; }
        public int QuestionID { get; set; }
        public int UserID { get; set; }
        public string UserResponse { get; set; }

        public DateTime ResponseTime { get; set; }
        public virtual Quiz Quiz { get; set; } = null!;

        public virtual User User { get; set; } = null!;

        public virtual Question Question { get; set; } = null!;
    }

}
