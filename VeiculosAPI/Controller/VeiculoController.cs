using Microsoft.AspNetCore.Mvc;
using VeiculosAPI.DTOs;
using VeiculosAPI.Services;

namespace VeiculosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly IVeiculoService _veiculoService;

        public VeiculosController(IVeiculoService veiculoService)
        {
            _veiculoService = veiculoService;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            try
            {
                var veiculos = await _veiculoService.ListarTodos();
                return Ok(veiculos);
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
                var veiculo = await _veiculoService.BuscarPorId(id);
                if (veiculo == null)
                    return NotFound("Veículo não encontrado");

                return Ok(veiculo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("cliente/{clienteId}")]
        public async Task<IActionResult> BuscarPorCliente(int clienteId)
        {
            try
            {
                var veiculos = await _veiculoService.BuscarPorCliente(clienteId);
                return Ok(veiculos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CreateVeiculoDTO dto)
        {
            try
            {
                var veiculo = await _veiculoService.Criar(dto);
                return CreatedAtAction(nameof(BuscarPorId), new { id = veiculo.Id }, veiculo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] UpdateVeiculoDTO dto)
        {
            try
            {
                var veiculo = await _veiculoService.Atualizar(id, dto);
                if (veiculo == null)
                    return NotFound("Veículo não encontrado");

                return Ok(veiculo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] AtualizarStatusDTO dto)
        {
            try
            {
                var veiculo = await _veiculoService.AtualizarStatus(id, dto.Status);
                if (veiculo == null)
                    return NotFound("Veículo não encontrado");

                return Ok(veiculo);
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
                var sucesso = await _veiculoService.Deletar(id);
                if (!sucesso)
                    return NotFound("Veículo não encontrado");

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}