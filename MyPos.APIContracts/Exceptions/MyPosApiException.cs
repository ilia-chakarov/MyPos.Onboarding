namespace WebAPI.Exceptions
{
    public class MyPosApiException : Exception
    {
        public int StatusCode { get; }

        public MyPosApiException(string message, int statusCode = 400) : base (message)
        {
            StatusCode = statusCode;
        }
    }
}
