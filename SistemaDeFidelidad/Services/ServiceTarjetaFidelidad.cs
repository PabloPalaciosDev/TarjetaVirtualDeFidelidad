using SistemaDeFidelidad.Models;
using SistemaDeFidelidad.Interfaces;

namespace SistemaDeFidelidad.Services
{
    public class ServiceTarjetaFidelidad : IServices<TarjetaFidelidad>
    {
        private readonly IRepository<TarjetaFidelidad> _repository;

        public ServiceTarjetaFidelidad(IRepository<TarjetaFidelidad> repository)
        {
            _repository = repository;
        }

        public Task<ServiceResult> AddAsync(TarjetaFidelidad entity)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> DeleteByGuidAsync(Guid? id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<IEnumerable<TarjetaFidelidad>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<TarjetaFidelidad>> GetByGuidAsync(Guid? id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<TarjetaFidelidad>> GetByIdAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UpdateAsync(TarjetaFidelidad entity)
        {
            throw new NotImplementedException();
        }
    }
}
