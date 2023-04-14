using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Quiz_server.Models;

public partial class User
{
    public string? UserName { get; set; } = null!;

    public int? UserId { get; set; }
    [JsonIgnore]
    public string? Password { get; set; }

    public DateTime? Time { get; set; }

    public virtual ICollection<Quiz> Quizzes { get; } = new List<Quiz>();

    public virtual ICollection<Score> Scores { get; } = new List<Score>();

    public virtual ICollection<QuizResponse> QuizResponses { get; }= new List<QuizResponse>();
}
