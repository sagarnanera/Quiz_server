using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quiz_server.DBcontext;
using Quiz_server.Helper;
using Quiz_server.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Data.Common;
using System.Text;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});


builder.Services
    .AddDbContext<QuizGameDbContext>(options =>
    {
        var cs = builder.Configuration.GetConnectionString("DefaultConnection");

        try
        {
            options.UseMySql(cs, ServerVersion.AutoDetect(cs));
        }
        catch (Exception e)
        {
            Console.WriteLine("Error connecting to database: " + e.Message);
            Console.WriteLine("Please try again later or contact support.");
        }
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:secret").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IUserServices, UserServices>();

builder.Services.AddScoped<IQuestionServices, QuestionServices>();

builder.Services.AddScoped<IQuizServices,QuizServices>();

builder.Services.AddScoped<IQuizResponseServices,QuizResponseServices>();

builder.Services.AddScoped<IScoreServices, ScoreServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorHandlerMiddleware>();

//app.UseExceptionHandler(errorApp =>
//{
//    errorApp.Run(async context =>
//    {
//        var exceptionHandlerPathFeature =
//            context.Features.Get<IExceptionHandlerPathFeature>();

//        var exception = exceptionHandlerPathFeature.Error;
//        if (exception is DbException)
//        {
//            context.Response.StatusCode = 500;
//            context.Response.ContentType = "application/json";
//            var errorMessage = new { Message = "Unable to connect to database." };
//            var json = JsonSerializer.Serialize(errorMessage);
//            await context.Response.WriteAsync(json);
//        }
//    });
//});

// Add other middleware and routing configuration here


app.Run();
