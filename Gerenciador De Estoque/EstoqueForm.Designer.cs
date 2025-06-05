namespace Gerenciador_De_Estoque
{
    partial class EstoqueForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EstoqueForm));
            this.stockListView = new System.Windows.Forms.ListView();
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NOME = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VALIDADE = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ESTOQUE_MINIMO = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.QUANTIDADE_ATUAL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renomearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.excluirProdutoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backBtn = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // stockListView
            // 
            this.stockListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.NOME,
            this.VALIDADE,
            this.ESTOQUE_MINIMO,
            this.QUANTIDADE_ATUAL});
            this.stockListView.ContextMenuStrip = this.contextMenuStrip1;
            this.stockListView.FullRowSelect = true;
            this.stockListView.GridLines = true;
            this.stockListView.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.StateImageIndex = 0;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.StateImageIndex = 0;
            this.stockListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5});
            this.stockListView.Location = new System.Drawing.Point(129, 12);
            this.stockListView.Name = "stockListView";
            this.stockListView.Size = new System.Drawing.Size(1127, 603);
            this.stockListView.TabIndex = 0;
            this.stockListView.UseCompatibleStateImageBehavior = false;
            this.stockListView.View = System.Windows.Forms.View.Details;
            this.stockListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.stockListView_MouseClick);
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 116;
            // 
            // NOME
            // 
            this.NOME.Text = "NOME";
            this.NOME.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NOME.Width = 658;
            // 
            // VALIDADE
            // 
            this.VALIDADE.Text = "VALIDADE";
            this.VALIDADE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.VALIDADE.Width = 106;
            // 
            // ESTOQUE_MINIMO
            // 
            this.ESTOQUE_MINIMO.Text = "ESTOQUE MINIMO";
            this.ESTOQUE_MINIMO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ESTOQUE_MINIMO.Width = 121;
            // 
            // QUANTIDADE_ATUAL
            // 
            this.QUANTIDADE_ATUAL.Text = "QUANTIDADE ATUAL";
            this.QUANTIDADE_ATUAL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.QUANTIDADE_ATUAL.Width = 123;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renomearToolStripMenuItem,
            this.excluirProdutoToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(175, 48);
            // 
            // renomearToolStripMenuItem
            // 
            this.renomearToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1});
            this.renomearToolStripMenuItem.Name = "renomearToolStripMenuItem";
            this.renomearToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.renomearToolStripMenuItem.Text = "Renomear Produto";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox1_KeyDown);
            // 
            // excluirProdutoToolStripMenuItem
            // 
            this.excluirProdutoToolStripMenuItem.Name = "excluirProdutoToolStripMenuItem";
            this.excluirProdutoToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.excluirProdutoToolStripMenuItem.Text = "Excluir Produto";
            this.excluirProdutoToolStripMenuItem.Click += new System.EventHandler(this.excluirProdutoToolStripMenuItem_Click);
            // 
            // backBtn
            // 
            this.backBtn.Location = new System.Drawing.Point(1210, 668);
            this.backBtn.Name = "backBtn";
            this.backBtn.Size = new System.Drawing.Size(128, 49);
            this.backBtn.TabIndex = 1;
            this.backBtn.Text = "Voltar";
            this.backBtn.UseVisualStyleBackColor = true;
            this.backBtn.Click += new System.EventHandler(this.backBtn_Click);
            // 
            // EstoqueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.backBtn);
            this.Controls.Add(this.stockListView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EstoqueForm";
            this.Text = "Estoque";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView stockListView;
        private System.Windows.Forms.Button backBtn;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader NOME;
        private System.Windows.Forms.ColumnHeader VALIDADE;
        private System.Windows.Forms.ColumnHeader ESTOQUE_MINIMO;
        private System.Windows.Forms.ColumnHeader QUANTIDADE_ATUAL;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem renomearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excluirProdutoToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
    }
}