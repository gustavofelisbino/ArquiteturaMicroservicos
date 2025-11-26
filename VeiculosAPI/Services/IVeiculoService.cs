using VeiculosAPI.DTOs;
using VeiculosAPI.Models;
using VeiculosAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace VeiculosAPI.Services
{
    public interface IVeiculoService
    {
        Task<List<VeiculoDTO>> ListarTodos();
        Task<VeiculoDTO?> BuscarPorId(int id);
        Task<List<VeiculoDTO>> BuscarPorCliente(int clienteId);
        Task<VeiculoDTO> Criar(CreateVeiculoDTO dto);
        Task<VeiculoDTO?> Atualizar(int id, UpdateVeiculoDTO dto);
        Task<VeiculoDTO?> AtualizarStatus(int id, string status);
        Task<bool> Deletar(int id);
    }

    public class VeiculoService : IVeiculoService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public VeiculoService(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<List<VeiculoDTO>> ListarTodos()
        {
            var veiculos = await _context.Veiculos.ToListAsync();
            return veiculos.Select(v => MapToDTO(v)).ToList();
        }

        public async Task<VeiculoDTO?> BuscarPorId(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            return veiculo != null ? MapToDTO(veiculo) : null;
        }

        public async Task<List<VeiculoDTO>> BuscarPorCliente(int clienteId)
        {
            var veiculos = await _context.Veiculos
                .Where(v => v.ClienteId == clienteId)
                .ToListAsync();
            return veiculos.Select(v => MapToDTO(v)).ToList();
        }

        public async Task<VeiculoDTO> Criar(CreateVeiculoDTO dto)
        {
            var clienteExiste = await VerificarClienteExiste(dto.ClienteId);
            if (!clienteExiste)
                throw new Exception("Cliente não encontrado");

            var veiculo = new Veiculo
            {
                ClienteId = dto.ClienteId,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Ano = dto.Ano,
                Placa = dto.Placa,
                Status = "Disponível",
                DataCadastro = DateTime.UtcNow
            };

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return MapToDTO(veiculo);
        }

        public async Task<VeiculoDTO?> Atualizar(int id, UpdateVeiculoDTO dto)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null) return null;

            if (!string.IsNullOrEmpty(dto.Marca)) veiculo.Marca = dto.Marca;
            if (!string.IsNullOrEmpty(dto.Modelo)) veiculo.Modelo = dto.Modelo;
            if (dto.Ano.HasValue) veiculo.Ano = dto.Ano.Value;
            if (!string.IsNullOrEmpty(dto.Status)) veiculo.Status = dto.Status;

            await _context.SaveChangesAsync();
            return MapToDTO(veiculo);
        }

        public async Task<VeiculoDTO?> AtualizarStatus(int id, string status)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null) return null;

            veiculo.Status = status;
            await _context.SaveChangesAsync();
            return MapToDTO(veiculo);
        }

        public async Task<bool> Deletar(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null) return false;

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> VerificarClienteExiste(int clienteId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:5001/api/clientes/{clienteId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private VeiculoDTO MapToDTO(Veiculo veiculo)
        {
            return new VeiculoDTO
            {
                Id = veiculo.Id,
                ClienteId = veiculo.ClienteId,
                Marca = veiculo.Marca,
                Modelo = veiculo.Modelo,
                Ano = veiculo.Ano,
                Placa = veiculo.Placa,
                Status = veiculo.Status
            };
        }
    }
}