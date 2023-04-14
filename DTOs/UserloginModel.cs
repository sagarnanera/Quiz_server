using System.ComponentModel.DataAnnotations;

namespace Quiz_server.DTOs
{
    public class UserloginModel
    {
        [Required]
        public string? userName { get; set; }
        [Required]
        public string? password { get; set; }
    }
}
