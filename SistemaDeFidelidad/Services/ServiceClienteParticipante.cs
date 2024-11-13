using SistemaDeFidelidad.Interfaces;
using SistemaDeFidelidad.Models;

public class ServiceClienteParticipante : IServices<ClienteParticipante>
{
    private readonly IRepository<ClienteParticipante> _repository;
    private readonly IRepository<TarjetaFidelidad> _repositoryTarjetaFidelidad;

    public ServiceClienteParticipante(IRepository<ClienteParticipante> repository, IRepository<TarjetaFidelidad> repositoryTarjetaFidelidad)
    {
        _repository = repository;
        _repositoryTarjetaFidelidad = repositoryTarjetaFidelidad;
    }

    public async Task<ServiceResult> AddAsync(ClienteParticipante entity)
    {
        try
        {
            // Validación de cédula existente
            var clientesExistentes = await _repository.GetAllAsync();
            if (clientesExistentes.Any(c => c.CedulaCliente == entity.CedulaCliente))
            {
                return ServiceResult.FailureResult("Ya existe un cliente con la misma cédula");
            }

            await _repository.AddAsync(entity);
            await _repository.SaveAsync();


            //Obtener cliente creado
            var cliente = await _repository.GetByAnyAsync(c => c.CedulaCliente == entity.CedulaCliente);

            if (cliente == null) {
                return ServiceResult.FailureResult("Error al crear cliente");
            }

            //Crear tarjeta de fidelidad
            var tarjetaFidelidad = new TarjetaFidelidad
            {
                IdTarjeta = Guid.NewGuid(),
                IdCliente = cliente.IdCliente,
                Puntos = 0,
                Activa = true
            };

            await _repositoryTarjetaFidelidad.AddAsync(tarjetaFidelidad);
            await _repositoryTarjetaFidelidad.SaveAsync();


            return ServiceResult.SuccessResult("Cliente creado con éxito");
        }
        catch (Exception ex)
        {
            return ServiceResult.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult> DeleteByGuidAsync(Guid? id)
    {
        try
        {
            var cliente = await _repository.GetByGuidAsync(id, "IdCliente", c => c.Tarjetas, c => c.Descuentos);
            if (cliente == null)
            {
                return ServiceResult.FailureResult("Cliente no encontrado");
            }

            await _repository.DeleteByGuidAsync(cliente.IdCliente);
            await _repository.SaveAsync();

            return ServiceResult.SuccessResult("Cliente eliminado con éxito");
        }
        catch (Exception ex)
        {
            return ServiceResult.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult> UpdateAsync(ClienteParticipante entity)
    {
        try
        {
            var cliente = await _repository.GetByGuidAsync(entity.IdCliente, "IdCliente", c => c.Tarjetas, c => c.Descuentos);
            if (cliente == null)
            {
                return ServiceResult.FailureResult("Cliente no encontrado");
            }

            await _repository.UpdateAsync(entity);
            await _repository.SaveAsync();

            return ServiceResult.SuccessResult("Cliente actualizado con éxito");
        }
        catch (Exception ex)
        {
            return ServiceResult.FailureResult(ex.Message);
        }
    }

    #region Gets personalizados
    public async Task<ServiceResult<IEnumerable<ClienteParticipante>>> GetAllAsync()
    {
        try
        {
            //Obtener tarjetas y descuentos registrados del cliente
            var clientes = await _repository.GetAllWithIncludesAsync(c => c.Tarjetas, c => c.Descuentos);
            return ServiceResult<IEnumerable<ClienteParticipante>>.SuccessResult(clientes);
        }
        catch (Exception ex)
        {
            return ServiceResult<IEnumerable<ClienteParticipante>>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<ClienteParticipante>> GetByIdAsync(int? id)
    {
        if (id == null)
        {
            return ServiceResult<ClienteParticipante>.FailureResult("Id no puede ser nulo");
        }

        var cliente = await _repository.GetByIdAsync(id, c => c.Tarjetas, c => c.Descuentos);
        if (cliente == null)
        {
            return ServiceResult<ClienteParticipante>.FailureResult("Cliente no encontrado");
        }

        return ServiceResult<ClienteParticipante>.SuccessResult(cliente);
    }

    public async Task<ServiceResult<ClienteParticipante>> GetByGuidAsync(Guid? id)
    {
        if (id == null)
        {
            return ServiceResult<ClienteParticipante>.FailureResult("Id no puede ser nulo");
        }

        var cliente = await _repository.GetByGuidAsync(id,"IdCliente", c => c.Tarjetas, c => c.Descuentos);
        if (cliente == null)
        {
            return ServiceResult<ClienteParticipante>.FailureResult("Cliente no encontrado");
        }

        return ServiceResult<ClienteParticipante>.SuccessResult(cliente);
    }

    #endregion
}
