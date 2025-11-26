namespace OrdensServicoAPI.Models
{
    public class OrdemServico
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int VeiculoId { get; set; }
        public string DescricaoProblema { get; set; } = string.Empty;
        public DateTime DataEntrada { get; set; } = DateTime.UtcNow;
        public DateTime? DataConclusao { get; set; }
        public string Status { get; set; } = "Pendente";
        public decimal? ValorTotal { get; set; }
    }
}