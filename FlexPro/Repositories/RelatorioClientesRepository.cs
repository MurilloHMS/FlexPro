using FlexPro.Data;
using FlexPro.Models;
using Microsoft.EntityFrameworkCore;

namespace FlexPro.Repositories;

public class RelatorioClientesRepository
{
    private readonly ApplicationDbContext _context;

    public RelatorioClientesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task GetMetrics(IEnumerable<DadosRelatorioEnvioCliente> relatorio)
    {
        var clientes = await _context.Cliente.ToListAsync();

        foreach (var cliente in clientes)  
        {
            var quantidadeDeProdutosComprados = relatorio.Select(x => x.DadosVendas.Select(y => y.Produto).Count());
            var quantidadeDeNotasFiscais =
                relatorio.Select(x => x.DadosVendas.Select(c => c.NumeroNfe).Count());
        }
    }

    public async Task SendEmailCliente()
    {
        var body = @"<!DOCTYPE html>
                    <html lang=""pt-br"">
                    <head>
                        <meta charset=""UTF-8"">
                        <title>Proauto Kimium - Métricas Mensais</title>
                    </head>
                    <body style=""margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4; text-align: center;"">
                        <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                            <tr>
                                <td align=""center"">
                                    
                                    <!-- Cabeçalho -->
                                    <table role=""presentation"" width=""600"" cellspacing=""0"" cellpadding=""20"" border=""0"" style=""background: #262d61; color: #ffffff; border-radius: 8px 8px 0 0; background-size: 100%;"">
                                        <tr>
                                            <td align=""center"">
                                                <img src=""https://www.proautokimium.com.br/images/2020/11/icone-proauto-150x150.png"" alt=""Proauto Kimium"" width=""60"" height=""60"">
                                                <h2 style=""margin: 5px 0;"">Métricas Mensais</h2>
                                                <h3 style=""margin: 5px 0;"">Fevereiro - 2025</h3>
                                            </td>
                                        </tr>
                                    </table>

                                    <!-- Mensagem de Introdução -->
                                    <table role=""presentation"" width=""600"" cellspacing=""0"" cellpadding=""20"" border=""0"" style=""background-color: #ffffff;"">
                                        <tr>
                                            <td align=""center"">
                                                <p style=""color: #555; font-size: 16px;"">
                                                    Olá, <strong>Cliente</strong>! Veja como foi seu desempenho neste mês com a <strong>Proauto Kimium</strong>.
                                                </p>
                                            </td>
                                        </tr>
                                    </table>

                                    <!-- Métricas Principais -->
                                    <table role=""presentation"" width=""600"" cellspacing=""0"" cellpadding=""15"" border=""0"" style=""background-color: #ffffff;"">
                                        <tr>
                                            <td align=""center"" width=""33%"" style=""border-right: 1px solid #ddd;"">
                                                <h1 style=""color: #007bff; margin: 0; font-size: 2rem;"">106</h1>
                                                <p style=""color: #555; font-size: 14px;"">Litros de Produtos Comprados</p>
                                            </td>
                                            <td align=""center"" width=""33%"" style=""border-right: 1px solid #ddd;"">
                                                <h1 style=""color: #007bff; margin: 0; font-size: 2rem;"">17</h1>
                                                <p style=""color: #555; font-size: 14px;"">Notas Fiscais Emitidas</p>
                                            </td>
                                            <td align=""center"" width=""33%"">
                                                <h1 style=""color: #007bff; margin: 0; font-size: 2rem;"">09</h1>
                                                <p style=""color: #555; font-size: 14px;"">Manutenções Preventivas</p>
                                            </td>
                                        </tr>
                                    </table>

                                    <!-- Destaques -->
                                    <table role=""presentation"" width=""600"" cellspacing=""0"" cellpadding=""15"" border=""0"" style=""background-color: #ffffff; margin-top: 10px;"">
                                        <tr>
                                            <td align=""center"" style=""background-color: #f8f9fa; padding: 15px;"">
                                                <h3 style=""color: #333; margin: 5px 0;"">📌 Destaque do Mês</h3>
                                                <p style=""color: #555; font-size: 14px; margin: 5px 0;"">Produto mais comprado: <strong>KIMI OXY </strong></p>
                                                <p style=""color: #555; font-size: 14px; margin: 5px 0;"">Faturamento total: <strong>R$ 12.350,00</strong></p>
                                            </td>
                                        </tr>
                                    </table>

                                    <!-- Máquinas Alugadas -->
                                    <table role=""presentation"" width=""600"" cellspacing=""0"" cellpadding=""15"" border=""0"" style=""background-color: #ffffff;"">
                                        <tr>
                                            <td align=""center"">
                                                <h1 style=""color: #007bff; margin: 0; font-size: 2rem;"">02</h1>
                                                <p style=""color: #555; font-size: 14px;"">Máquinas Alugadas</p>
                                            </td>
                                        </tr>
                                    </table>

                                    <!-- Rodapé -->
                                    <table role=""presentation"" width=""600"" cellspacing=""0"" cellpadding=""15"" border=""0"" style=""background-color: #262d61; color: #ffffff; border-radius: 0 0 8px 8px; margin-top: 20px;"">
                                        <tr>
                                            <td align=""center"">
                                                <img src=""https://www.proautokimium.com.br/images/2020/11/logo-proauto.png"" alt=""Proauto Kimium"" width=""120"" style=""opacity: 0.8;"">
                                                <p style=""margin: 5px 0; font-size: 14px;"">📞 (11) 99999-9999 | 📧 sac@proautokimium.com.br</p>
                                                <p style=""margin: 5px 0; font-size: 12px;"">Av João do Prado, 300 - Santo Andrré, SP</p>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                        </table>
                    </body>
                    </html>";
    }
}