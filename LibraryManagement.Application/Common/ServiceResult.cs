namespace LibraryManagement.Application.Common
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public static ServiceResult<T> Success(T data) =>
            new ServiceResult<T> { IsSuccess = true, Data = data };
        public static ServiceResult<T> Failure(string errorMessage) =>
            new ServiceResult<T> { IsSuccess = false, ErrorMessage = errorMessage };
    }
}
