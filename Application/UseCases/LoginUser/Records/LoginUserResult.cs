using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.LoginUser.Records
{
    public record LoginUserResult
    {
        public string Token { get; init; }
        public string RefreshToken { get; init; }
        public DateTime Expiration { get; init; }
        public Guid Id { get; init; }
        public LoginUserResult(string token, Guid id)
        {
            Token = token;
            Id = id;
        }
    }
}
