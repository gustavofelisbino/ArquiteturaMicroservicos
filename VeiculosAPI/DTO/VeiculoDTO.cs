namespace VeiculosAPI.DTOs
{
    public class VeiculoDTO
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Ano { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class CreateVeiculoDTO
    {
        public int ClienteId { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Ano { get; set; }
        public string Placa { get; set; } = string.Empty;
    }

    public class UpdateVeiculoDTO
    {
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int? Ano { get; set; }
        public string? Status { get; set; }
    }

    public class AtualizarStatusDTO
    {
        public string Status { get; set; } = string.Empty;
    }
}