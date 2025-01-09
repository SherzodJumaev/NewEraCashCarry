using NewEraCashCarry.DTOs.UserDTOs;
using PDP.University.Examine.Project.Web.API.Models;

namespace NewEraCashCarry.Mappers.UserMaps
{
    public static class UserMappers
    {
        public static UserDto FromUserToUserDto(this User user)
        {
            return new UserDto
            {
                Username = user.Username,
                Password = user.Password,
                Role = user.Role,
            };
        }

        public static User FromUserDtoToUser(this UserDto userDto)
        {
            return new User
            {
                Username = userDto.Username,
                Password = userDto.Password,
                Role = userDto.Role,
            };
        }
    }
}
