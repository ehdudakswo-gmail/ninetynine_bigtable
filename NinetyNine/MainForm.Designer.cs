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
            this.fileListView = new System.Windows.Forms.ListView();
            this.bigTableCreateButton = new System.Windows.Forms.Button();
            this.fileOpenButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fileListView
            // 
            this.fileListView.Location = new System.Drawing.Point(13, 68);
            this.fileListView.Name = "fileListView";
            this.fileListView.Size = new System.Drawing.Size(775, 370);
            this.fileListView.TabIndex = 0;
            this.fileListView.UseCompatibleStateImageBehavior = false;
            // 
            // bigTableCreateButton
            // 
            this.bigTableCreateButton.Location = new System.Drawing.Point(169, 12);
            this.bigTableCreateButton.Name = "bigTableCreateButton";
            this.bigTableCreateButton.Size = new System.Drawing.Size(150, 40);
            this.bigTableCreateButton.TabIndex = 1;
            this.bigTableCreateButton.Text = "빅테이블 만들기";
            this.bigTableCreateButton.UseVisualStyleBackColor = true;
            this.bigTableCreateButton.Click += new System.EventHandler(this.bigTableCreateButton_Click);
            // 
            // fileOpenButton
            // 
            this.fileOpenButton.Location = new System.Drawing.Point(13, 12);
            this.fileOpenButton.Name = "fileOpenButton";
            this.fileOpenButton.Size = new System.Drawing.Size(150, 40);
            this.fileOpenButton.TabIndex = 2;
            this.fileOpenButton.Text = "파일 불러오기";
            this.fileOpenButton.UseVisualStyleBackColor = true;
            this.fileOpenButton.Click += new System.EventHandler(this.fileOpenButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.fileOpenButton);
            this.Controls.Add(this.bigTableCreateButton);
            this.Controls.Add(this.fileListView);
            this.Name = "MainForm";
            this.Text = "NinetyNine";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView fileListView;
        private System.Windows.Forms.Button bigTableCreateButton;
        private System.Windows.Forms.Button fileOpenButton;
    }
}

