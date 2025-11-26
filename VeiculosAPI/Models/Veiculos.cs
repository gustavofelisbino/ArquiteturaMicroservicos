namespace VeiculosAPI.Models
{
    public class Veiculo
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Ano { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Status { get; set; } = "Disponível";
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}