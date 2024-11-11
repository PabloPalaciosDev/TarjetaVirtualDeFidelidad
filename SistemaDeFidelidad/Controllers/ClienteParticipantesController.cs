using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaDeFidelidad.DbContext;
using SistemaDeFidelidad.Models;
using SistemaDeFidelidad.Models.DTOs;
using Asp.Versioning;

namespace SistemaDeFidelidad.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClienteParticipantesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClienteParticipantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClienteParticipantes
        [HttpGet("GetAll")]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.ClientesParticipantes.ToListAsync());
        }

        // GET: ClienteParticipantes/Details/5
        [HttpGet("GetByGuid")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteParticipante = await _context.ClientesParticipantes
                .FirstOrDefaultAsync(m => m.IdCliente == id);
            if (clienteParticipante == null)
            {
                return NotFound();
            }

            return Ok(clienteParticipante);
        }

        // POST: ClienteParticipantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]DTOClienteParticipante clienteParticipante)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Verificar si ya existe un cliente con la misma cédula
                    if (_context.ClientesParticipantes.Any(c => c.CedulaCliente == clienteParticipante.CedulaCliente))
                    {
                        ModelState.AddModelError("CedulaCliente", "Ya existe un cliente con esta cédula.");
                        return Ok("Clienta ya registrado");
                    }

                    var clienteNuevo = new ClienteParticipante
                    {
                        IdCliente = Guid.NewGuid(),
                        CedulaCliente = clienteParticipante.CedulaCliente,
                        NombreCliente = clienteParticipante.NombreCliente,
                        ApellidoCliente = clienteParticipante.ApellidoCliente,
                        EmailCliente = clienteParticipante.EmailCliente,
                        TelefonoCliente = clienteParticipante.TelefonoCliente
                    };
                    _context.ClientesParticipantes.Add(clienteNuevo);
                    await _context.SaveChangesAsync();
                }
                return Ok(clienteParticipante);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // POST: ClienteParticipantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Update")]
        public async Task<IActionResult> Edit(Guid id,[FromBody] DTOClienteParticipante clienteParticipante)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var clienteParticipanteFound = await _context.ClientesParticipantes.FindAsync(id);

                    if (clienteParticipanteFound == null)
                    {
                        return NotFound();
                    }

                    clienteParticipanteFound.CedulaCliente = clienteParticipante.CedulaCliente;
                    clienteParticipanteFound.NombreCliente = clienteParticipante.NombreCliente;
                    clienteParticipanteFound.ApellidoCliente = clienteParticipante.ApellidoCliente;
                    clienteParticipanteFound.EmailCliente = clienteParticipante.EmailCliente;
                    clienteParticipanteFound.TelefonoCliente = clienteParticipante.TelefonoCliente;
                    clienteParticipanteFound.IdCliente = id;

                    _context.ClientesParticipantes.Update(clienteParticipanteFound);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return BadRequest(ex.Message);
                }
            }
            return Ok(clienteParticipante);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var clienteParticipante = await _context.ClientesParticipantes.FindAsync(id);
            if (clienteParticipante != null)
            {
                _context.ClientesParticipantes.Remove(clienteParticipante);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

