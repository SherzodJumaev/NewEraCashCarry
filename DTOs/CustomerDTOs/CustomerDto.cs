using System.ComponentModel.DataAnnotations;

namespace PDP.University.Examine.Project.Web.API.DTOs.CustomerDTOs
{
    public class CustomerDto
    {
        [Required]
        [MinLength(4, ErrorMessage = "First Name should be over 4 characters.")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Last Name should be over 4 characters.")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Email should be real e-mail type.")]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
