namespace SistemaDeFidelidad.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
    using SistemaDeFidelidad.Models;
    using SistemaDeFidelidad.Models.DTOs;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;


    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClienteParticipantesController : Controller
    {
        private readonly ServiceClienteParticipante _serviceClienteParticipante;
        private readonly IConfiguration _configuration;

        public ClienteParticipantesController(ServiceClienteParticipante serviceClienteParticipante, IConfiguration configuration)
        {
            _serviceClienteParticipante = serviceClienteParticipante;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            try
            {
                // Buscar al cliente por email
                var clienteParticipante = await _serviceClienteParticipante.GetByEmailAsync(loginRequest.Email);
                if (!clienteParticipante.Success)
                {
                    return Unauthorized(new { message = "Credenciales inválidas" });
                }

                // Hashear la contraseña ingresada
                string hashedPassword = HashPassword(loginRequest.Password);

                // Comparar la contraseña hasheada con la almacenada en la base de datos
                if (clienteParticipante.Data!.Contrasena != hashedPassword)
                {
                    return Unauthorized(new { message = "Credenciales inválidas" });
                }

                // Crear los claims para el token JWT
                var claims = new[]
                {
            new Claim(ClaimTypes.Name, clienteParticipante.Data.NombreCliente),
            new Claim(ClaimTypes.Email, clienteParticipante.Data.EmailCliente),
            new Claim(ClaimTypes.NameIdentifier, clienteParticipante.Data.IdCliente.ToString()),
        };

                // Generar el token JWT utilizando los valores de appsettings.json
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: creds
                );

                // Retornar el token al cliente
                return Ok(new
                {
                    idCliente = clienteParticipante.Data.IdCliente,
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    name = clienteParticipante.Data.NombreCliente,
                    lastname = clienteParticipante.Data.ApellidoCliente,
                    cedula = clienteParticipante.Data.CedulaCliente,
                    Email = clienteParticipante.Data.EmailCliente,
                    Tarjeta = clienteParticipante.Data.Tarjetas
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize]
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


        [Authorize]
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
                    TelefonoCliente = clienteParticipante.TelefonoCliente,
                    Contrasena = clienteParticipante.Contrasena
                };

                var response = await _serviceClienteParticipante.AddAsync(clienteParticipanteEntity);
                //respuesta con message del proceso
                if (response.Success)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
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

        [Authorize]
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

        //endpoint para validar si el token es valido
        [Authorize]
        [HttpGet("ValidateToken")]
        public IActionResult ValidateToken()
        {
            return Ok(new { message = "Token válido" });
        }
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
}
