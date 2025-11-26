using Microsoft.AspNetCore.Mvc;
using OrdensServicoAPI.DTOs;
using OrdensServicoAPI.Services;

namespace OrdensServicoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdensServicoController : ControllerBase
    {
        private readonly IOrdemServicoService _ordemService;

        public OrdensServicoController(IOrdemServicoService ordemService)
        {
            _ordemService = ordemService;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            try
            {
                var ordens = await _ordemService.ListarTodas();
                return Ok(ordens);
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
                var ordem = await _ordemService.BuscarPorId(id);
                if (ordem == null)
                    return NotFound("Ordem de serviço não encontrada");

                return Ok(ordem);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("cliente/{clienteId}/historico")]
        public async Task<IActionResult> BuscarClienteComHistorico(int clienteId)
        {
            try
            {
                var resultado = await _ordemService.BuscarClienteComHistorico(clienteId);
                if (resultado == null)
                    return NotFound("Cliente não encontrado");

                return Ok(resultado);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CreateOrdemServicoDTO dto)
        {
            try
            {
                var ordem = await _ordemService.Criar(dto);
                return CreatedAtAction(nameof(BuscarPorId), new { id = ordem.Id }, ordem);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] UpdateOrdemServicoDTO dto)
        {
            try
            {
                var ordem = await _ordemService.Atualizar(id, dto);
                if (ordem == null)
                    return NotFound("Ordem de serviço não encontrada");

                return Ok(ordem);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] AtualizarStatusOrdemDTO dto)
        {
            try
            {
                var ordem = await _ordemService.AtualizarStatus(id, dto.Status);
                if (ordem == null)
                    return NotFound("Ordem de serviço não encontrada");

                return Ok(ordem);
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
                var sucesso = await _ordemService.Deletar(id);
                if (!sucesso)
                    return NotFound("Ordem de serviço não encontrada");

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}