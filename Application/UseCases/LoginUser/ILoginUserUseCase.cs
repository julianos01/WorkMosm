using Application.UseCases.LoginUser.Records;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.LoginUser
{
    public interface ILoginUserUseCase
    {
        Task<LoginUserResult> ExecuteAsync(LoginUserRequest request);

    }
}
