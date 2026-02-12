using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.GetUserProfile
{
    public record GetUserResponse(string Id, string Email);
}
