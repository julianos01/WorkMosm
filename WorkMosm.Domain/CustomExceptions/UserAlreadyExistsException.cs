namespace WorkMosm.Domain.CustomExceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string message = "User Already exists")
        : base(message) { }

    }
}
