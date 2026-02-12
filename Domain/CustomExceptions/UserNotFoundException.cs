using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomExceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string email):base($"User with email '{email}' was not found.")
        {
        }
    }
}
