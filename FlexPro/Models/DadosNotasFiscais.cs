using Index = FlexPro.Components.Pages.Dashboards.Veiculos.Index;

namespace FlexPro.Models;

public class DadosNotasFiscais 
{
    public string Fornecedor { get; set; }
    public string NumeroNota { get; set; }
    public string Produto { get; set; }
    public decimal ValorUnitario { get; set; }
    public string CFOP { get; set; }
}