namespace Gerenciador_De_Estoque
{
    partial class SellForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SellForm));
            this.sellListView = new System.Windows.Forms.ListView();
            this.NOME = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.QUANTIDADE = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.AddBtn = new System.Windows.Forms.Button();
            this.SellBtn = new System.Windows.Forms.Button();
            this.amountNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.priceTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.backBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.systemAmountTextBox = new System.Windows.Forms.TextBox();
            this.paioleiroTxt = new System.Windows.Forms.TextBox();
            this.recebedorTxt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ufTB = new System.Windows.Forms.TextBox();
            this.UF = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amountNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // sellListView
            // 
            this.sellListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NOME,
            this.UF,
            this.QUANTIDADE});
            this.sellListView.ContextMenuStrip = this.contextMenuStrip1;
            this.sellListView.FullRowSelect = true;
            this.sellListView.HideSelection = false;
            this.sellListView.Location = new System.Drawing.Point(644, 55);
            this.sellListView.Name = "sellListView";
            this.sellListView.Size = new System.Drawing.Size(598, 574);
            this.sellListView.TabIndex = 0;
            this.sellListView.UseCompatibleStateImageBehavior = false;
            this.sellListView.View = System.Windows.Forms.View.Details;
            this.sellListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.sellListView_MouseClick);
            // 
            // NOME
            // 
            this.NOME.Text = "NOME";
            this.NOME.Width = 440;
            // 
            // QUANTIDADE
            // 
            this.QUANTIDADE.Text = "QUANTIDADE";
            this.QUANTIDADE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.QUANTIDADE.Width = 92;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(122, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.toolStripMenuItem1.Text = "Remover";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // idTextBox
            // 
            this.idTextBox.Location = new System.Drawing.Point(24, 68);
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.Size = new System.Drawing.Size(268, 20);
            this.idTextBox.TabIndex = 0;
            this.idTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.idTextBox_KeyDown);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(24, 119);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(268, 20);
            this.nameTextBox.TabIndex = 10;
            this.nameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nameTextBox_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Codigo de Barras";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Nome";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 250);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Quantidade";
            // 
            // AddBtn
            // 
            this.AddBtn.Location = new System.Drawing.Point(172, 258);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(120, 33);
            this.AddBtn.TabIndex = 2;
            this.AddBtn.Text = "Adicionar";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // SellBtn
            // 
            this.SellBtn.Location = new System.Drawing.Point(772, 648);
            this.SellBtn.Name = "SellBtn";
            this.SellBtn.Size = new System.Drawing.Size(343, 31);
            this.SellBtn.TabIndex = 10;
            this.SellBtn.TabStop = false;
            this.SellBtn.Text = "Finalizar";
            this.SellBtn.UseVisualStyleBackColor = true;
            this.SellBtn.Click += new System.EventHandler(this.SellBtn_Click);
            // 
            // amountNumericUpDown
            // 
            this.amountNumericUpDown.AllowDrop = true;
            this.amountNumericUpDown.DecimalPlaces = 4;
            this.amountNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.amountNumericUpDown.Location = new System.Drawing.Point(24, 266);
            this.amountNumericUpDown.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.amountNumericUpDown.Name = "amountNumericUpDown";
            this.amountNumericUpDown.Size = new System.Drawing.Size(127, 20);
            this.amountNumericUpDown.TabIndex = 1;
            this.amountNumericUpDown.Enter += new System.EventHandler(this.amountNumericUpDown_Enter);
            // 
            // priceTextBox
            // 
            this.priceTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.priceTextBox.Location = new System.Drawing.Point(24, 173);
            this.priceTextBox.Name = "priceTextBox";
            this.priceTextBox.ReadOnly = true;
            this.priceTextBox.Size = new System.Drawing.Size(96, 20);
            this.priceTextBox.TabIndex = 10;
            this.priceTextBox.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Preço";
            // 
            // backBtn
            // 
            this.backBtn.Location = new System.Drawing.Point(24, 648);
            this.backBtn.Name = "backBtn";
            this.backBtn.Size = new System.Drawing.Size(97, 35);
            this.backBtn.TabIndex = 12;
            this.backBtn.Text = "Voltar";
            this.backBtn.UseVisualStyleBackColor = true;
            this.backBtn.Click += new System.EventHandler(this.backBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(175, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Quantidade no Sistema";
            // 
            // systemAmountTextBox
            // 
            this.systemAmountTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.systemAmountTextBox.Location = new System.Drawing.Point(178, 173);
            this.systemAmountTextBox.Name = "systemAmountTextBox";
            this.systemAmountTextBox.ReadOnly = true;
            this.systemAmountTextBox.Size = new System.Drawing.Size(114, 20);
            this.systemAmountTextBox.TabIndex = 13;
            this.systemAmountTextBox.TabStop = false;
            // 
            // paioleiroTxt
            // 
            this.paioleiroTxt.Location = new System.Drawing.Point(24, 441);
            this.paioleiroTxt.Name = "paioleiroTxt";
            this.paioleiroTxt.Size = new System.Drawing.Size(268, 20);
            this.paioleiroTxt.TabIndex = 15;
            // 
            // recebedorTxt
            // 
            this.recebedorTxt.Location = new System.Drawing.Point(24, 496);
            this.recebedorTxt.Name = "recebedorTxt";
            this.recebedorTxt.Size = new System.Drawing.Size(268, 20);
            this.recebedorTxt.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 480);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Recebedor";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Cozinha",
            "PROFESP",
            "Copa CMD",
            "Praça D\'Armas",
            "Peninsula",
            "OUTROS"});
            this.comboBox1.Location = new System.Drawing.Point(24, 383);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(159, 21);
            this.comboBox1.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 425);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Paioleiro";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 367);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "Setor";
            // 
            // ufTB
            // 
            this.ufTB.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ufTB.Location = new System.Drawing.Point(298, 119);
            this.ufTB.Name = "ufTB";
            this.ufTB.ReadOnly = true;
            this.ufTB.Size = new System.Drawing.Size(32, 20);
            this.ufTB.TabIndex = 22;
            this.ufTB.TabStop = false;
            // 
            // UF
            // 
            this.UF.Text = "UF";
            this.UF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SellForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.ClientSize = new System.Drawing.Size(1284, 701);
            this.Controls.Add(this.ufTB);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.recebedorTxt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.paioleiroTxt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.systemAmountTextBox);
            this.Controls.Add(this.backBtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.priceTextBox);
            this.Controls.Add(this.amountNumericUpDown);
            this.Controls.Add(this.SellBtn);
            this.Controls.Add(this.AddBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.idTextBox);
            this.Controls.Add(this.sellListView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SellForm";
            this.Text = "Formulario de Venda";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.amountNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView sellListView;
        private System.Windows.Forms.ColumnHeader NOME;
        private System.Windows.Forms.ColumnHeader QUANTIDADE;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.Button SellBtn;
        private System.Windows.Forms.NumericUpDown amountNumericUpDown;
        private System.Windows.Forms.TextBox priceTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button backBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox systemAmountTextBox;
        private System.Windows.Forms.TextBox paioleiroTxt;
        private System.Windows.Forms.TextBox recebedorTxt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.TextBox ufTB;
        private System.Windows.Forms.ColumnHeader UF;
    }
}