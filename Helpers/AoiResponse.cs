namespace Restaurant.Api.Helpers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        // Success response with data
        public ApiResponse(T data, string message = "")
        {
            Success = true;
            Data = data;
            Message = message;
        }

        // Failure response without data
        public ApiResponse(string message)
        {
            Success = false;
            Message = message;
            Data = default!;
        }
    }
}
