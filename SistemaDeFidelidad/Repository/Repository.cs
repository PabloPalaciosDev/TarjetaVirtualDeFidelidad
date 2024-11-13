using SistemaDeFidelidad.Interfaces;
using SistemaDeFidelidad.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SistemaDeFidelidad.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Get personalizados
        public async Task<T> GetByIdAsync(int? id, params Expression<Func<T, object>>[] includes)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "El identificador no puede ser nulo.");

            try
            {
                IQueryable<T> query = _context.Set<T>();

                // Incluir las propiedades relacionadas
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                // Buscar la entidad con el identificador proporcionado
                var result = await query.FirstOrDefaultAsync(entity => EF.Property<int>(entity, "Id") == id);

                return result!;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la entidad: {ex.Message}");
            }
        }

        public async Task<T> GetByGuidAsync(Guid? id, string entidad, params Expression<Func<T, object>>[] includes)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "El identificador no puede ser nulo.");

            try
            {
                IQueryable<T> query = _context.Set<T>();

                // Incluir las propiedades relacionadas
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                // Buscar la entidad con el identificador proporcionado
                var result = await query.FirstOrDefaultAsync(entity => EF.Property<Guid>(entity, entidad) == id.Value);

                return result!;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la entidad: {ex.Message}");
            }
        }


        public async Task<T?> GetByAnyAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            //Obtener propiedades relacioandas de entidad
            try
            {
                IQueryable<T> query = _context.Set<T>();
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return await query.FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
        {
            //Obtener propiedades relacioandas de entidad
            try
            {
                IQueryable<T> query = _context.Set<T>();
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public async Task AddAsync(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                await _context.SaveChangesAsync();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity != null)
                {
                    _context.Set<T>().Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteByGuidAsync(Guid? id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity != null)
                {
                    _context.Set<T>().Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

}
