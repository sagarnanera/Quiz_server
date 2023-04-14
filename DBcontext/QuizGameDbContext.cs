using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Quiz_server.Models;

namespace Quiz_server.DBcontext;

public partial class QuizGameDbContext : DbContext
{
    public QuizGameDbContext()
    {
    }

    public QuizGameDbContext(DbContextOptions<QuizGameDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<Score> Scores { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<QuizResponse> QuizResponses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseMySql("server=localhost;database=quiz_game_db;uid=root;pwd=Mysqlpassword", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.24-mariadb"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PRIMARY");

            entity.ToTable("Questions");

            entity.HasIndex(e => e.QuizId, "quizID");

            entity.Property(e => e.QuestionId)
                .HasColumnType("int(5)")
                .HasColumnName("questionID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("createdOn");
            entity.Property(e => e.Options)
                .HasColumnType("json")
                .HasColumnName("options");
            entity.Property(e => e.Points)
                .HasDefaultValueSql("'1'")
                .HasColumnName("points");
            entity.Property(e => e.QuestionString)
                .HasMaxLength(500)
                .HasColumnName("questionString");
            entity.Property(e => e.QuestionType)
                .HasColumnType("int(2)")
                .HasColumnName("questionType");
            entity.Property(e => e.QuizId)
                .HasColumnType("int(11)")
                .HasColumnName("quizID");

            entity.Property(e => e.CorrectAnswer)
                .HasColumnType("json")
                .HasColumnName("correctAnswer");

            entity.HasOne(d => d.Quiz).WithMany(p => p.Questions)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("fk_quiz_question_id");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.QuizId).HasName("PRIMARY");

            entity.ToTable("Quizzes");

            entity.HasIndex(e => e.UserId, "userID");

            entity.Property(e => e.QuizId)
                .HasColumnType("int(11)")
                .HasColumnName("quizID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("createdOn");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.QuizName)
                .HasMaxLength(20)
                .HasColumnName("quizName");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("startDate");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.Quizzes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_quiz_id");
        });

        modelBuilder.Entity<Score>(entity =>
        {
            entity.HasKey(e => e.ScoreId).HasName("PRIMARY");

            entity.ToTable("Scores");

            entity.HasIndex(e => e.QuizId, "quizId");

            entity.HasIndex(e => e.UserId, "userId");

            entity.Property(e => e.ScoreId)
                .HasColumnType("int(11)")
                .HasColumnName("scoreID");
            entity.Property(e => e.Attempts)
                .HasColumnType("int(11)")
                .HasColumnName("attempts");
            entity.Property(e => e.QuizId)
                .HasColumnType("int(11)")
                .HasColumnName("quizID");
            entity.Property(e => e.userScore).HasColumnName("Score");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("userId");

            entity.HasOne(d => d.Quiz).WithMany(p => p.Scores)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("fk_quiz_score_id");

            entity.HasOne(d => d.User).WithMany(p => p.Scores)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_score_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("Users");

            entity.HasIndex(e => e.UserName, "UserName").IsUnique();

            entity.Property(e => e.UserId)
                .HasColumnType("int(8)")
                .HasColumnName("userId");
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .HasColumnName("UserName");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Time)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("time");
        });


        //modelBuilder.Entity<QuizResponse>(entity =>
        //{
        //    entity.HasKey(e => e.resID).HasName("PRIMARY");
        //    entity.ToTable("QuizResponse");

        //    entity.HasIndex(e => e.userId, "userId");
        //    entity.HasIndex(e=> e.quizId,"quizId");

        //    entity.Property(e => e.resID)
        //        .HasColumnType("int(11)")
        //        .HasColumnName("resID");
        //});



        //modelBuilder.Entity<QuizResponse>(entity =>
        //{
        //    entity.HasKey(e => e.ResID).HasName("PRIMARY");

        //    entity.ToTable("QuizResponse");

        //    entity.HasIndex(e => e.UserId, "userId");

        //    entity.HasIndex(e => e.QuizId, "quizId");

        //    entity.Property(e => e.ResID)
        //        .HasColumnType("int(11)")
        //        .HasColumnName("resID");

        //    entity.Property(e => e.UserResponse)
        //          .HasColumnType("json")
        //          .HasColumnName("userResponse");

        //    entity.Property(e => e.QuizId)
        //       .HasColumnType("int(11)")
        //       .HasColumnName("quizID");

        //    entity.Property(e => e.UserId)
        //        .HasColumnType("int(11)")
        //        .HasColumnName("userId");

        //    // add foreign key constraints
        //    entity.HasOne(d => d.Quiz).WithMany(p => p.Responses)
        //        .HasForeignKey(d => d.QuizId)
        //        .HasConstraintName("fk_quiz_response_id");

        //    entity.HasOne(d => d.User).WithMany(p => p.Responses)
        //        .HasForeignKey(d => d.UserId)
        //        .HasConstraintName("fk_user_response_id");
        //});

        modelBuilder.Entity<QuizResponse>(entity =>
        {
            entity.HasKey(e => e.responseID).HasName("PRIMARY");

            entity.ToTable("QuizResponse");

            entity.HasIndex(e => e.UserID, "userId");

            entity.HasIndex(e => e.QuizID, "quizId");

            entity.HasIndex(e => e.QuestionID, "questionId");

            entity.Property(e => e.responseID)
                .HasColumnType("int(11)")
                .HasColumnName("resID");

            entity.Property(e => e.UserResponse)
                .HasColumnType("json")
                .HasColumnName("userResponse");

            entity.Property(e => e.QuizID)
                .HasColumnType("int(11)")
                .HasColumnName("quizID");

            entity.Property(e => e.UserID)
                .HasColumnType("int(11)")
                .HasColumnName("userId");

            entity.Property(e => e.QuestionID)
                .HasColumnType("int(11)")
                .HasColumnName("questionID");

            entity.Property(e => e.ResponseTime)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("ResponseTime");

            // add foreign key constraints
            entity.HasOne(d => d.Quiz).WithMany(p => p.QuizResponses)
                .HasForeignKey(d => d.QuizID)
                .HasConstraintName("fk_quiz_response_id");

            entity.HasOne(d => d.User).WithMany(p => p.QuizResponses)
                .HasForeignKey(d => d.UserID)
                .HasConstraintName("fk_user_response_id");

            entity.HasOne(d => d.Question).WithMany(p => p.QuizResponses)
                .HasForeignKey(d => d.QuestionID)
                .HasConstraintName("fk_question_response_id");
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
