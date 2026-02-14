using WorkMosm.Application.UseCases.LoginUser.Records;
using WorkMosm.Domain.Entities;

namespace WorkMosm.Application.Interfaces.Security
{
    public interface ITokenGenerator
    {
        LoginUserResult GenerateToken(User user);
    }
}
