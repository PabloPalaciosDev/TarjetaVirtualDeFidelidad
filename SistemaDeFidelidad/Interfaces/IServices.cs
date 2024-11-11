namespace SistemaDeFidelidad.Interfaces
{
    public interface IServices<T> where T : class
    {
        Task<ServiceResult> AddAsync(T entity);
        Task<ServiceResult> UpdateAsync(T entity);
        Task<ServiceResult> DeleteAsync(object id);
        Task<ServiceResult<T>> GetByIdAsync(string id);
        Task<ServiceResult<IEnumerable<T>>> GetAllAsync();
    }

    /// <summary>
    /// Clase para encapsular los resultados de los servicios.
    /// </summary>
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static ServiceResult SuccessResult(string message = "Operation successful") =>
            new ServiceResult { Success = true, Message = message };

        public static ServiceResult FailureResult(string message) =>
            new ServiceResult { Success = false, Message = message };
    }

    /// <summary>
    /// Clase genérica para encapsular resultados con datos.
    /// </summary>
    public class ServiceResult<T> : ServiceResult
    {
        public T Data { get; set; }

        public static ServiceResult<T> SuccessResult(T data, string message = "Operation successful") =>
            new ServiceResult<T> { Success = true, Data = data, Message = message };

        public static ServiceResult<T> FailureResult(string message) =>
            new ServiceResult<T> { Success = false, Message = message };
    }
}
