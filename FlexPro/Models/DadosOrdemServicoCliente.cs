namespace FlexPro.Models;

public class DadosOrdemServicoCliente
{
    public string NumeroOS { get; set; }
    public DateTime DataAbertura { get; set; }
    public DateTime DataFechamento { get; set; }
    public int DiasDaSemana { get; set; }
    public string CodigoCliente { get; set; }
    public string NomeCliente { get; set; }
}