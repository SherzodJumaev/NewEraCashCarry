using System.ComponentModel.DataAnnotations;

namespace NewEraCashCarry.DTOs.UserDTOs
{
    public class UserDto
    {
        [Required]
        [MinLength(4, ErrorMessage = "Username should be over 4 characters.")]
        public string Username { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Password should be over 6 characters.")]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
