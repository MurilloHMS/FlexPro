using System;
using System.Collections.Generic;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using UglyToad.PdfPig;

namespace FlexPro.Models
{
    public class SepararPDF
    {
        public string Nome { get; set; }

        public static List<SepararPDF> GetSeparatePdfByPage(string inputPdfPath)
        {
            if (!File.Exists(inputPdfPath))
                return new List<SepararPDF>();

            var archives = new List<SepararPDF>();

            using (var document = UglyToad.PdfPig.PdfDocument.Open(inputPdfPath))
            {
                int pageIndex = 0;
                foreach (var page in document.GetPages())
                {
                    string textPagina = page.Text;
                    string nomeFuncionario = ExtrairNomeFuncionario(textPagina);

                    archives.Add(new SepararPDF
                    {
                        Nome = !string.IsNullOrEmpty(nomeFuncionario) ? nomeFuncionario : $"Página {pageIndex + 1}"
                    });

                    pageIndex++;
                }
            }
            return archives;
        }

        public static void SeparatedPdfByPage(string inputPdfPath, string outputFolder, List<SepararPDF> lista)
        {
            if (!File.Exists(inputPdfPath) || lista == null || lista.Count == 0)
                return;

            Directory.CreateDirectory(outputFolder);

            using (var inputDocument = PdfReader.Open(inputPdfPath, PdfDocumentOpenMode.Import))
            {
                for (int pageNumber = 0; pageNumber < inputDocument.PageCount; pageNumber++)
                {
                    using (var outputDocument = new PdfSharp.Pdf.PdfDocument())
                    {
                        outputDocument.AddPage(inputDocument.Pages[pageNumber]);

                        string nomeArquivo = SanitizeFileName(lista[pageNumber].Nome);
                        string outputFilePath = Path.Combine(outputFolder, $"{nomeArquivo}.pdf");

                        outputDocument.Save(outputFilePath);
                    }
                }
            }
        }

        private static string ExtrairNomeFuncionario(string texto)
        {
            string chave = "FL";
            int indexNome = texto.IndexOf(chave, StringComparison.OrdinalIgnoreCase);

            if (indexNome != -1)
            {
                int inicio = indexNome + chave.Length;
                string restante = texto.Substring(inicio).Trim();
                string[] partes = restante.Split(' ');

                List<string> nomePartes = new List<string>();
                bool encontrouNome = false;
        
                foreach (string parte in partes)
                {
                    if (!encontrouNome && int.TryParse(parte, out _))
                        continue; 
            
                    encontrouNome = true;
            
                    if (int.TryParse(parte, out _)) 
                        break;
            
                    nomePartes.Add(parte);
                }

                return string.Join(" ", nomePartes).Trim();
            }

            return string.Empty;
        }

        private static string SanitizeFileName(string fileName)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c.ToString(), "_");
            }
            return fileName;
        }
    }
}
