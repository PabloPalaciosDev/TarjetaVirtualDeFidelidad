using Microsoft.AspNetCore.Mvc;
using SistemaDeFidelidad.DbContext;
using SistemaDeFidelidad.Interfaces;
using SistemaDeFidelidad.Models;
using SistemaDeFidelidad.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SistemaDeFidelidad.Services
{
    public class ServiceClienteParticipante : IServices<ClienteParticipante>
    {
        ApplicationDbContext _context;
        public ServiceClienteParticipante(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult> AddAsync(ClienteParticipante entity)
        {
            try
            {
                //Validación de cédula existente
                if (_context.ClientesParticipantes.Any(c => c.CedulaCliente == entity.CedulaCliente))
                {
                    return ServiceResult.FailureResult("Ya existe un cliente con la misma cédula");
                }
                
                await _context.ClientesParticipantes.AddAsync(entity);
                await _context.SaveChangesAsync();

                return ServiceResult.SuccessResult("Cliente creado con éxito");
            }
            catch (Exception ex) {
                return ServiceResult.FailureResult(ex.Message);
            }
        }

        public Task<ServiceResult> DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<IEnumerable<ClienteParticipante>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<ClienteParticipante>> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UpdateAsync(ClienteParticipante entity)
        {
            throw new NotImplementedException();
        }
    }
}
