namespace NinetyNine
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.파일ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.열기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.저장ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.빅테이블ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BigTable_Parsing_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BigTable_Mapping_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.도움말ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.라이센스ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ePPlusToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newtonsoftJsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabPage_BigTable = new System.Windows.Forms.TabPage();
            this.dataGridView_BigTable = new System.Windows.Forms.DataGridView();
            this.tabPage_Organization = new System.Windows.Forms.TabPage();
            this.dataGridView_Organization = new System.Windows.Forms.DataGridView();
            this.tabPage_Schedule = new System.Windows.Forms.TabPage();
            this.dataGridView_Schedule = new System.Windows.Forms.DataGridView();
            this.tabPage_Statement = new System.Windows.Forms.TabPage();
            this.dataGridView_Statement = new System.Windows.Forms.DataGridView();
            this.tabPage_Form = new System.Windows.Forms.TabPage();
            this.dataGridView_Form = new System.Windows.Forms.DataGridView();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage_Work = new System.Windows.Forms.TabPage();
            this.dataGridView_Work = new System.Windows.Forms.DataGridView();
            this.tabPage_Floor = new System.Windows.Forms.TabPage();
            this.dataGridView_Floor = new System.Windows.Forms.DataGridView();
            this.tabPage_What = new System.Windows.Forms.TabPage();
            this.dataGridView_What = new System.Windows.Forms.DataGridView();
            this.tabPage_How = new System.Windows.Forms.TabPage();
            this.dataGridView_How = new System.Windows.Forms.DataGridView();
            this.menuStrip.SuspendLayout();
            this.tabPage_BigTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_BigTable)).BeginInit();
            this.tabPage_Organization.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Organization)).BeginInit();
            this.tabPage_Schedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Schedule)).BeginInit();
            this.tabPage_Statement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Statement)).BeginInit();
            this.tabPage_Form.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Form)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPage_Work.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Work)).BeginInit();
            this.tabPage_Floor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Floor)).BeginInit();
            this.tabPage_What.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_What)).BeginInit();
            this.tabPage_How.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_How)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.파일ToolStripMenuItem,
            this.빅테이블ToolStripMenuItem,
            this.도움말ToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(700, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // 파일ToolStripMenuItem
            // 
            this.파일ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.열기ToolStripMenuItem,
            this.저장ToolStripMenuItem});
            this.파일ToolStripMenuItem.Name = "파일ToolStripMenuItem";
            this.파일ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.파일ToolStripMenuItem.Text = "파일";
            // 
            // 열기ToolStripMenuItem
            // 
            this.열기ToolStripMenuItem.Name = "열기ToolStripMenuItem";
            this.열기ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.열기ToolStripMenuItem.Text = "열기";
            this.열기ToolStripMenuItem.Click += new System.EventHandler(this.열기ToolStripMenuItem_Click);
            // 
            // 저장ToolStripMenuItem
            // 
            this.저장ToolStripMenuItem.Name = "저장ToolStripMenuItem";
            this.저장ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.저장ToolStripMenuItem.Text = "저장";
            this.저장ToolStripMenuItem.Click += new System.EventHandler(this.저장ToolStripMenuItem_Click);
            // 
            // 빅테이블ToolStripMenuItem
            // 
            this.빅테이블ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BigTable_Parsing_ToolStripMenuItem,
            this.BigTable_Mapping_ToolStripMenuItem});
            this.빅테이블ToolStripMenuItem.Name = "빅테이블ToolStripMenuItem";
            this.빅테이블ToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.빅테이블ToolStripMenuItem.Text = "빅테이블";
            // 
            // BigTable_Parsing_ToolStripMenuItem
            // 
            this.BigTable_Parsing_ToolStripMenuItem.Name = "BigTable_Parsing_ToolStripMenuItem";
            this.BigTable_Parsing_ToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.BigTable_Parsing_ToolStripMenuItem.Text = "1차 (Parsing)";
            this.BigTable_Parsing_ToolStripMenuItem.Click += new System.EventHandler(this.BigTable_Parsing_ToolStripMenuItem_Click);
            // 
            // BigTable_Mapping_ToolStripMenuItem
            // 
            this.BigTable_Mapping_ToolStripMenuItem.Name = "BigTable_Mapping_ToolStripMenuItem";
            this.BigTable_Mapping_ToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.BigTable_Mapping_ToolStripMenuItem.Text = "2차 (Mapping)";
            this.BigTable_Mapping_ToolStripMenuItem.Click += new System.EventHandler(this.BigTable_Mapping_ToolStripMenuItem_Click);
            // 
            // 도움말ToolStripMenuItem
            // 
            this.도움말ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.라이센스ToolStripMenuItem1});
            this.도움말ToolStripMenuItem.Name = "도움말ToolStripMenuItem";
            this.도움말ToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.도움말ToolStripMenuItem.Text = "도움말";
            // 
            // 라이센스ToolStripMenuItem1
            // 
            this.라이센스ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ePPlusToolStripMenuItem1,
            this.newtonsoftJsonToolStripMenuItem});
            this.라이센스ToolStripMenuItem1.Name = "라이센스ToolStripMenuItem1";
            this.라이센스ToolStripMenuItem1.Size = new System.Drawing.Size(122, 22);
            this.라이센스ToolStripMenuItem1.Text = "라이센스";
            // 
            // ePPlusToolStripMenuItem1
            // 
            this.ePPlusToolStripMenuItem1.Name = "ePPlusToolStripMenuItem1";
            this.ePPlusToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
            this.ePPlusToolStripMenuItem1.Text = "EPPlus";
            // 
            // newtonsoftJsonToolStripMenuItem
            // 
            this.newtonsoftJsonToolStripMenuItem.Name = "newtonsoftJsonToolStripMenuItem";
            this.newtonsoftJsonToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.newtonsoftJsonToolStripMenuItem.Text = "Newtonsoft.Json";
            // 
            // tabPage_BigTable
            // 
            this.tabPage_BigTable.Controls.Add(this.dataGridView_BigTable);
            this.tabPage_BigTable.Location = new System.Drawing.Point(4, 22);
            this.tabPage_BigTable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_BigTable.Name = "tabPage_BigTable";
            this.tabPage_BigTable.Size = new System.Drawing.Size(671, 261);
            this.tabPage_BigTable.TabIndex = 4;
            this.tabPage_BigTable.Text = "BigTable";
            this.tabPage_BigTable.UseVisualStyleBackColor = true;
            // 
            // dataGridView_BigTable
            // 
            this.dataGridView_BigTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_BigTable.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_BigTable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView_BigTable.Name = "dataGridView_BigTable";
            this.dataGridView_BigTable.RowTemplate.Height = 27;
            this.dataGridView_BigTable.Size = new System.Drawing.Size(210, 120);
            this.dataGridView_BigTable.TabIndex = 0;
            // 
            // tabPage_Organization
            // 
            this.tabPage_Organization.Controls.Add(this.dataGridView_Organization);
            this.tabPage_Organization.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Organization.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_Organization.Name = "tabPage_Organization";
            this.tabPage_Organization.Size = new System.Drawing.Size(671, 261);
            this.tabPage_Organization.TabIndex = 2;
            this.tabPage_Organization.Text = "Organization";
            this.tabPage_Organization.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Organization
            // 
            this.dataGridView_Organization.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Organization.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Organization.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView_Organization.Name = "dataGridView_Organization";
            this.dataGridView_Organization.RowTemplate.Height = 27;
            this.dataGridView_Organization.Size = new System.Drawing.Size(210, 120);
            this.dataGridView_Organization.TabIndex = 0;
            // 
            // tabPage_Schedule
            // 
            this.tabPage_Schedule.Controls.Add(this.dataGridView_Schedule);
            this.tabPage_Schedule.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Schedule.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_Schedule.Name = "tabPage_Schedule";
            this.tabPage_Schedule.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_Schedule.Size = new System.Drawing.Size(671, 261);
            this.tabPage_Schedule.TabIndex = 1;
            this.tabPage_Schedule.Text = "Schedule";
            this.tabPage_Schedule.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Schedule
            // 
            this.dataGridView_Schedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Schedule.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Schedule.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView_Schedule.Name = "dataGridView_Schedule";
            this.dataGridView_Schedule.RowTemplate.Height = 27;
            this.dataGridView_Schedule.Size = new System.Drawing.Size(210, 120);
            this.dataGridView_Schedule.TabIndex = 0;
            // 
            // tabPage_Statement
            // 
            this.tabPage_Statement.Controls.Add(this.dataGridView_Statement);
            this.tabPage_Statement.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Statement.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_Statement.Name = "tabPage_Statement";
            this.tabPage_Statement.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_Statement.Size = new System.Drawing.Size(671, 261);
            this.tabPage_Statement.TabIndex = 0;
            this.tabPage_Statement.Text = "Statement";
            this.tabPage_Statement.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Statement
            // 
            this.dataGridView_Statement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Statement.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Statement.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView_Statement.Name = "dataGridView_Statement";
            this.dataGridView_Statement.RowTemplate.Height = 27;
            this.dataGridView_Statement.Size = new System.Drawing.Size(210, 120);
            this.dataGridView_Statement.TabIndex = 0;
            // 
            // tabPage_Form
            // 
            this.tabPage_Form.Controls.Add(this.dataGridView_Form);
            this.tabPage_Form.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Form.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_Form.Name = "tabPage_Form";
            this.tabPage_Form.Size = new System.Drawing.Size(671, 261);
            this.tabPage_Form.TabIndex = 3;
            this.tabPage_Form.Text = "Form";
            this.tabPage_Form.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Form
            // 
            this.dataGridView_Form.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Form.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Form.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView_Form.Name = "dataGridView_Form";
            this.dataGridView_Form.RowTemplate.Height = 27;
            this.dataGridView_Form.Size = new System.Drawing.Size(210, 120);
            this.dataGridView_Form.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage_Form);
            this.tabControl.Controls.Add(this.tabPage_Statement);
            this.tabControl.Controls.Add(this.tabPage_Schedule);
            this.tabControl.Controls.Add(this.tabPage_Organization);
            this.tabControl.Controls.Add(this.tabPage_BigTable);
            this.tabControl.Controls.Add(this.tabPage_Work);
            this.tabControl.Controls.Add(this.tabPage_Floor);
            this.tabControl.Controls.Add(this.tabPage_What);
            this.tabControl.Controls.Add(this.tabPage_How);
            this.tabControl.Location = new System.Drawing.Point(10, 32);
            this.tabControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(679, 287);
            this.tabControl.TabIndex = 1;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPage_Work
            // 
            this.tabPage_Work.Controls.Add(this.dataGridView_Work);
            this.tabPage_Work.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Work.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_Work.Name = "tabPage_Work";
            this.tabPage_Work.Size = new System.Drawing.Size(671, 261);
            this.tabPage_Work.TabIndex = 6;
            this.tabPage_Work.Text = "Work";
            this.tabPage_Work.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Work
            // 
            this.dataGridView_Work.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Work.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Work.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView_Work.Name = "dataGridView_Work";
            this.dataGridView_Work.RowTemplate.Height = 27;
            this.dataGridView_Work.Size = new System.Drawing.Size(210, 120);
            this.dataGridView_Work.TabIndex = 1;
            // 
            // tabPage_Floor
            // 
            this.tabPage_Floor.Controls.Add(this.dataGridView_Floor);
            this.tabPage_Floor.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Floor.Name = "tabPage_Floor";
            this.tabPage_Floor.Size = new System.Drawing.Size(671, 261);
            this.tabPage_Floor.TabIndex = 8;
            this.tabPage_Floor.Text = "Floor";
            this.tabPage_Floor.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Floor
            // 
            this.dataGridView_Floor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Floor.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Floor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView_Floor.Name = "dataGridView_Floor";
            this.dataGridView_Floor.RowTemplate.Height = 27;
            this.dataGridView_Floor.Size = new System.Drawing.Size(210, 120);
            this.dataGridView_Floor.TabIndex = 2;
            // 
            // tabPage_What
            // 
            this.tabPage_What.Controls.Add(this.dataGridView_What);
            this.tabPage_What.Location = new System.Drawing.Point(4, 22);
            this.tabPage_What.Name = "tabPage_What";
            this.tabPage_What.Size = new System.Drawing.Size(671, 261);
            this.tabPage_What.TabIndex = 9;
            this.tabPage_What.Text = "What";
            this.tabPage_What.UseVisualStyleBackColor = true;
            // 
            // dataGridView_What
            // 
            this.dataGridView_What.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_What.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_What.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView_What.Name = "dataGridView_What";
            this.dataGridView_What.RowTemplate.Height = 27;
            this.dataGridView_What.Size = new System.Drawing.Size(210, 120);
            this.dataGridView_What.TabIndex = 3;
            // 
            // tabPage_How
            // 
            this.tabPage_How.Controls.Add(this.dataGridView_How);
            this.tabPage_How.Location = new System.Drawing.Point(4, 22);
            this.tabPage_How.Name = "tabPage_How";
            this.tabPage_How.Size = new System.Drawing.Size(671, 261);
            this.tabPage_How.TabIndex = 10;
            this.tabPage_How.Text = "How";
            this.tabPage_How.UseVisualStyleBackColor = true;
            // 
            // dataGridView_How
            // 
            this.dataGridView_How.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_How.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_How.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView_How.Name = "dataGridView_How";
            this.dataGridView_How.RowTemplate.Height = 27;
            this.dataGridView_How.Size = new System.Drawing.Size(210, 120);
            this.dataGridView_How.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 360);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "NinetyNine";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tabPage_BigTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_BigTable)).EndInit();
            this.tabPage_Organization.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Organization)).EndInit();
            this.tabPage_Schedule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Schedule)).EndInit();
            this.tabPage_Statement.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Statement)).EndInit();
            this.tabPage_Form.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Form)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPage_Work.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Work)).EndInit();
            this.tabPage_Floor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Floor)).EndInit();
            this.tabPage_What.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_What)).EndInit();
            this.tabPage_How.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_How)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem 파일ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 열기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 저장ToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem 도움말ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 라이센스ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ePPlusToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 빅테이블ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BigTable_Parsing_ToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage_BigTable;
        private System.Windows.Forms.DataGridView dataGridView_BigTable;
        private System.Windows.Forms.TabPage tabPage_Organization;
        private System.Windows.Forms.DataGridView dataGridView_Organization;
        private System.Windows.Forms.TabPage tabPage_Schedule;
        private System.Windows.Forms.DataGridView dataGridView_Schedule;
        private System.Windows.Forms.TabPage tabPage_Statement;
        private System.Windows.Forms.DataGridView dataGridView_Statement;
        private System.Windows.Forms.TabPage tabPage_Form;
        private System.Windows.Forms.DataGridView dataGridView_Form;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage_Work;
        private System.Windows.Forms.DataGridView dataGridView_Work;
        private System.Windows.Forms.ToolStripMenuItem newtonsoftJsonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BigTable_Mapping_ToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage_Floor;
        private System.Windows.Forms.DataGridView dataGridView_Floor;
        private System.Windows.Forms.TabPage tabPage_What;
        private System.Windows.Forms.DataGridView dataGridView_What;
        private System.Windows.Forms.TabPage tabPage_How;
        private System.Windows.Forms.DataGridView dataGridView_How;
    }
}

