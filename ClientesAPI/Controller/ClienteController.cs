using Microsoft.AspNetCore.Mvc;
using ClientesAPI.DTOs;
using ClientesAPI.Services;

namespace ClientesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            try
            {
                var clientes = await _clienteService.ListarTodos();
                return Ok(clientes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                var cliente = await _clienteService.BuscarPorId(id);
                if (cliente == null)
                    return NotFound("Cliente não encontrado");

                return Ok(cliente);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CreateClienteDTO dto)
        {
            try
            {
                var cliente = await _clienteService.Criar(dto);
                return CreatedAtAction(nameof(BuscarPorId), new { id = cliente.Id }, cliente);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] UpdateClienteDTO dto)
        {
            try
            {
                var cliente = await _clienteService.Atualizar(id, dto);
                if (cliente == null)
                    return NotFound("Cliente não encontrado");

                return Ok(cliente);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                var sucesso = await _clienteService.Deletar(id);
                if (!sucesso)
                    return NotFound("Cliente não encontrado");

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}