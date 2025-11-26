using OrdensServicoAPI.DTOs;
using OrdensServicoAPI.Models;
using OrdensServicoAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace OrdensServicoAPI.Services
{
    public interface IOrdemServicoService
    {
        Task<List<OrdemServicoDTO>> ListarTodas();
        Task<OrdemServicoDTO?> BuscarPorId(int id);
        Task<ClienteComHistoricoDTO?> BuscarClienteComHistorico(int clienteId);
        Task<OrdemServicoDTO> Criar(CreateOrdemServicoDTO dto);
        Task<OrdemServicoDTO?> Atualizar(int id, UpdateOrdemServicoDTO dto);
        Task<OrdemServicoDTO?> AtualizarStatus(int id, string status);
        Task<bool> Deletar(int id);
    }

    public class OrdemServicoService : IOrdemServicoService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private const string ClientesApiUrl = "http://localhost:5001/api/clientes";
        private const string VeiculosApiUrl = "http://localhost:5002/api/veiculos";

        public OrdemServicoService(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<List<OrdemServicoDTO>> ListarTodas()
        {
            var ordens = await _context.OrdensServico.ToListAsync();
            var ordensDto = new List<OrdemServicoDTO>();

            foreach (var ordem in ordens)
            {
                ordensDto.Add(await MapToDTOComDados(ordem));
            }

            return ordensDto;
        }

        public async Task<OrdemServicoDTO?> BuscarPorId(int id)
        {
            var ordem = await _context.OrdensServico.FindAsync(id);
            return ordem != null ? await MapToDTOComDados(ordem) : null;
        }

        public async Task<ClienteComHistoricoDTO?> BuscarClienteComHistorico(int clienteId)
        {
            var cliente = await BuscarClienteDaApi(clienteId);
            if (cliente == null) return null;

            var ordens = await _context.OrdensServico
                .Where(o => o.ClienteId == clienteId)
                .ToListAsync();

            var ordensDto = new List<OrdemServicoDTO>();
            foreach (var ordem in ordens)
            {
                ordensDto.Add(await MapToDTOComDados(ordem));
            }

            return new ClienteComHistoricoDTO
            {
                Cliente = cliente,
                Historico = ordensDto
            };
        }

        public async Task<OrdemServicoDTO> Criar(CreateOrdemServicoDTO dto)
        {
            var cliente = await BuscarClienteDaApi(dto.ClienteId);
            if (cliente == null)
                throw new Exception("Cliente não encontrado");

            var veiculo = await BuscarVeiculoDaApi(dto.VeiculoId);
            if (veiculo == null)
                throw new Exception("Veículo não encontrado");

            var ordem = new OrdemServico
            {
                ClienteId = dto.ClienteId,
                VeiculoId = dto.VeiculoId,
                DescricaoProblema = dto.DescricaoProblema,
                DataEntrada = DateTime.UtcNow,
                Status = "Pendente",
                ValorTotal = dto.ValorTotal
            };

            _context.OrdensServico.Add(ordem);
            await _context.SaveChangesAsync();

            return await MapToDTOComDados(ordem);
        }

        public async Task<OrdemServicoDTO?> Atualizar(int id, UpdateOrdemServicoDTO dto)
        {
            var ordem = await _context.OrdensServico.FindAsync(id);
            if (ordem == null) return null;

            if (!string.IsNullOrEmpty(dto.DescricaoProblema))
                ordem.DescricaoProblema = dto.DescricaoProblema;
            if (dto.ValorTotal.HasValue)
                ordem.ValorTotal = dto.ValorTotal;

            await _context.SaveChangesAsync();
            return await MapToDTOComDados(ordem);
        }

        public async Task<OrdemServicoDTO?> AtualizarStatus(int id, string status)
        {
            var ordem = await _context.OrdensServico.FindAsync(id);
            if (ordem == null) return null;

            ordem.Status = status;

            if (status.Equals("Em Andamento", StringComparison.OrdinalIgnoreCase))
            {
                await AtualizarStatusVeiculo(ordem.VeiculoId, "Indisponível");
            }
            else if (status.Equals("Entregue", StringComparison.OrdinalIgnoreCase))
            {
                ordem.DataConclusao = DateTime.UtcNow;
                await AtualizarStatusVeiculo(ordem.VeiculoId, "Disponível");
            }
            else if (status.Equals("Concluída", StringComparison.OrdinalIgnoreCase))
            {
                ordem.DataConclusao = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return await MapToDTOComDados(ordem);
        }

        public async Task<bool> Deletar(int id)
        {
            var ordem = await _context.OrdensServico.FindAsync(id);
            if (ordem == null) return false;

            _context.OrdensServico.Remove(ordem);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<ClienteInfo?> BuscarClienteDaApi(int clienteId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ClienteInfo>($"{ClientesApiUrl}/{clienteId}");
                return response;
            }
            catch
            {
                return null;
            }
        }

        private async Task<VeiculoInfo?> BuscarVeiculoDaApi(int veiculoId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<VeiculoInfo>($"{VeiculosApiUrl}/{veiculoId}");
                return response;
            }
            catch
            {
                return null;
            }
        }

        private async Task<bool> AtualizarStatusVeiculo(int veiculoId, string status)
        {
            try
            {
                var url = $"{VeiculosApiUrl}/{veiculoId}/status";
                var payload = new { Status = status };
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PatchAsync(url, content);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private async Task<OrdemServicoDTO> MapToDTOComDados(OrdemServico ordem)
        {
            var cliente = await BuscarClienteDaApi(ordem.ClienteId);
            var veiculo = await BuscarVeiculoDaApi(ordem.VeiculoId);

            return new OrdemServicoDTO
            {
                Id = ordem.Id,
                ClienteId = ordem.ClienteId,
                VeiculoId = ordem.VeiculoId,
                DescricaoProblema = ordem.DescricaoProblema,
                DataEntrada = ordem.DataEntrada,
                DataConclusao = ordem.DataConclusao,
                Status = ordem.Status,
                ValorTotal = ordem.ValorTotal,
                Cliente = cliente,
                Veiculo = veiculo
            };
        }
    }
}