using SistemaDeFidelidad.Interfaces;
using SistemaDeFidelidad.Models;

public class ServiceClienteParticipante : IServices<ClienteParticipante>
{
    private readonly IRepository<ClienteParticipante> _repository;

    public ServiceClienteParticipante(IRepository<ClienteParticipante> repository)
    {
        _repository = repository;
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
            var cliente = await _repository.GetByGuidAsync(id);
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

    public async Task<ServiceResult<IEnumerable<ClienteParticipante>>> GetAllAsync()
    {
        try
        {
            var clientes = await _repository.GetAllAsync();
            return ServiceResult<IEnumerable<ClienteParticipante>>.SuccessResult(clientes);
        }
        catch (Exception ex)
        {
            return ServiceResult<IEnumerable<ClienteParticipante>>.FailureResult(ex.Message);
        }
    }

    public async Task<ServiceResult<ClienteParticipante>> GetByIdAsync(string? id)
    {
        if (id == null)
        {
            return ServiceResult<ClienteParticipante>.FailureResult("Id no puede ser nulo");
        }

        var cliente = await _repository.GetByIdAsync(id);
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

        var cliente = await _repository.GetByGuidAsync(id);
        if (cliente == null)
        {
            return ServiceResult<ClienteParticipante>.FailureResult("Cliente no encontrado");
        }

        return ServiceResult<ClienteParticipante>.SuccessResult(cliente);
    }


    public async Task<ServiceResult> UpdateAsync(ClienteParticipante entity)
    {
        try
        {
            var cliente = await _repository.GetByIdAsync(entity.IdCliente.ToString());
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
}
