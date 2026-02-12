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
        public DateTime Expiration { get; init; }
        public string Id { get; init; }

        public LoginUserResult(string token,DateTime expirationTime, string id)
        {
            Token = token;
            Expiration = expirationTime;
            Id = id;
        }
    }
}
