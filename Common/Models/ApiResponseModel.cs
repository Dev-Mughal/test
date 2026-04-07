namespace Common.Models
{
    public record ApiResponseModel<T>(bool Success, string Message, T Data);
    public record ApiResponseModel(bool Success, string Message);
    public record ApiErrorResponseModel(bool Success, string Message, object Errors);

    public static class ApiResponse
    {
        public static ApiResponseModel<T> SuccessResponse<T>(T data, string message = "Request successful")
        {
            return new ApiResponseModel<T>(true, message, data);
        }
        public static ApiResponseModel SuccessResponse(string message = "Request successful")
        {
            return new ApiResponseModel(true, message);
        }
        public static ApiErrorResponseModel ErrorResponse(object errors, string message = "Request failed")
        {
            return new ApiErrorResponseModel(false, message, errors);
        }
    }

}
