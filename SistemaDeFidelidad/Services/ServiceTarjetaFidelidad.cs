using SistemaDeFidelidad.Models;
using SistemaDeFidelidad.Interfaces;

namespace SistemaDeFidelidad.Services
{
    public class ServiceTarjetaFidelidad : IServices<TarjetaFidelidad>
    {
        private readonly IRepository<TarjetaFidelidad> _repository;
        private readonly IRepository<ClienteParticipante> _clienteParticipante;
        private readonly IRepository<DescuentosCliente> _descuentosCliente;

        public ServiceTarjetaFidelidad(IRepository<TarjetaFidelidad> repository, IRepository<ClienteParticipante> clienteParticipante, IRepository<DescuentosCliente> descuentosCliente)
        {
            _repository = repository;
            _clienteParticipante = clienteParticipante;
            _descuentosCliente = descuentosCliente;
        }

        public async Task<ServiceResult> AddAsync(TarjetaFidelidad entity)
        {
            //Obtener cliente
            try
            {
                var clienteFound = await _clienteParticipante.GetByGuidAsync(entity.IdCliente, "IdCliente", c => c.Tarjetas, c => c.Descuentos);
                if (clienteFound == null)
                {
                    return ServiceResult.FailureResult("Cliente no encontrado");
                }

                await _repository.AddAsync(entity);
                await _repository.SaveAsync();

                return ServiceResult.SuccessResult("Tarjeta creada con éxito");
            }
            catch (Exception e)
            {
                return ServiceResult.FailureResult(e.Message);
            }
        }

        public async Task<ServiceResult> DeleteByGuidAsync(Guid? id)
        {
            try
            {
                var tarjeta = await _repository.GetByGuidAsync(id, "IdTarjeta", t => t.Cliente, t => t.Descuentos);
                if (tarjeta == null)
                {
                    return ServiceResult.FailureResult("Tarjeta no encontrada");
                }

                await _repository.DeleteByGuidAsync(id);
                await _repository.SaveAsync();

                return ServiceResult.SuccessResult("Tarjeta eliminada con éxito");
            }
            catch (Exception e)
            {
                return ServiceResult.FailureResult(e.Message);
            }
        }

        public async Task<ServiceResult<IEnumerable<TarjetaFidelidad>>> GetAllAsync()
        {
            try
            {
                var tarjetas = await _repository.GetAllAsync();
                return ServiceResult<IEnumerable<TarjetaFidelidad>>.SuccessResult(tarjetas);
            }
            catch (Exception e)
            {
                return (ServiceResult<IEnumerable<TarjetaFidelidad>>)ServiceResult.FailureResult(e.Message);
            }
        }

        public async Task<ServiceResult<TarjetaFidelidad>> GetByGuidAsync(Guid? id)
        {
            try
            {
                var tarjeta = await _repository.GetByGuidAsync(id, "IdCliente", t => t.Cliente, t => t.Descuentos);
                if (tarjeta == null)
                {
                    return ServiceResult<TarjetaFidelidad>.FailureResult("Tarjeta no encontrada");
                }

                return ServiceResult<TarjetaFidelidad>.SuccessResult(tarjeta);
            }
            catch (Exception e)
            {

                return (ServiceResult<TarjetaFidelidad>)ServiceResult.FailureResult(e.Message);
            }
        }

        public Task<ServiceResult<TarjetaFidelidad>> GetByIdAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> UpdateAsync(TarjetaFidelidad entity)
        {
            try
            {
                if(entity.Puntos > 0 && entity.Puntos <= 1)
                {
                    return ServiceResult.FailureResult("Solo se puede asignar un punto a la vez, y debe ser mayor que 0 e igual a 1");
                }
                

                var tarjeta = await _repository.GetByGuidAsync(entity.IdTarjeta, "IdTarjeta", t => t.Cliente, t => t.Descuentos);
                if (tarjeta == null)
                {
                    return ServiceResult.FailureResult("Tarjeta no encontrada");
                }

                //Puntos mayores a 5 se registra 25% de descuento
                if (tarjeta.Puntos == 5)
                {
                    var descuento = new DescuentosCliente
                    {
                        IdCliente = tarjeta.IdCliente,
                        IdTarjeta = tarjeta.IdTarjeta,
                        CantidadDescuento = 25,
                        Usado = false,

                    };

                    await _descuentosCliente.AddAsync(descuento);
                    await _descuentosCliente.SaveAsync();
                }

                //Puntos iguales a 10 se crea una nueva tarjeta
                if (tarjeta.Puntos == 10)
                {
                    var newTarjeta = new TarjetaFidelidad
                    {
                        IdCliente = tarjeta.IdCliente,
                        Puntos = 0,
                        Activa = true,
                    };

                    await _repository.AddAsync(newTarjeta);
                    await _repository.SaveAsync();
                }


                tarjeta.Puntos = entity.Puntos;
                tarjeta.Activa = entity.Activa;

                await _repository.UpdateAsync(tarjeta);
                await _repository.SaveAsync();

                return ServiceResult.SuccessResult("Tarjeta actualizada con éxito");

            }
            catch (Exception e)
            {
                return ServiceResult.FailureResult(e.Message);
            }
        }


    }
}
