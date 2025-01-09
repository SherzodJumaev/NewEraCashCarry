using PDP.University.Examine.Project.Web.API.Models;

namespace NewEraCashCarry.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}
