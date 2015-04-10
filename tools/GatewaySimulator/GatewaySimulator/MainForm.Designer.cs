namespace GatewaySimulator
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.LabelRegisterStatus = new System.Windows.Forms.Label();
            this.ButtonReplace = new System.Windows.Forms.Button();
            this.ButtonRegister = new System.Windows.Forms.Button();
            this.TextBoxBoxMac = new System.Windows.Forms.TextBox();
            this.TextBoxBoxName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.TextBoxHierarchyText = new System.Windows.Forms.RichTextBox();
            this.LabelHierarchyStatus = new System.Windows.Forms.Label();
            this.ButtonHierarchyUpload = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.OutputText = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 96F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(915, 505);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(21, 18);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(872, 468);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.LabelRegisterStatus);
            this.tabPage1.Controls.Add(this.ButtonReplace);
            this.tabPage1.Controls.Add(this.ButtonRegister);
            this.tabPage1.Controls.Add(this.TextBoxBoxMac);
            this.tabPage1.Controls.Add(this.TextBoxBoxName);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(864, 442);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "注册";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // LabelRegisterStatus
            // 
            this.LabelRegisterStatus.AutoSize = true;
            this.LabelRegisterStatus.ForeColor = System.Drawing.Color.Red;
            this.LabelRegisterStatus.Location = new System.Drawing.Point(126, 122);
            this.LabelRegisterStatus.Name = "LabelRegisterStatus";
            this.LabelRegisterStatus.Size = new System.Drawing.Size(35, 13);
            this.LabelRegisterStatus.TabIndex = 6;
            this.LabelRegisterStatus.Text = "label4";
            // 
            // ButtonReplace
            // 
            this.ButtonReplace.Location = new System.Drawing.Point(374, 71);
            this.ButtonReplace.Name = "ButtonReplace";
            this.ButtonReplace.Size = new System.Drawing.Size(75, 23);
            this.ButtonReplace.TabIndex = 5;
            this.ButtonReplace.Text = "替换";
            this.ButtonReplace.UseVisualStyleBackColor = true;
            this.ButtonReplace.Click += new System.EventHandler(this.ButtonReplace_Click);
            // 
            // ButtonRegister
            // 
            this.ButtonRegister.Location = new System.Drawing.Point(374, 42);
            this.ButtonRegister.Name = "ButtonRegister";
            this.ButtonRegister.Size = new System.Drawing.Size(75, 23);
            this.ButtonRegister.TabIndex = 4;
            this.ButtonRegister.Text = "注册";
            this.ButtonRegister.UseVisualStyleBackColor = true;
            this.ButtonRegister.Click += new System.EventHandler(this.ButtonRegister_Click);
            // 
            // TextBoxBoxMac
            // 
            this.TextBoxBoxMac.Location = new System.Drawing.Point(168, 72);
            this.TextBoxBoxMac.Name = "TextBoxBoxMac";
            this.TextBoxBoxMac.Size = new System.Drawing.Size(187, 20);
            this.TextBoxBoxMac.TabIndex = 3;
            this.TextBoxBoxMac.Text = "2C-41-38-0D-E1-F5";
            // 
            // TextBoxBoxName
            // 
            this.TextBoxBoxName.Location = new System.Drawing.Point(168, 44);
            this.TextBoxBoxName.Name = "TextBoxBoxName";
            this.TextBoxBoxName.Size = new System.Drawing.Size(187, 20);
            this.TextBoxBoxName.TabIndex = 2;
            this.TextBoxBoxName.Text = "NancyCustomer1.Box1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(86, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "网关Mac地址";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(107, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "网关名称";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(864, 442);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "层级";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.TextBoxHierarchyText);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.LabelHierarchyStatus);
            this.splitContainer1.Panel2.Controls.Add(this.ButtonHierarchyUpload);
            this.splitContainer1.Size = new System.Drawing.Size(858, 436);
            this.splitContainer1.SplitterDistance = 557;
            this.splitContainer1.TabIndex = 0;
            // 
            // TextBoxHierarchyText
            // 
            this.TextBoxHierarchyText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBoxHierarchyText.Location = new System.Drawing.Point(0, 0);
            this.TextBoxHierarchyText.Name = "TextBoxHierarchyText";
            this.TextBoxHierarchyText.Size = new System.Drawing.Size(557, 436);
            this.TextBoxHierarchyText.TabIndex = 0;
            this.TextBoxHierarchyText.Text = resources.GetString("TextBoxHierarchyText.Text");
            // 
            // LabelHierarchyStatus
            // 
            this.LabelHierarchyStatus.AutoSize = true;
            this.LabelHierarchyStatus.ForeColor = System.Drawing.Color.Red;
            this.LabelHierarchyStatus.Location = new System.Drawing.Point(26, 51);
            this.LabelHierarchyStatus.Name = "LabelHierarchyStatus";
            this.LabelHierarchyStatus.Size = new System.Drawing.Size(35, 13);
            this.LabelHierarchyStatus.TabIndex = 1;
            this.LabelHierarchyStatus.Text = "label1";
            // 
            // ButtonHierarchyUpload
            // 
            this.ButtonHierarchyUpload.Location = new System.Drawing.Point(26, 21);
            this.ButtonHierarchyUpload.Name = "ButtonHierarchyUpload";
            this.ButtonHierarchyUpload.Size = new System.Drawing.Size(75, 23);
            this.ButtonHierarchyUpload.TabIndex = 0;
            this.ButtonHierarchyUpload.Text = "上传";
            this.ButtonHierarchyUpload.UseVisualStyleBackColor = true;
            this.ButtonHierarchyUpload.Click += new System.EventHandler(this.ButtonHierarchyUpload_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.OutputText);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(864, 442);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "输出";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // OutputText
            // 
            this.OutputText.BackColor = System.Drawing.Color.Black;
            this.OutputText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputText.ForeColor = System.Drawing.Color.Lime;
            this.OutputText.Location = new System.Drawing.Point(3, 3);
            this.OutputText.Name = "OutputText";
            this.OutputText.Size = new System.Drawing.Size(858, 436);
            this.OutputText.TabIndex = 0;
            this.OutputText.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 505);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox TextBoxHierarchyText;
        private System.Windows.Forms.Label LabelHierarchyStatus;
        private System.Windows.Forms.Button ButtonHierarchyUpload;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox OutputText;
        private System.Windows.Forms.TextBox TextBoxBoxMac;
        private System.Windows.Forms.TextBox TextBoxBoxName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ButtonReplace;
        private System.Windows.Forms.Button ButtonRegister;
        private System.Windows.Forms.Label LabelRegisterStatus;
    }
}