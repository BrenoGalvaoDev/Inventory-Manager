using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace Gerenciador_De_Estoque
{
    public class PDFGenerator
    {
        public void GeneratePDFReport(List<Product> products)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Arquivo PDF (*.pdf)|*.pdf";
                saveFileDialog.Title = "Salvar Relatório de Vendas";
                saveFileDialog.FileName = "RelatorioVendas.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var document = new Document();
                        document.Info.Title = "Relatório de Vendas";
                        document.Info.Subject = "Relatório de Vendas";
                        document.Info.Author = "Gerenciador de Estoque";

                        var section = document.AddSection();

                        // Título
                        var title = section.AddParagraph("Relatório de Vendas");
                        title.Format.Font.Size = 16;
                        title.Format.Font.Bold = true;
                        title.Format.Alignment = ParagraphAlignment.Center;
                        title.Format.SpaceAfter = "1cm";

                        // Data
                        var date = section.AddParagraph($"Data: {DateTime.Now:dd/MM/yyyy HH:mm}");
                        date.Format.SpaceAfter = "0.5cm";

                        // Tabela
                        var table = section.AddTable();
                        table.Borders.Width = 0.75;
                        table.Borders.Color = Colors.Black;
                        table.Format.Alignment = ParagraphAlignment.Center;

                        // Definir colunas
                        table.AddColumn("4cm"); // Código de Barras
                        table.AddColumn("5cm"); // Nome
                        table.AddColumn("3cm"); // Quantidade
                        table.AddColumn("3cm"); // Valor Unitário
                        table.AddColumn("3cm"); // Valor Total

                        // Cabeçalho
                        var header = table.AddRow();
                        header.Shading.Color = Colors.LightGray;
                        header.HeadingFormat = true;
                        header.Format.Font.Bold = true;

                        header.Cells[0].AddParagraph("Código de Barras");
                        header.Cells[1].AddParagraph("Produto");
                        header.Cells[2].AddParagraph("Qtd.");
                        header.Cells[3].AddParagraph("Valor Unit.");
                        header.Cells[4].AddParagraph("Valor Total");

                        decimal totalGeral = 0;

                        // Dados
                        foreach (var prod in products)
                        {
                            var row = table.AddRow();
                            row.Cells[0].AddParagraph(prod.Barcode);
                            row.Cells[1].AddParagraph(prod.Name);
                            row.Cells[2].AddParagraph(prod.Amount.ToString());
                            row.Cells[3].AddParagraph(prod.Value.ToString("C"));

                            decimal total = prod.Amount * prod.Value;
                            totalGeral += total;
                            row.Cells[4].AddParagraph(total.ToString("C"));
                        }

                        // Linha de total geral
                        var totalRow = table.AddRow();
                        totalRow.Shading.Color = Colors.LightYellow;
                        totalRow.Cells[0].MergeRight = 3;
                        totalRow.Cells[0].AddParagraph("TOTAL GERAL:");
                        totalRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                        totalRow.Cells[4].AddParagraph(totalGeral.ToString("C"));

                        // Rodapé
                        var footer = section.AddParagraph($"\nRelatório gerado por Gerenciador de Estoque");
                        footer.Format.Font.Size = 9;
                        footer.Format.Alignment = ParagraphAlignment.Center;

                        // Gerar PDF
                        var renderer = new PdfDocumentRenderer(true)
                        {
                            Document = document
                        };
                        renderer.RenderDocument();
                        renderer.PdfDocument.Save(saveFileDialog.FileName);

                        MessageBox.Show("Relatório gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao gerar PDF: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
