using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Quiz_server.DBcontext;
using Quiz_server.DTOs;
using Quiz_server.Helper;
using Quiz_server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quiz_server.Services
{

    public interface IUserServices
    {

        User GetUser(string userName);

        UserDTO Login(UserloginModel request);
        Task RegisterUser(UserRegisterModel userRegisterModel);

        void UpdateUser(int id, UserRegisterModel updatedUser);

        void DeleteUser(string userName);

        void LogOut();

    }
    public class UserServices : IUserServices
    {
        private readonly QuizGameDbContext _dbContext;

        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;
        public UserServices(QuizGameDbContext quizGameDbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = quizGameDbContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        public void DeleteUser(string userName)
        {
            var id = (from u in _dbContext.Users where u.UserName == userName select u.UserId).FirstOrDefault();

            var user = GetUserById(id);
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

        }



        public User GetUser(string? userName)
        {
            var id = (from user in _dbContext.Users where user.UserName == userName select user.UserId).FirstOrDefault();

            if (id == null)
            {
                //Console.WriteLine("here in exception");
                 throw new KeyNotFoundException("User not found");
            }

            //Console.WriteLine("quiz found");



            //var user = GetUserById(id);

            //var userDTO = _mapper.Map<UserDTO>(user);

            return GetUserById(id);
        }

        public UserDTO Login(UserloginModel request)
        {
            if (request == null || request.password == null || request.userName == null)
            {
                throw new AppException("Invalid client request");
            }
            else
            {
                var user = (from u in _dbContext.Users
                            where u.UserName == request.userName
                            select new
                            {
                                id = u.UserId,
                                name = u.UserName,
                                password = u.Password
                            }).FirstOrDefault();

                if (user == null)
                {
                    throw new AppException("userName or password invalid...");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.password, user.password))
                {
                    throw new AppException("Wrong password.");
                }

                string token = CreateToken(user.name, user.id);

                var response = _mapper.Map<UserDTO>(request);

                response.token= token;

                return response;
            }
        }


        public async Task RegisterUser(UserRegisterModel request)
        {
            var userExists = _dbContext.Users.Any(user=>user.UserName ==request.UserName);

            if (userExists)
            {
                throw new AppException($"UserName {request.UserName} is already taken.");
            }

            string passwordHash
               = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = _mapper.Map<User>(request);

            user.Password= passwordHash;

            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();
        }

        public void UpdateUser(int id, UserRegisterModel updatedUser)
        {
            throw new AppException("Functionality is under development.");
        }


        public void LogOut()
        {
            throw new AppException("Functionality is under development.");
        }


        // helper functions......
        private User GetUserById(int? id)
        {
            var user = _dbContext.Users.Find(id.Value);
            if (user == null) throw new KeyNotFoundException("User not found");

            return user;
        }

        private string CreateToken(string userName, int? userID)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, userID.Value.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:secret").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims,
                                             expires: DateTime.Now.AddDays(1),
                                             signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
