using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.RegisterUser
{
    public interface IRegisterUserUseCase
    {
        Task ExecuteAsync(string email, string passwordHash);
    }
}
