using Application.UseCases.LoginUser.Records;
using Domain.Entities;

namespace Application.Interfaces.Security
{
    public interface ITokenGenerator
    {
        LoginUserResult GenerateToken(User user);
    }
}
