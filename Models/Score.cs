using System;
using System.Collections.Generic;

namespace Quiz_server.Models;

public partial class Score
{
    public int ScoreId { get; set; }

    public int UserId { get; set; }

    public int QuizId { get; set; }

    public float userScore { get; set; }

    public int Attempts { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
