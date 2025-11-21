using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace Gerenciador_De_Estoque
{
    public enum ReportType
    {
        Sales,
        FullStock,
        Expiring,
        CriticalStock
    }

    public class PDFGenerator
    {
        // Agora recebe setor, paioleiro e recebedor
        public void GenerateReport(List<Product> products, string sector, string paioleiro, string recebedor, ReportType type)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Arquivo PDF (*.pdf)|*.pdf";
                saveFileDialog.Title = "Salvar Relatório";

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
                        var document = new Document();

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

                        var section = document.AddSection();

                        // Cabeçalho
                        string titleString = "";

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

                        var title = section.AddParagraph(titleString);
                        title.Format.Font.Size = 16;
                        title.Format.Font.Bold = true;
                        title.Format.Alignment = ParagraphAlignment.Center;
                        title.Format.SpaceAfter = "0.5cm";

                        var date = section.AddParagraph($"Data: {DateTime.Now:dd/MM/yyyy HH:mm}");
                        date.Format.SpaceAfter = "0.5cm";

                        // Conteúdo específico do relatório
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

                        // Rodapé padrão
                        var footer = section.AddParagraph($"\nRelatório gerado por Gerenciador de Estoque");
                        footer.Format.Font.Size = 9;
                        footer.Format.Alignment = ParagraphAlignment.Center;

                        // Renderização
                        PdfDocumentRenderer renderer = new PdfDocumentRenderer(true)
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

        // ----------------- RELATÓRIOS ESPECÍFICOS -----------------

        // Agora adaptado para Setor, Paioleiro e Recebedor
        private void BuildSalesTable(Section section, List<Product> products, string sector, string paioleiro, string recebedor)
        {
            // Nome do setor no topo
            var setorParagrafo = section.AddParagraph($"Setor: {sector}");
            setorParagrafo.Format.Font.Size = 12;
            setorParagrafo.Format.Font.Bold = true;
            setorParagrafo.Format.Alignment = ParagraphAlignment.Center;
            setorParagrafo.Format.SpaceAfter = "0.5cm";

            var table = CreateBaseTable(section, new[] { "4cm", "5cm", "3cm", "3cm", "3cm" },
                new[] { "Código de Barras", "Produto", "Qtd.", "Valor Unit.", "Valor Total" });

            decimal totalGeral = 0;

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

            var totalRow = table.AddRow();
            totalRow.Shading.Color = Colors.LightYellow;
            totalRow.Cells[0].MergeRight = 3;
            totalRow.Cells[0].AddParagraph("TOTAL:");
            totalRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            totalRow.Cells[4].AddParagraph(totalGeral.ToString("C"));

            // Espaço antes das assinaturas
            section.AddParagraph().Format.SpaceBefore = "2cm";

            // Criação da "linha" com paioleiro e recebedor nas extremidades
            var assinaturaTable = section.AddTable();
            assinaturaTable.AddColumn("7cm");
            assinaturaTable.AddColumn("7cm");

            var assinaturaRow = assinaturaTable.AddRow();

            // Coluna esquerda: Recebedor
            var recebedorPar = assinaturaRow.Cells[0].AddParagraph($"{recebedor}\nRecebedor");
            recebedorPar.Format.Alignment = ParagraphAlignment.Left;
            recebedorPar.Format.Font.Size = 11;
            recebedorPar.Format.Font.Bold = true;

            // Coluna direita: Paioleiro
            var paioleiroPar = assinaturaRow.Cells[1].AddParagraph($"{paioleiro}\nPaioleiro");
            paioleiroPar.Format.Alignment = ParagraphAlignment.Right;
            paioleiroPar.Format.Font.Size = 11;
            paioleiroPar.Format.Font.Bold = true;
        }

        private void BuildFullStockTable(Section section, List<Product> products)
        {
            var table = CreateBaseTable(section, new[] { "2cm", "6cm", "3cm", "3cm", "4cm" },
                new[] { "Item Nº", "Produto", "Qtd.", "Valor Unit.", "Validade" });

            int counter = 1;
            foreach (var prod in products)
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph(counter.ToString());
                row.Cells[1].AddParagraph(prod.Name);
                row.Cells[2].AddParagraph(prod.Amount.ToString());
                row.Cells[3].AddParagraph(prod.Value.ToString("C"));
                row.Cells[4].AddParagraph(prod.Validate.ToString("dd/MM/yyyy"));
                counter++;
            }
        }

        private void BuildExpiringTable(Section section, List<Product> products)
        {
            var table = CreateBaseTable(section, new[] { "2cm", "6cm", "3cm", "3cm", "4cm" },
                new[] { "Item Nº", "Produto", "Qtd.", "Valor Unit.", "Validade" });

            int counter = 1;
            foreach (var prod in products)
            {
                if (prod.Validate <= DateTime.Now.AddDays(90)) // próximos 90 dias
                {
                    var row = table.AddRow();
                    row.Cells[0].AddParagraph(counter.ToString());
                    row.Cells[1].AddParagraph(prod.Name);
                    row.Cells[2].AddParagraph(prod.Amount.ToString());
                    row.Cells[3].AddParagraph(prod.Value.ToString("C"));
                    row.Cells[4].AddParagraph(prod.Validate.ToString("dd/MM/yyyy"));
                    counter++;
                }
            }
        }

        private void BuildCriticalStockTable(Section section, List<Product> products)
        {
            var table = CreateBaseTable(section, new[] { "2cm", "6cm", "3cm", "3cm", "4cm" },
                new[] { "Item Nº", "Produto", "Qtd.", "Valor Unit.", "Validade" });

            int counter = 1;
            foreach (var prod in products)
            {
                if (prod.Amount <= prod.minStock)
                {
                    var row = table.AddRow();
                    row.Cells[0].AddParagraph(counter.ToString());
                    row.Cells[1].AddParagraph(prod.Name);
                    row.Cells[2].AddParagraph(prod.Amount.ToString());
                    row.Cells[3].AddParagraph(prod.Value.ToString("C"));
                    row.Cells[4].AddParagraph(prod.Validate.ToString("dd/MM/yyyy"));
                    counter++;
                }
            }
        }

        // ----------------- MÉTODO BASE PARA CRIAR TABELAS -----------------

        private Table CreateBaseTable(Section section, string[] columnWidths, string[] headers)
        {
            var table = section.AddTable();
            table.Borders.Width = 0.75;
            table.Borders.Color = Colors.Black;
            table.Format.Alignment = ParagraphAlignment.Center;

            foreach (var width in columnWidths)
                table.AddColumn(width);

            var headerRow = table.AddRow();
            headerRow.Shading.Color = Colors.LightGray;
            headerRow.Format.Font.Bold = true;

            for (int i = 0; i < headers.Length; i++)
                headerRow.Cells[i].AddParagraph(headers[i]);

            return table;
        }
    }
}
