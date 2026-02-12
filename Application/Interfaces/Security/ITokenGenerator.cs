using Application.UseCases.LoginUser.Records;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Security
{
    public interface ITokenGenerator
    {
        LoginUserResult GenerateToken(User user);
    }
}
