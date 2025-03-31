namespace FlexPro.Client.Models;

public class DadosRequest
{
    public List<InformativoNFe> InformativoNFes { get; set; }
    public List<InformativoPecasTrocadas> InformativoPecasTrocadas { get; set; }
    public List<InformativoOS> informativoOs { get; set; }
}