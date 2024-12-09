namespace FlexPro.Services;

public class Calculos
{
    public static string CalcularDesempenho<T>(T valorAtual, T valorAnterior, string tipo) where T : struct
    {
        if (valorAtual.Equals(0))
        {
            return "Sem Dados Para Comparação";
        }
        
        var porcentagem = CalcularPorcentagem(Convert.ToDouble(valorAtual), Convert.ToDouble(valorAnterior));
        var descricao = porcentagem < 0 ? "caiu" : "aumentou";
        string desempenho = default;
        switch (tipo.ToString())
        {
            case "C":
                desempenho = $"{descricao} em {porcentagem:P} de {valorAnterior:C} para {valorAtual:C}";
                break;
            case "N0":
                desempenho = $"{descricao} em {porcentagem:P} de {valorAnterior:N0} para {valorAtual:N0}";
                break;
            case "N":
                desempenho = $"{descricao} em {porcentagem:P} de {valorAnterior:N} para {valorAtual:N}";
                break;
        }
        return desempenho;
    }
    
    public static double CalcularPorcentagem(double? atual, double? anterior)
    {
        if (anterior == 0) return 0;
        return ((atual.Value / anterior.Value) - 1);
    }
}