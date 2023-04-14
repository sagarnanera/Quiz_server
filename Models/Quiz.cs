using System;
using System.Collections.Generic;

namespace Quiz_server.Models;

public partial class Quiz
{
    public int QuizId { get; set; }

    public string QuizName { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime CreatedOn { get; set; }

    public int UserId { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Question> Questions { get; } = new List<Question>();

    public virtual ICollection<Score> Scores { get; } = new List<Score>();

    public virtual ICollection<QuizResponse> QuizResponses { get; } = new List<QuizResponse>();

    public virtual User User { get; set; } = null!;
}
