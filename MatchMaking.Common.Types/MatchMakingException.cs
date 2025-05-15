namespace MatchMaking.Common.Types
{
    public class MatchMakingException : Exception
    {
        public ApplicationErrorTypes ErrorType { get; }
        public string ErrorMessage { get; }

        public MatchMakingException(ApplicationErrorTypes errorType, string message) : base(message)
        {
            ErrorType = errorType;

            ErrorMessage = message;
        }
    }

    public enum ApplicationErrorTypes
    {
        GeneralError = 1,

        LimitExceeded = 2,

        MatchNotExists = 3,
    }
}