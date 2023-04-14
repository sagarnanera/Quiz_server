using System.ComponentModel.DataAnnotations;

namespace Quiz_server.DTOs
{
    public class UserRegisterModel
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }


    }
}
