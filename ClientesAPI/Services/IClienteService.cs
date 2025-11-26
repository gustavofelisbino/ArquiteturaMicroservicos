using ClientesAPI.DTOs;
using ClientesAPI.Models;
using ClientesAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ClientesAPI.Services
{
    public interface IClienteService
    {
        Task<List<ClienteDTO>> ListarTodos();
        Task<ClienteDTO?> BuscarPorId(int id);
        Task<ClienteDTO> Criar(CreateClienteDTO dto);
        Task<ClienteDTO?> Atualizar(int id, UpdateClienteDTO dto);
        Task<bool> Deletar(int id);
    }

    public class ClienteService : IClienteService
    {
        private readonly AppDbContext _context;

        public ClienteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClienteDTO>> ListarTodos()
        {
            var clientes = await _context.Clientes.ToListAsync();
            return clientes.Select(c => MapToDTO(c)).ToList();
        }

        public async Task<ClienteDTO?> BuscarPorId(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            return cliente != null ? MapToDTO(cliente) : null;
        }

        public async Task<ClienteDTO> Criar(CreateClienteDTO dto)
        {
            var cliente = new Cliente
            {
                Nome = dto.Nome,
                CPF = dto.CPF,
                Telefone = dto.Telefone,
                Email = dto.Email,
                Endereco = dto.Endereco,
                DataCadastro = DateTime.UtcNow
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return MapToDTO(cliente);
        }

        public async Task<ClienteDTO?> Atualizar(int id, UpdateClienteDTO dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return null;

            if (!string.IsNullOrEmpty(dto.Nome)) cliente.Nome = dto.Nome;
            if (!string.IsNullOrEmpty(dto.Telefone)) cliente.Telefone = dto.Telefone;
            if (!string.IsNullOrEmpty(dto.Email)) cliente.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Endereco)) cliente.Endereco = dto.Endereco;

            await _context.SaveChangesAsync();
            return MapToDTO(cliente);
        }

        public async Task<bool> Deletar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }

        private ClienteDTO MapToDTO(Cliente cliente)
        {
            return new ClienteDTO
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                CPF = cliente.CPF,
                Telefone = cliente.Telefone,
                Email = cliente.Email,
                Endereco = cliente.Endereco
            };
        }
    }
}