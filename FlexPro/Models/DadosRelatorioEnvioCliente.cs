namespace FlexPro.Models;

public class DadosRelatorioEnvioCliente
{
    public string CodigoSistema { get; set; }
    public string Nome { get; set; }
    public List<DadosVendaCliente> DadosVendas { get; set; }
    public List<DadosOrdemServicoCliente> DadosServicos { get; set; }
}