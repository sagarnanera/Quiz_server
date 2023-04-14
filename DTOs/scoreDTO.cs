namespace Quiz_server.DTOs
{
    public class scoreDTO
    {
        //public int ScoreId { get; set; }
        public int QuizId { get; set; }

        public float userScore { get; set; }

        //public int Attempts { get; set; }

        public float totalScore { get; set; }
    }
}
