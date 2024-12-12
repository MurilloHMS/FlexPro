using ClosedXML.Excel;
using Microsoft.AspNetCore.Components.Forms;
using FlexPro.Data;
using Microsoft.EntityFrameworkCore;

namespace FlexPro.Services;

public class SepararDadosCartoes
{
    private readonly ApplicationDbContext _context;
    public SepararDadosCartoes(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<Dictionary<string, List<(DateTime data, string descricao, decimal valor)>>>
        GerarPlanilhasPorFuncionarioAsync(IBrowserFile arquivo)
    {
        var dadosFuncionarios = new Dictionary<string, List<(DateTime data, string descricao, decimal valor)>>();

        try
        {
            var funcionarios = await _context.Funcionarios.ToListAsync();
            using (var stream = new MemoryStream())
            {
                await arquivo.OpenReadStream().CopyToAsync(stream);
                stream.Position = 0;
                using (XLWorkbook workbook = new(stream))
                {
                    var sheet = workbook.Worksheets.First();
                    string funcionarioSelecionado = null;
                    bool _isCollect = false;

                    foreach (var row in sheet.RowsUsed())
                    {
                        string rowData = string.Join("\t", row.CellsUsed().Select(cell => cell.GetValue<string>()));
                        if (string.IsNullOrEmpty(rowData)) continue;

                        if (funcionarios.Any(identifier => rowData.Contains(identifier.Nome)))
                        {
                            funcionarioSelecionado = rowData;
                            if (!dadosFuncionarios.ContainsKey(funcionarioSelecionado))
                            {
                                dadosFuncionarios[funcionarioSelecionado] =
                                    new List<(DateTime date, string description, decimal value)>();
                            }
                        }

                        if (rowData.Contains("data"))
                        {
                            _isCollect = true;
                        }else if (rowData.Contains("Total de lançamentos nacionais") ||
                                  rowData.Contains("Lançamentos nacionais"))
                        {
                            _isCollect = false;
                        }

                        if (_isCollect && funcionarioSelecionado != null)
                        {
                            var columns = rowData.Split('\t');
                            if (columns.Length >= 3)
                            {
                                if (DateTime.TryParse(columns[0], out DateTime date) &&
                                    decimal.TryParse(columns[2], out decimal value))
                                {
                                    dadosFuncionarios[funcionarioSelecionado].Add((date,columns[1],value));
                                }
                            }
                        }
                    }
                }
            }

            return dadosFuncionarios;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}