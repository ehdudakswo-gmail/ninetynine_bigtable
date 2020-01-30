namespace NinetyNine.Auto
{
    partial class AutoMappingForm
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
            this.comboBox_DataType = new System.Windows.Forms.ComboBox();
            this.textBox_Search = new System.Windows.Forms.TextBox();
            this.listBox_DataList = new System.Windows.Forms.ListBox();
            this.button_Enter = new System.Windows.Forms.Button();
            this.label_Data = new System.Windows.Forms.Label();
            this.label_Search = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox_DataType
            // 
            this.comboBox_DataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_DataType.FormattingEnabled = true;
            this.comboBox_DataType.Location = new System.Drawing.Point(58, 13);
            this.comboBox_DataType.Name = "comboBox_DataType";
            this.comboBox_DataType.Size = new System.Drawing.Size(414, 20);
            this.comboBox_DataType.TabIndex = 0;
            this.comboBox_DataType.SelectedIndexChanged += new System.EventHandler(this.comboBox_DataType_SelectedIndexChanged);
            this.comboBox_DataType.SelectionChangeCommitted += new System.EventHandler(this.comboBox_DataType_SelectionChangeCommitted);
            this.comboBox_DataType.Click += new System.EventHandler(this.comboBox_DataType_Click);
            this.comboBox_DataType.Enter += new System.EventHandler(this.comboBox_DataType_Enter);
            // 
            // textBox_Search
            // 
            this.textBox_Search.Location = new System.Drawing.Point(58, 40);
            this.textBox_Search.Name = "textBox_Search";
            this.textBox_Search.Size = new System.Drawing.Size(414, 21);
            this.textBox_Search.TabIndex = 1;
            this.textBox_Search.TextChanged += new System.EventHandler(this.textBox_Search_TextChanged);
            // 
            // listBox_DataList
            // 
            this.listBox_DataList.FormattingEnabled = true;
            this.listBox_DataList.ItemHeight = 12;
            this.listBox_DataList.Location = new System.Drawing.Point(13, 68);
            this.listBox_DataList.Name = "listBox_DataList";
            this.listBox_DataList.Size = new System.Drawing.Size(459, 448);
            this.listBox_DataList.TabIndex = 2;
            this.listBox_DataList.DoubleClick += new System.EventHandler(this.listBox_DataList_DoubleClick);
            // 
            // button_Enter
            // 
            this.button_Enter.Location = new System.Drawing.Point(12, 520);
            this.button_Enter.Name = "button_Enter";
            this.button_Enter.Size = new System.Drawing.Size(460, 30);
            this.button_Enter.TabIndex = 3;
            this.button_Enter.Text = "Enter";
            this.button_Enter.UseVisualStyleBackColor = true;
            this.button_Enter.Click += new System.EventHandler(this.button_Enter_Click);
            // 
            // label_Data
            // 
            this.label_Data.AutoSize = true;
            this.label_Data.Location = new System.Drawing.Point(12, 17);
            this.label_Data.Name = "label_Data";
            this.label_Data.Size = new System.Drawing.Size(41, 12);
            this.label_Data.TabIndex = 4;
            this.label_Data.Text = "데이터";
            // 
            // label_Search
            // 
            this.label_Search.AutoSize = true;
            this.label_Search.Location = new System.Drawing.Point(12, 43);
            this.label_Search.Name = "label_Search";
            this.label_Search.Size = new System.Drawing.Size(29, 12);
            this.label_Search.TabIndex = 5;
            this.label_Search.Text = "검색";
            // 
            // AutoMappingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 561);
            this.Controls.Add(this.label_Search);
            this.Controls.Add(this.label_Data);
            this.Controls.Add(this.button_Enter);
            this.Controls.Add(this.listBox_DataList);
            this.Controls.Add(this.textBox_Search);
            this.Controls.Add(this.comboBox_DataType);
            this.Name = "AutoMappingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "맵핑 자동화";
            this.Load += new System.EventHandler(this.AutoMappingForm_Load);
            this.Resize += new System.EventHandler(this.AutoMappingForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_DataType;
        private System.Windows.Forms.TextBox textBox_Search;
        private System.Windows.Forms.ListBox listBox_DataList;
        private System.Windows.Forms.Button button_Enter;
        private System.Windows.Forms.Label label_Data;
        private System.Windows.Forms.Label label_Search;
    }
}