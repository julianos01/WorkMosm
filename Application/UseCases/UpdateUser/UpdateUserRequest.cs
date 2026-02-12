using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.UpdateUser
{
    public record UpdateUserRequest(string Id,string? Email, string? PasswordHash);
}
