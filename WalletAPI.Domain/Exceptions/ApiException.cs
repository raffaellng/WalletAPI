namespace WalletAPI.Domain.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }

        public ApiException(int statusCode, string message)
            : base(message ?? throw new ArgumentNullException(nameof(message), "Message cannot be null"))
        {
            ValidateStatusCode(statusCode);
            StatusCode = statusCode;
        }
        private static void ValidateStatusCode(int statusCode)
        {
            if (statusCode < 100 || statusCode > 599)
                throw new ArgumentOutOfRangeException(nameof(statusCode), "Status code must be a valid HTTP status code (100-599).");
        }

    }
}
