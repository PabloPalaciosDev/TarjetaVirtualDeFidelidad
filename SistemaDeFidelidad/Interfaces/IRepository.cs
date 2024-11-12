namespace SistemaDeFidelidad.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task<T> GetByGuidAsync(Guid? id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
        Task DeleteByGuidAsync(Guid? id);
        Task SaveAsync();
    }

}
