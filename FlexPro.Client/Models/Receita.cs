using System.ComponentModel.DataAnnotations;

namespace FlexPro.Client.Models
{
    public class Receita
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Embalagem { get; set; }
        public decimal ValorMaoDeObra { get; set; }
        public double Caixas { get; set; }
    }
}
