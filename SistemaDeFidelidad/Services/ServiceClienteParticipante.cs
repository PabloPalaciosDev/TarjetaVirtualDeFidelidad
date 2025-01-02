using SistemaDeFidelidad.Interfaces;
using SistemaDeFidelidad.Models;
using System.Security.Cryptography;
using System.Text;

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
            //validacion de correo existente 
            if (clientesExistentes.Any(c => c.EmailCliente == entity.EmailCliente))
            {
                return ServiceResult.FailureResult("Ya existe un cliente con el mismo correo");
            }

            //validacion contrasena existente 
            if (clientesExistentes.Any(c => c.Contrasena == entity.Contrasena))
            {
                return ServiceResult.FailureResult("Contraseña invalida");
            }

            //validacion mismo telefono
            if (clientesExistentes.Any(c => c.TelefonoCliente == entity.TelefonoCliente))
            {
                return ServiceResult.FailureResult("Ya existe un cliente con el mismo telefono");
            }

            // Hashear la contraseña
            entity.Contrasena = HashPassword(entity.Contrasena);

            await _repository.AddAsync(entity);
            await _repository.SaveAsync();

            // Obtener cliente creado
            var cliente = await _repository.GetByAnyAsync(c => c.CedulaCliente == entity.CedulaCliente);

            if (cliente == null)
            {
                return ServiceResult.FailureResult("Error al crear cliente");
            }

            // Crear tarjeta de fidelidad
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
            //mapear respuesta a un nuevo arreglo clientesMapped

            var clientesMapped = clientes.Select(c => new ClienteParticipante
            {
                IdCliente = c.IdCliente,
                CedulaCliente = c.CedulaCliente,
                NombreCliente = c.NombreCliente,
                ApellidoCliente = c.ApellidoCliente,
                EmailCliente = c.EmailCliente,
                TelefonoCliente = c.TelefonoCliente,
                Tarjetas = c.Tarjetas,
                Descuentos = c.Descuentos
            });
            return ServiceResult<IEnumerable<ClienteParticipante>>.SuccessResult(clientesMapped);
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

    public async Task<ServiceResult<ClienteParticipante>> GetByEmailAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return ServiceResult<ClienteParticipante>.FailureResult("El correo no puede ser nulo o vacío");
        }

        try
        {
            // Buscar al cliente por su correo electrónico
            var cliente = await _repository.GetByAnyAsync(c => c.EmailCliente == email);

            if (cliente == null)
            {
                return ServiceResult<ClienteParticipante>.FailureResult("Cliente no encontrado");
            }

            return ServiceResult<ClienteParticipante>.SuccessResult(cliente);
        }
        catch (Exception ex)
        {
            return ServiceResult<ClienteParticipante>.FailureResult(ex.Message);
        }
    }

    #endregion


    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            // Convierte la contraseña a un arreglo de bytes
            byte[] bytes = Encoding.UTF8.GetBytes(password);

            // Calcula el hash
            byte[] hash = sha256.ComputeHash(bytes);

            // Convierte el hash a un string hexadecimal
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}

