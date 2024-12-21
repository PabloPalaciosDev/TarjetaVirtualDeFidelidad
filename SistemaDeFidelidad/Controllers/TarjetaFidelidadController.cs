namespace SistemaDeFidelidad.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Mvc;
    using SistemaDeFidelidad.Models;
    using SistemaDeFidelidad.Models.DTOs;
    using SistemaDeFidelidad.Services;

    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TarjetaFidelidadController : Controller
    {
        private readonly ServiceTarjetaFidelidad _serviceTarjetaFidelidad;

        public TarjetaFidelidadController(ServiceTarjetaFidelidad serviceTarjetaFidelidad)
        {
            _serviceTarjetaFidelidad = serviceTarjetaFidelidad;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TarjetaFidelidad tarjetaFidelidad)
        {
            try
            {
                var result = await _serviceTarjetaFidelidad.AddAsync(tarjetaFidelidad);
                if (result.Success)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            try
            {
                var result = await _serviceTarjetaFidelidad.DeleteByGuidAsync(id);
                if (result.Success)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tarjeta = await _serviceTarjetaFidelidad.GetByGuidAsync(id);
                if (tarjeta == null)
                {
                    return NotFound();
                }
                return Ok(tarjeta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("AddPoints")]
        public async Task<IActionResult> Update([FromBody] DTOAgregarPuntosTarjeta tarjetaFidelidad)
        {
            try
            {

                var tarjeta = new TarjetaFidelidad
                {
                    IdTarjeta = tarjetaFidelidad.IdTarjeta,
                    IdCliente = tarjetaFidelidad.IdCliente,
                    Puntos = tarjetaFidelidad.Puntos,
                    Activa = true
                };

                var result = await _serviceTarjetaFidelidad.UpdateAsync(tarjeta);
                if (result.Success)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
