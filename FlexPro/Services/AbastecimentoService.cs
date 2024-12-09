using FlexPro.Data;
using System.Text;
using Microsoft.EntityFrameworkCore;
using FlexPro.Models;

namespace FlexPro.Services;

public class AbastecimentoService
{
    private readonly ApplicationDbContext _context;

    public AbastecimentoService(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<string> CalcularAbastecimentoGeral(DateTime data)
    {
        var dataAtual = data.ToUniversalTime();
        var inicioMesAtual = new DateTime(dataAtual.Year, dataAtual.Month, 1).ToUniversalTime();
        var fimMesAtual = inicioMesAtual.AddMonths(1).AddDays(-1);
        var inicioMesAnterior = inicioMesAtual.AddMonths(-1);
        var fimMesAnterior = inicioMesAtual.AddDays(-1);
        
        var abastecimentoMesAtual = await _context.Abastecimento.Where(a => a.DataDoAbastecimento >= inicioMesAtual && a.DataDoAbastecimento <= fimMesAtual).ToListAsync();
        var abastecimentoMesAnterior = await _context.Abastecimento.Where(a => a.DataDoAbastecimento >= inicioMesAnterior && a.DataDoAbastecimento <= fimMesAnterior).ToListAsync();

        var sb = new StringBuilder();

        var totalLitrosMesAtual = abastecimentoMesAtual.Sum(a => a.Litros);
        var totalLitrosMesAnterior = abastecimentoMesAnterior.Sum(a => a.Litros);

        var totalPercorridoMesAtual = abastecimentoMesAtual.Sum(a => a.DiferencaHodometro);
        var totalPercorridoMesAnterior = abastecimentoMesAnterior.Sum(a => a.DiferencaHodometro);
        
        var mediaKmMesAtual = abastecimentoMesAtual.Average(a => a.MediaKm);
        var mediaKmMesAnterior = abastecimentoMesAnterior.Average(a => a.MediaKm);

        var valorTotalGastoMesAtual = abastecimentoMesAtual.Sum(a => a.ValorTotalTransacao);
        var valorTotalGastoMesAnterior = abastecimentoMesAnterior.Sum(a => a.ValorTotalTransacao);

        var mediaPrecoLitroMesAtual = abastecimentoMesAtual.Average(a => a.Preco);
        var mediaPrecoLitroMesAnterior = abastecimentoMesAnterior.Average(a => a.Preco);
        
        sb.AppendLine($"Abastecimento Geral");
        sb.AppendLine($"Quantidade de litros abastecidos: {Calculos.CalcularDesempenho(totalLitrosMesAtual, totalLitrosMesAnterior,"N")}");
        sb.AppendLine($"Média de KM/L: {Calculos.CalcularDesempenho(mediaKmMesAtual, mediaKmMesAnterior, "N")}");
        sb.AppendLine($"Média de Preço/L: {Calculos.CalcularDesempenho(mediaPrecoLitroMesAtual, mediaPrecoLitroMesAnterior, "C")}");
        sb.AppendLine($"Valor Total Gasto: {Calculos.CalcularDesempenho(valorTotalGastoMesAtual, valorTotalGastoMesAnterior, "C")}");
        sb.AppendLine($"Distancia percorrida: {Calculos.CalcularDesempenho(totalPercorridoMesAtual, totalPercorridoMesAnterior, "N0")}");
        
        return sb.ToString();
    }
    
    public async Task<string> CalcularAbastecimentoSetor(DateTime data)
    {
        var dataAtual = data.ToUniversalTime();
        var inicioMesAtual = new DateTime(dataAtual.Year, dataAtual.Month, 1).ToUniversalTime();
        var fimMesAtual = inicioMesAtual.AddMonths(1).AddDays(-1);
        var inicioMesAnterior = inicioMesAtual.AddMonths(-1);
        var fimMesAnterior = inicioMesAtual.AddDays(-1);
        
        var abastecimentoMesAtual = await _context.Abastecimento.Where(a => a.DataDoAbastecimento >= inicioMesAtual && a.DataDoAbastecimento <= fimMesAtual).ToListAsync();
        var abastecimentoMesAnterior = await _context.Abastecimento.Where(a => a.DataDoAbastecimento >= inicioMesAnterior && a.DataDoAbastecimento <= fimMesAnterior).ToListAsync();
        var departamentos = await _context.Abastecimento.Select(a => a.Departamento).Distinct().ToListAsync();

        var sb = new StringBuilder();

        foreach (var departamento in departamentos)
        {
            var abastecimentoAtualDepto = abastecimentoMesAtual.Where(a => a.Departamento == departamento);
            var abastecimentoAnteriorDepto = abastecimentoMesAnterior.Where(a => a.Departamento == departamento);

            var totalLitrosMesAtual = abastecimentoAtualDepto.Sum(a => a.Litros);
            var totalLitrosMesAnterior = abastecimentoAnteriorDepto.Sum(a => a.Litros);

            var totalPercorridoMesAtual = abastecimentoAtualDepto.Sum(a => a.DiferencaHodometro);
            var totalPercorridoMesAnterior = abastecimentoAnteriorDepto.Sum(a => a.DiferencaHodometro);
            
            var mediaKmMesAtual = abastecimentoAtualDepto.Average(a => a.MediaKm);
            var mediaKmMesAnterior = abastecimentoAnteriorDepto.Average(a => a.MediaKm);

            var valorTotalGastoMesAtual = abastecimentoAtualDepto.Sum(a => a.ValorTotalTransacao);
            var valorTotalGastoMesAnterior = abastecimentoAnteriorDepto.Sum(a => a.ValorTotalTransacao);

            var mediaPrecoLitroMesAtual = abastecimentoAtualDepto.Average(a => a.Preco);
            var mediaPrecoLitroMesAnterior = abastecimentoAnteriorDepto.Average(a => a.Preco);

            sb.AppendLine($"Departamento {departamento}");
            sb.AppendLine($"Quantidade de litros abastecidos: {Calculos.CalcularDesempenho(totalLitrosMesAtual, totalLitrosMesAnterior,"N")}");
            sb.AppendLine($"Média de KM/L: {Calculos.CalcularDesempenho(mediaKmMesAtual, mediaKmMesAnterior, "N")}");
            sb.AppendLine($"Média de Preço/L: {Calculos.CalcularDesempenho(mediaPrecoLitroMesAtual, mediaPrecoLitroMesAnterior, "C")}");
            sb.AppendLine($"Valor Total Gasto: {Calculos.CalcularDesempenho(valorTotalGastoMesAtual, valorTotalGastoMesAnterior, "C")}");
            sb.AppendLine($"Distancia percorrida: {Calculos.CalcularDesempenho(totalPercorridoMesAtual, totalPercorridoMesAnterior, "N0")}");
            sb.AppendLine();

        }
        return sb.ToString();
    }
    
    public async Task<string> CalcularAbastecimentoIndividual(DateTime data)
    {
        var dataAtual = data.ToUniversalTime();
        var inicioMesAtual = new DateTime(dataAtual.Year, dataAtual.Month, 1).ToUniversalTime();
        var fimMesAtual = inicioMesAtual.AddMonths(1).AddDays(-1);
        var inicioMesAnterior = inicioMesAtual.AddMonths(-1);
        var fimMesAnterior = inicioMesAtual.AddDays(-1);
        
        var abastecimentoMesAtual = await _context.Abastecimento.Where(a => a.DataDoAbastecimento >= inicioMesAtual && a.DataDoAbastecimento <= fimMesAtual).ToListAsync();
        var abastecimentoMesAnterior = await _context.Abastecimento.Where(a => a.DataDoAbastecimento >= inicioMesAnterior && a.DataDoAbastecimento <= fimMesAnterior).ToListAsync();
        var funcionarios = await _context.Abastecimento.Select(a => a.NomeDoMotorista).Distinct().ToListAsync();

        var sb = new StringBuilder();

        foreach (var funcionario in funcionarios)
        {
            try
            {
                var abastecimentoAtualFuncionario = abastecimentoMesAtual.Where(a => a.NomeDoMotorista == funcionario);
                var abastecimentoAnteriorFuncionario =
                    abastecimentoMesAnterior.Where(a => a.NomeDoMotorista == funcionario);

                var totalLitrosMesAtual = abastecimentoAtualFuncionario.Sum(a => a.Litros);
                var totalLitrosMesAnterior = abastecimentoAnteriorFuncionario.Sum(a => a.Litros);

                var totalPercorridoMesAtual = abastecimentoAtualFuncionario.Sum(a => a.DiferencaHodometro);
                var totalPercorridoMesAnterior = abastecimentoAnteriorFuncionario.Sum(a => a.DiferencaHodometro);

                var mediaKmMesAtual = abastecimentoAtualFuncionario.Average(a => a.MediaKm);
                var mediaKmMesAnterior = abastecimentoAnteriorFuncionario.Average(a => a.MediaKm);

                var valorTotalGastoMesAtual = abastecimentoAtualFuncionario.Sum(a => a.ValorTotalTransacao);
                var valorTotalGastoMesAnterior = abastecimentoAnteriorFuncionario.Sum(a => a.ValorTotalTransacao);

                var mediaPrecoLitroMesAtual = abastecimentoAtualFuncionario.Average(a => a.Preco);
                var mediaPrecoLitroMesAnterior = abastecimentoAnteriorFuncionario.Average(a => a.Preco);

                sb.AppendLine($"Funcionário {funcionario}");
                sb.AppendLine($"Quantidade de litros abastecidos: {Calculos.CalcularDesempenho(totalLitrosMesAtual, totalLitrosMesAnterior, "N")}");
                sb.AppendLine($"Média de KM/L: {Calculos.CalcularDesempenho(mediaKmMesAtual, mediaKmMesAnterior, "N")}");
                sb.AppendLine($"Média de Preço/L: {Calculos.CalcularDesempenho(mediaPrecoLitroMesAtual, mediaPrecoLitroMesAnterior, "C")}");
                sb.AppendLine($"Valor Total Gasto: {Calculos.CalcularDesempenho(valorTotalGastoMesAtual, valorTotalGastoMesAnterior, "C")}");
                sb.AppendLine($"Distancia percorrida: {Calculos.CalcularDesempenho(totalPercorridoMesAtual, totalPercorridoMesAnterior, "N0")}");
                sb.AppendLine();
            }
            catch (Exception)
            {
                continue;
            }
            
        }
        return sb.ToString();
    }
}