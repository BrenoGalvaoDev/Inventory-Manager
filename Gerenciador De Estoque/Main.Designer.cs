namespace Gerenciador_De_Estoque
{
    partial class Main
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.openRegisterBtn = new System.Windows.Forms.Button();
            this.closeToDueDateListView = new System.Windows.Forms.ListView();
            this.Nome = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Validade = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Quantidade = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lowStockListView = new System.Windows.Forms.ListView();
            this._Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StockMin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Amount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.exitBtn = new System.Windows.Forms.Button();
            this.ShowStockBtn = new System.Windows.Forms.Button();
            this.sellFormBtn = new System.Windows.Forms.Button();
            this.expiringBtn = new System.Windows.Forms.Button();
            this.criticalStockBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openRegisterBtn
            // 
            this.openRegisterBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openRegisterBtn.Location = new System.Drawing.Point(1047, 27);
            this.openRegisterBtn.Name = "openRegisterBtn";
            this.openRegisterBtn.Size = new System.Drawing.Size(111, 72);
            this.openRegisterBtn.TabIndex = 0;
            this.openRegisterBtn.Text = "Adicionar Novo Item";
            this.openRegisterBtn.UseVisualStyleBackColor = true;
            this.openRegisterBtn.Click += new System.EventHandler(this.openRegisterBtn_Click);
            // 
            // closeToDueDateListView
            // 
            this.closeToDueDateListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Nome,
            this.Validade,
            this.Quantidade});
            this.closeToDueDateListView.FullRowSelect = true;
            this.closeToDueDateListView.HideSelection = false;
            this.closeToDueDateListView.Location = new System.Drawing.Point(12, 27);
            this.closeToDueDateListView.Name = "closeToDueDateListView";
            this.closeToDueDateListView.Size = new System.Drawing.Size(755, 197);
            this.closeToDueDateListView.TabIndex = 1;
            this.closeToDueDateListView.UseCompatibleStateImageBehavior = false;
            this.closeToDueDateListView.View = System.Windows.Forms.View.Details;
            // 
            // Nome
            // 
            this.Nome.Text = "Nome";
            this.Nome.Width = 563;
            // 
            // Validade
            // 
            this.Validade.Text = "Validade";
            this.Validade.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Validade.Width = 87;
            // 
            // Quantidade
            // 
            this.Quantidade.Text = "Quantidade";
            this.Quantidade.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Quantidade.Width = 100;
            // 
            // lowStockListView
            // 
            this.lowStockListView.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lowStockListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._Name,
            this.StockMin,
            this.Amount});
            this.lowStockListView.FullRowSelect = true;
            this.lowStockListView.HideSelection = false;
            this.lowStockListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.lowStockListView.Location = new System.Drawing.Point(18, 310);
            this.lowStockListView.Name = "lowStockListView";
            this.lowStockListView.Size = new System.Drawing.Size(749, 214);
            this.lowStockListView.TabIndex = 2;
            this.lowStockListView.UseCompatibleStateImageBehavior = false;
            this.lowStockListView.View = System.Windows.Forms.View.Details;
            // 
            // _Name
            // 
            this._Name.Text = "Nome";
            this._Name.Width = 566;
            // 
            // StockMin
            // 
            this.StockMin.Text = "Min";
            this.StockMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.StockMin.Width = 78;
            // 
            // Amount
            // 
            this.Amount.Text = "Quantidade";
            this.Amount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Amount.Width = 100;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Itens Próximos da Validade";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 289);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Itens Com Estoque Critico";
            // 
            // exitBtn
            // 
            this.exitBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exitBtn.Location = new System.Drawing.Point(18, 614);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(86, 45);
            this.exitBtn.TabIndex = 5;
            this.exitBtn.Text = "Exit";
            this.exitBtn.UseVisualStyleBackColor = true;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // ShowStockBtn
            // 
            this.ShowStockBtn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ShowStockBtn.Location = new System.Drawing.Point(1047, 120);
            this.ShowStockBtn.Name = "ShowStockBtn";
            this.ShowStockBtn.Size = new System.Drawing.Size(111, 69);
            this.ShowStockBtn.TabIndex = 1;
            this.ShowStockBtn.Text = "Mostrar Estoque";
            this.ShowStockBtn.UseVisualStyleBackColor = true;
            this.ShowStockBtn.Click += new System.EventHandler(this.ShowStockBtn_Click);
            // 
            // sellFormBtn
            // 
            this.sellFormBtn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.sellFormBtn.Location = new System.Drawing.Point(1047, 210);
            this.sellFormBtn.Name = "sellFormBtn";
            this.sellFormBtn.Size = new System.Drawing.Size(111, 62);
            this.sellFormBtn.TabIndex = 2;
            this.sellFormBtn.Text = "Formulario de Venda";
            this.sellFormBtn.UseVisualStyleBackColor = true;
            this.sellFormBtn.Click += new System.EventHandler(this.sellFormBtn_Click);
            // 
            // expiringBtn
            // 
            this.expiringBtn.Location = new System.Drawing.Point(541, 230);
            this.expiringBtn.Name = "expiringBtn";
            this.expiringBtn.Size = new System.Drawing.Size(225, 30);
            this.expiringBtn.TabIndex = 6;
            this.expiringBtn.Text = "Relatorio de Itens Proximos de Vencer";
            this.expiringBtn.UseVisualStyleBackColor = true;
            this.expiringBtn.Click += new System.EventHandler(this.expiringBtn_Click);
            // 
            // criticalStockBtn
            // 
            this.criticalStockBtn.Location = new System.Drawing.Point(542, 546);
            this.criticalStockBtn.Name = "criticalStockBtn";
            this.criticalStockBtn.Size = new System.Drawing.Size(225, 30);
            this.criticalStockBtn.TabIndex = 7;
            this.criticalStockBtn.Text = "Relatorio de Itens com Estoque Critico";
            this.criticalStockBtn.UseVisualStyleBackColor = true;
            this.criticalStockBtn.Click += new System.EventHandler(this.criticalStockBtn_Click);
            // 
            // Main
            // 
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.ClientSize = new System.Drawing.Size(1284, 701);
            this.Controls.Add(this.criticalStockBtn);
            this.Controls.Add(this.expiringBtn);
            this.Controls.Add(this.sellFormBtn);
            this.Controls.Add(this.ShowStockBtn);
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lowStockListView);
            this.Controls.Add(this.closeToDueDateListView);
            this.Controls.Add(this.openRegisterBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "Gerenciador de Estoque";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Main_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private System.Windows.Forms.Button openRegisterBtn;
        //private System.Windows.Forms.ListView lowStockListView;
        private System.Windows.Forms.Button openRegisterBtn;
        private System.Windows.Forms.ListView closeToDueDateListView;
        private System.Windows.Forms.ListView lowStockListView;
        private System.Windows.Forms.ColumnHeader _Name;
        private System.Windows.Forms.ColumnHeader StockMin;
        private System.Windows.Forms.ColumnHeader Amount;
        private System.Windows.Forms.ColumnHeader Nome;
        private System.Windows.Forms.ColumnHeader Validade;
        private System.Windows.Forms.ColumnHeader Quantidade;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button exitBtn;
        private System.Windows.Forms.Button ShowStockBtn;
        private System.Windows.Forms.Button sellFormBtn;
        private System.Windows.Forms.Button expiringBtn;
        private System.Windows.Forms.Button criticalStockBtn;
    }
}

