namespace SistemaDeFidelidad.Interfaces
{
    public interface IServices<T> where T : class
    {
        Task<ServiceResult> AddAsync(T entity);
        Task<ServiceResult> UpdateAsync(T entity);
        Task<ServiceResult> DeleteByGuidAsync(Guid? id);
        Task<ServiceResult<T>> GetByIdAsync(int? id);
        Task<ServiceResult<T>> GetByGuidAsync(Guid? id);
        Task<ServiceResult<IEnumerable<T>>> GetAllAsync();
    }

    /// <summary>
    /// Clase para encapsular los resultados de los servicios.
    /// </summary>
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public static ServiceResult SuccessResult(string message = "Consulta completada con éxito") =>
            new ServiceResult { Success = true, Message = message };

        public static ServiceResult FailureResult(string message) =>
            new ServiceResult { Success = false, Message = message };
    }

    /// <summary>
    /// Clase genérica para encapsular resultados con datos.
    /// </summary>
    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

        public static ServiceResult<T> SuccessResult(T data, string message = "Consulta completada con éxito") =>
            new ServiceResult<T> { Success = true, Data = data, Message = message };

        public static new ServiceResult<T> FailureResult(string message) =>
            new ServiceResult<T> { Success = false, Message = message };
    }
}
