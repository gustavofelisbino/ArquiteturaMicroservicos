namespace OrdensServicoAPI.DTOs
{
	public class OrdemServicoDTO
	{
		public int Id { get; set; }
		public int ClienteId { get; set; }
		public int VeiculoId { get; set; }
		public string DescricaoProblema { get; set; } = string.Empty;
		public DateTime DataEntrada { get; set; }
		public DateTime? DataConclusao { get; set; }
		public string Status { get; set; } = string.Empty;
		public decimal? ValorTotal { get; set; }
		public ClienteInfo? Cliente { get; set; }
		public VeiculoInfo? Veiculo { get; set; }
	}

	public class CreateOrdemServicoDTO
	{
		public int ClienteId { get; set; }
		public int VeiculoId { get; set; }
		public string DescricaoProblema { get; set; } = string.Empty;
		public decimal? ValorTotal { get; set; }
	}

	public class UpdateOrdemServicoDTO
	{
		public string? DescricaoProblema { get; set; }
		public decimal? ValorTotal { get; set; }
	}

	public class AtualizarStatusOrdemDTO
	{
		public string Status { get; set; } = string.Empty;
	}

	public class ClienteInfo
	{
		public int Id { get; set; }
		public string Nome { get; set; } = string.Empty;
		public string CPF { get; set; } = string.Empty;
		public string Telefone { get; set; } = string.Empty;
	}

	public class VeiculoInfo
	{
		public int Id { get; set; }
		public string Marca { get; set; } = string.Empty;
		public string Modelo { get; set; } = string.Empty;
		public string Placa { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;
	}

	public class ClienteComHistoricoDTO
	{
		public ClienteInfo Cliente { get; set; } = new();
		public List<OrdemServicoDTO> Historico { get; set; } = new();
	}
}