using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace Gerenciador_De_Estoque
{
    /// <summary>
    /// Enumeration defining the possible types of inventory reports.
    /// </summary>
    public enum ReportType
    {
        Sales, // Report for sales/disbursements
        FullStock, // Report showing all current stock
        Expiring, // Report for products close to expiration date
        CriticalStock // Report for products with low stock levels
    }

    /// <summary>
    /// Class responsible for generating PDF documents using the MigraDoc library.
    /// </summary>
    public class PDFGenerator
    {
        /// <summary>
        /// Generates a PDF report based on the specified report type and product data.
        /// </summary>
        public void GenerateReport(List<Product> products, string sector, string paioleiro, string recebedor, ReportType type)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // Set file dialog properties
                saveFileDialog.Filter = "Arquivo PDF (*.pdf)|*.pdf";
                saveFileDialog.Title = "Salvar Relatório";

                // Set default file name based on the report type
                switch (type)
                {
                    case ReportType.CriticalStock:
                        saveFileDialog.FileName = "Relatorio_Estoque_Critico.pdf";
                        break;
                    case ReportType.Expiring:
                        saveFileDialog.FileName = "Relatorio_Genero_a_vencer.pdf";
                        break;
                    case ReportType.Sales:
                        saveFileDialog.FileName = "Relatorio_Venda.pdf";
                        break;
                    case ReportType.FullStock:
                        saveFileDialog.FileName = "Relatorio_Estoque_Disponivel.pdf";
                        break;
                }

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Create a new MigraDoc document
                        var document = new Document();

                        // Set document title based on report type
                        switch (type)
                        {
                            case ReportType.CriticalStock:
                                document.Info.Title = "Relatorio_Estoque_Critico.pdf";
                                break;
                            case ReportType.Expiring:
                                document.Info.Title = "Relatorio_Genero_a_vencer.pdf";
                                break;
                            case ReportType.Sales:
                                document.Info.Title = "Relatorio_Venda.pdf";
                                break;
                            case ReportType.FullStock:
                                document.Info.Title = "Relatorio_Estoque_Disponivel.pdf";
                                break;
                        }

                        document.Info.Author = "Gerenciador de Estoque";

                        // Add a new section to the document
                        var section = document.AddSection();

                        // --- Header Setup ---
                        string titleString = "";

                        // Determine the main report title text
                        switch (type)
                        {
                            case ReportType.CriticalStock:
                                titleString = "Estoque Crítico";
                                break;
                            case ReportType.Expiring:
                                titleString = "Gêneros Próximos da Validade";
                                break;
                            case ReportType.Sales:
                                titleString = "Relatório de Venda";
                                break;
                            case ReportType.FullStock:
                                titleString = "Estoque Disponível";
                                break;
                        }

                        // Add and format the main title
                        var title = section.AddParagraph(titleString);
                        title.Format.Font.Size = 16;
                        title.Format.Font.Bold = true;
                        title.Format.Alignment = ParagraphAlignment.Center;
                        title.Format.SpaceAfter = "0.5cm";

                        // Add the generation date and time
                        var date = section.AddParagraph($"Data: {DateTime.Now:dd/MM/yyyy HH:mm}");
                        date.Format.SpaceAfter = "0.5cm";

                        // --- Report Content Generation ---
                        switch (type)
                        {
                            case ReportType.Sales:
                                BuildSalesTable(section, products, sector, paioleiro, recebedor);
                                break;

                            case ReportType.FullStock:
                                BuildFullStockTable(section, products);
                                break;

                            case ReportType.Expiring:
                                BuildExpiringTable(section, products);
                                break;

                            case ReportType.CriticalStock:
                                BuildCriticalStockTable(section, products);
                                break;
                        }

                        // --- Footer Setup ---
                        var footer = section.AddParagraph($"\nRelatório gerado por Gerenciador de Estoque");
                        footer.Format.Font.Size = 9;
                        footer.Format.Alignment = ParagraphAlignment.Center;

                        // --- PDF Rendering and Saving ---
                        PdfDocumentRenderer renderer = new PdfDocumentRenderer(true)
                        {
                            Document = document
                        };
                        renderer.RenderDocument();
                        renderer.PdfDocument.Save(saveFileDialog.FileName);

                        MessageBox.Show("Relatório gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // --- Automatic PDF Opening (Attempt 1) ---
                        try
                        {
                            var psi = new ProcessStartInfo(saveFileDialog.FileName)
                            {
                                UseShellExecute = true
                            };
                            Process.Start(psi);
                        }
                        catch (Exception exOpen)
                        {
                            MessageBox.Show("Não foi possível abrir o PDF automaticamente: " + exOpen.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        // --- Automatic PDF Opening (Attempt 2 - Duplicate, kept as is per request) ---
                        try
                        {
                            var psi = new ProcessStartInfo(saveFileDialog.FileName)
                            {
                                UseShellExecute = true
                            };
                            Process.Start(psi);
                        }
                        catch (Exception exOpen)
                        {
                            MessageBox.Show("Não foi possível abrir o PDF automaticamente: " + exOpen.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions during PDF generation
                        MessageBox.Show("Erro ao gerar PDF: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        /// <summary>
        /// Builds a sales table within the specified section, displaying product details, totals, and signatures.
        /// </summary>
        private void BuildSalesTable(Section section, List<Product> products, string sector, string paioleiro, string recebedor)
        {
            // Sector name at the top
            var setorParagrafo = section.AddParagraph($"Setor: {sector}");
            setorParagrafo.Format.Font.Size = 12;
            setorParagrafo.Format.Font.Bold = true;
            setorParagrafo.Format.Alignment = ParagraphAlignment.Center;
            setorParagrafo.Format.SpaceAfter = "0.5cm";

            // Create the table with defined column widths and headers
            var table = CreateBaseTable(section, new[] { "4cm", "5cm", "3cm", "3cm", "3cm" },
                new[] { "Código de Barras", "Produto", "Qtd.", "Valor Unit.", "Valor Total" });

            decimal totalGeral = 0; // Initialize grand total

            // Populate table rows with product data
            foreach (var prod in products)
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph(prod.Barcode);
                row.Cells[1].AddParagraph(prod.Name);
                row.Cells[2].AddParagraph(prod.Amount.ToString());
                row.Cells[3].AddParagraph(prod.Value.ToString("C")); // Format value as currency

                decimal total = prod.Amount * prod.Value;
                totalGeral += total;
                row.Cells[4].AddParagraph(total.ToString("C")); // Format total as currency
            }

            // Add the grand total row
            var totalRow = table.AddRow();
            totalRow.Shading.Color = Colors.LightYellow;
            totalRow.Cells[0].MergeRight = 3; // Merge the first 4 cells
            totalRow.Cells[0].AddParagraph("TOTAL:");
            totalRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            totalRow.Cells[4].AddParagraph(totalGeral.ToString("C"));

            // Space before signatures
            section.AddParagraph().Format.SpaceBefore = "2cm";

            // Create a table for the signature block
            var assinaturaTable = section.AddTable();
            assinaturaTable.AddColumn("7cm"); // Column for Recebedor
            assinaturaTable.AddColumn("7cm"); // Column for Paioleiro

            var assinaturaRow = assinaturaTable.AddRow();

            // Left Column: Recebedor (Receiver)
            var recebedorPar = assinaturaRow.Cells[0].AddParagraph($"{recebedor}\nRecebedor");
            recebedorPar.Format.Alignment = ParagraphAlignment.Left;
            recebedorPar.Format.Font.Size = 11;
            recebedorPar.Format.Font.Bold = true;

            // Right Column: Paioleiro (Stock Manager)
            var paioleiroPar = assinaturaRow.Cells[1].AddParagraph($"{paioleiro}\nPaioleiro");
            paioleiroPar.Format.Alignment = ParagraphAlignment.Right;
            paioleiroPar.Format.Font.Size = 11;
            paioleiroPar.Format.Font.Bold = true;
        }

        /// <summary>
        /// Builds a complete stock table within the specified section using the provided list of products.
        /// </summary>
        private void BuildFullStockTable(Section section, List<Product> products)
        {
            // Create the table with defined column widths and headers
            var table = CreateBaseTable(section, new[] { "2cm", "6cm", "3cm", "3cm", "4cm" },
                new[] { "Item Nº", "Produto", "Qtd.", "Valor Unit.", "Validade" });

            int counter = 1; // Item numbering

            // Populate table rows with all product data
            foreach (var prod in products)
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph(counter.ToString());
                row.Cells[1].AddParagraph(prod.Name);
                row.Cells[2].AddParagraph(prod.Amount.ToString());
                row.Cells[3].AddParagraph(prod.Value.ToString("C")); // Format value as currency
                row.Cells[4].AddParagraph(prod.Validate.ToString("dd/MM/yyyy")); // Format date
                counter++;
            }
        }

        /// <summary>
        /// Builds a table within the specified section to display products that are expiring within the next 90 days.
        /// </summary>
        private void BuildExpiringTable(Section section, List<Product> products)
        {
            // Create the table with defined column widths and headers
            var table = CreateBaseTable(section, new[] { "2cm", "6cm", "3cm", "3cm", "4cm" },
                new[] { "Item Nº", "Produto", "Qtd.", "Valor Unit.", "Validade" });

            int counter = 1; // Item numbering

            // Iterate and filter products for the report
            foreach (var prod in products)
            {
                // This condition check is redundant if the input 'products' list is already filtered, 
                // but kept for completeness based on the internal logic found in the original method.
                if (prod.Validate <= DateTime.Now.AddDays(90)) // Check expiration within the next 90 days
                {
                    var row = table.AddRow();
                    row.Cells[0].AddParagraph(counter.ToString());
                    row.Cells[1].AddParagraph(prod.Name);
                    row.Cells[2].AddParagraph(prod.Amount.ToString());
                    row.Cells[3].AddParagraph(prod.Value.ToString("C")); // Format value as currency
                    row.Cells[4].AddParagraph(prod.Validate.ToString("dd/MM/yyyy")); // Format date
                    counter++;
                }
            }
        }

        /// <summary>
        /// Builds a table within the specified section to display products that are at or below their critical stock levels.
        /// </summary>
        private void BuildCriticalStockTable(Section section, List<Product> products)
        {
            // Create the table with defined column widths and headers
            var table = CreateBaseTable(section, new[] { "2cm", "6cm", "3cm", "3cm", "4cm" },
                new[] { "Item Nº", "Produto", "Qtd.", "Valor Unit.", "Validade" });

            int counter = 1; // Item numbering

            // Iterate and filter products for the report
            foreach (var prod in products)
            {
                // This condition check is redundant if the input 'products' list is already filtered, 
                // but kept for completeness based on the internal logic found in the original method.
                if (prod.Amount <= prod.minStock) // Check if current amount is less than or equal to minimum stock
                {
                    var row = table.AddRow();
                    row.Cells[0].AddParagraph(counter.ToString());
                    row.Cells[1].AddParagraph(prod.Name);
                    row.Cells[2].AddParagraph(prod.Amount.ToString());
                    row.Cells[3].AddParagraph(prod.Value.ToString("C")); // Format value as currency
                    row.Cells[4].AddParagraph(prod.Validate.ToString("dd/MM/yyyy")); // Format date
                    counter++;
                }
            }
        }

        /// <summary>
        /// Creates and initializes a basic table structure (borders, alignment, columns, and header row).
        /// </summary>
        private Table CreateBaseTable(Section section, string[] columnWidths, string[] headers)
        {
            var table = section.AddTable();
            table.Borders.Width = 0.75;
            table.Borders.Color = Colors.Black;
            table.Format.Alignment = ParagraphAlignment.Center;

            // Define column widths
            foreach (var width in columnWidths)
                table.AddColumn(width);

            // Add and format the header row
            var headerRow = table.AddRow();
            headerRow.Shading.Color = Colors.LightGray;
            headerRow.Format.Font.Bold = true;

            // Populate header cells
            for (int i = 0; i < headers.Length; i++)
                headerRow.Cells[i].AddParagraph(headers[i]);

            return table;
        }
    }
}