using System;
using System.Collections.Generic;

namespace Quiz_server.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public string QuestionString { get; set; } = null!;

    public int QuestionType { get; set; }

    public string Options { get; set; } = null!;

    public string CorrectAnswer { get; set; }

    public float Points { get; set; }

    public int QuizId { get; set; }

    public DateTime CreatedOn { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;

    public virtual ICollection<QuizResponse> QuizResponses { get; } = new List<QuizResponse>();
}
