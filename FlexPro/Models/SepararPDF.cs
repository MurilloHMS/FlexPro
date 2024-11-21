using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Collections.Generic;
using System.IO;

namespace FlexPro.Models
{
    public class SepararPDF
    {
        public string Nome { get; set; }

        public static List<SepararPDF> GetSeparatePdfByPage(string inputPdfPath)
        {
            if (!File.Exists(inputPdfPath))
                return new List<SepararPDF>();

            var inputDocument = PdfReader.Open(inputPdfPath, PdfDocumentOpenMode.Import);
            var archives = new List<SepararPDF>();

            for (int i = 0; i < inputDocument.PageCount; i++)
            {
                archives.Add(new SepararPDF { Nome = $"Pagina {i + 1}" });
            }

            return archives;
        }

        public static void SeparatedPdfByPage(string inputPdfPath, string outputFolder, List<SepararPDF> lista)
        {
            if (!File.Exists(inputPdfPath)) return;

            // Open the document with Import mode
            using (var inputDocument = PdfReader.Open(inputPdfPath, PdfDocumentOpenMode.Import))
            {
                for (int pageNumber = 0; pageNumber < inputDocument.PageCount; pageNumber++)
                {
                    // Create a new document for each page
                    using (var outputDocument = new PdfDocument())
                    {
                        // Import the page
                        var page = inputDocument.Pages[pageNumber];
                        // Create a new page in the output document with the same size and properties
                        var newPage = outputDocument.AddPage(page);

                        // Optionally, you can copy other attributes from the page here if necessary

                        // Define the output file path
                        string outputFilePath = Path.Combine(outputFolder, $"{lista[pageNumber].Nome}.pdf");

                        // Save the output document
                        outputDocument.Save(outputFilePath);
                    }
                }
            }
        }
    }
}
