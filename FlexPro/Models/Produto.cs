using System.ComponentModel.DataAnnotations;

namespace FlexPro.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string CodigoSistema { get; set; }
        public string Descricao { get; set; }
        public decimal QuantidadeFormula { get; set; }
        public decimal QuantidadeProducao { get; set; }
        public string MateriaPrima { get; set; }
        public int IdReceita { get; set; }
        public virtual Receita Receita { get; set; }
    }
}
