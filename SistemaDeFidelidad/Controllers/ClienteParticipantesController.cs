namespace SistemaDeFidelidad.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Mvc;
    using SistemaDeFidelidad.Models;
    using SistemaDeFidelidad.Models.DTOs;
    using SistemaDeFidelidad.Repository;

    /// <summary>
    /// Defines the <see cref="ClienteParticipantesController" />
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClienteParticipantesController : Controller
    {
        /// <summary>
        /// Defines the _serviceClienteParticipante
        /// </summary>
        private readonly ServiceClienteParticipante _serviceClienteParticipante;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClienteParticipantesController"/> class.
        /// </summary>
        /// <param name="serviceClienteParticipante">The serviceClienteParticipante<see cref="ServiceClienteParticipante"/></param>
        public ClienteParticipantesController(ServiceClienteParticipante serviceClienteParticipante)
        {
            _serviceClienteParticipante = serviceClienteParticipante;
        }

        // GET: ClienteParticipantes

        /// <summary>
        /// The GetAll
        /// </summary>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var clientesParticipantes = await _serviceClienteParticipante.GetAllAsync();
                return Ok(clientesParticipantes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: ClienteParticipantes/Details/5

        /// <summary>
        /// The GetByGuid
        /// </summary>
        /// <param name="id">The id<see cref="Guid?"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpGet("GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid? id)
        {
            try
            {
                //no encontrado
                if (id == null)
                {
                    return NotFound();
                }

                var clienteParticipante = await _serviceClienteParticipante.GetByGuidAsync(id);
                if (clienteParticipante == null)
                {
                    return NotFound();
                }
                return Ok(clienteParticipante);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: ClienteParticipantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// The Create
        /// </summary>
        /// <param name="clienteParticipante">The clienteParticipante<see cref="DTOClienteParticipante"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] DTOClienteParticipante clienteParticipante)
        {
            try
            {
                var clienteParticipanteEntity = new ClienteParticipante
                {
                    CedulaCliente = clienteParticipante.CedulaCliente,
                    NombreCliente = clienteParticipante.NombreCliente,
                    ApellidoCliente = clienteParticipante.ApellidoCliente,
                    EmailCliente = clienteParticipante.EmailCliente,
                    TelefonoCliente = clienteParticipante.TelefonoCliente
                };

                await _serviceClienteParticipante.AddAsync(clienteParticipanteEntity);
                return Ok(clienteParticipanteEntity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: ClienteParticipantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// The Edit
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/></param>
        /// <param name="clienteParticipante">The clienteParticipante<see cref="DTOClienteParticipante"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] DTOClienteParticipante clienteParticipante)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var clienteParticipanteFound = await _serviceClienteParticipante.GetByGuidAsync(id);

                    if (clienteParticipanteFound == null)
                    {
                        return NotFound();
                    }

                    clienteParticipanteFound.Data!.CedulaCliente = clienteParticipante.CedulaCliente;
                    clienteParticipanteFound.Data.NombreCliente = clienteParticipante.NombreCliente;
                    clienteParticipanteFound.Data.ApellidoCliente = clienteParticipante.ApellidoCliente;
                    clienteParticipanteFound.Data.EmailCliente = clienteParticipante.EmailCliente;
                    clienteParticipanteFound.Data.TelefonoCliente = clienteParticipante.TelefonoCliente;
                    clienteParticipanteFound.Data.IdCliente = id;

                   var resultado =  await _serviceClienteParticipante.UpdateAsync(clienteParticipanteFound.Data);

                    if (resultado.Success)
                    {
                        return Ok(clienteParticipanteFound.Data);
                    }else
                    {
                        return BadRequest(resultado.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return BadRequest(ex.Message);
                }
            }
            return Ok(clienteParticipante);
        }

        /// <summary>
        /// The Delete
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var clienteParticipante = await _serviceClienteParticipante.GetByGuidAsync(id);
                if (clienteParticipante == null)
                {
                    return NotFound();
                }

                var result = await _serviceClienteParticipante.DeleteByGuidAsync(clienteParticipante.Data!.IdCliente);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
