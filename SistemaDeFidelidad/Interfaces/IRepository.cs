using System.Linq.Expressions;

namespace SistemaDeFidelidad.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int? id, params Expression<Func<T, object>>[] includes);
        Task<T> GetByGuidAsync(Guid? id, string entidad, params Expression<Func<T, object>>[] includes);
        Task<T?> GetByAnyAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task DeleteByGuidAsync(Guid? id);
        Task SaveAsync();

        // Nuevo método para incluir propiedades relacionadas
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
    }
}
