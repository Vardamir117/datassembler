﻿namespace datassembler
{
    partial class Main_Window
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
            this.Text_Box_Dat_File = new System.Windows.Forms.TextBox();
            this.Button_Open_Dat_File = new System.Windows.Forms.Button();
            this.Button_Open_Txt_File = new System.Windows.Forms.Button();
            this.Button_Browse = new System.Windows.Forms.Button();
            this.Label_File_Path = new System.Windows.Forms.Label();
            this.Check_Box_Open_File = new System.Windows.Forms.CheckBox();
            this.Open_File_Dialog_1 = new System.Windows.Forms.OpenFileDialog();
            this.Text_Box_Delimiter = new System.Windows.Forms.TextBox();
            this.Label_Delimiter = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Text_Box_Dat_File
            // 
            this.Text_Box_Dat_File.Font = new System.Drawing.Font("Georgia", 16F);
            this.Text_Box_Dat_File.Location = new System.Drawing.Point(23, 56);
            this.Text_Box_Dat_File.Name = "Text_Box_Dat_File";
            this.Text_Box_Dat_File.Size = new System.Drawing.Size(558, 32);
            this.Text_Box_Dat_File.TabIndex = 0;
            // 
            // Button_Open_Dat_File
            // 
            this.Button_Open_Dat_File.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Button_Open_Dat_File.Font = new System.Drawing.Font("Georgia", 13F);
            this.Button_Open_Dat_File.Location = new System.Drawing.Point(23, 96);
            this.Button_Open_Dat_File.Name = "Button_Open_Dat_File";
            this.Button_Open_Dat_File.Size = new System.Drawing.Size(160, 34);
            this.Button_Open_Dat_File.TabIndex = 1;
            this.Button_Open_Dat_File.Text = "Dat to Txt -->";
            this.Button_Open_Dat_File.UseVisualStyleBackColor = false;
            this.Button_Open_Dat_File.Click += new System.EventHandler(this.Button_Open_Dat_File_Click);
            // 
            // Button_Open_Txt_File
            // 
            this.Button_Open_Txt_File.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Button_Open_Txt_File.Font = new System.Drawing.Font("Georgia", 13F);
            this.Button_Open_Txt_File.Location = new System.Drawing.Point(421, 96);
            this.Button_Open_Txt_File.Name = "Button_Open_Txt_File";
            this.Button_Open_Txt_File.Size = new System.Drawing.Size(160, 34);
            this.Button_Open_Txt_File.TabIndex = 3;
            this.Button_Open_Txt_File.Text = "<-- Txt to Dat";
            this.Button_Open_Txt_File.UseVisualStyleBackColor = false;
            this.Button_Open_Txt_File.Click += new System.EventHandler(this.Button_Open_Txt_File_Click);
            // 
            // Button_Browse
            // 
            this.Button_Browse.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Button_Browse.Font = new System.Drawing.Font("Georgia", 13F);
            this.Button_Browse.Location = new System.Drawing.Point(222, 96);
            this.Button_Browse.Name = "Button_Browse";
            this.Button_Browse.Size = new System.Drawing.Size(160, 34);
            this.Button_Browse.TabIndex = 4;
            this.Button_Browse.Text = "Browse";
            this.Button_Browse.UseVisualStyleBackColor = false;
            this.Button_Browse.Click += new System.EventHandler(this.Button_Browse_Click);
            // 
            // Label_File_Path
            // 
            this.Label_File_Path.AutoSize = true;
            this.Label_File_Path.Font = new System.Drawing.Font("Georgia", 20F);
            this.Label_File_Path.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.Label_File_Path.Location = new System.Drawing.Point(17, 16);
            this.Label_File_Path.Name = "Label_File_Path";
            this.Label_File_Path.Size = new System.Drawing.Size(121, 31);
            this.Label_File_Path.TabIndex = 5;
            this.Label_File_Path.Text = "File Path";
            // 
            // Check_Box_Open_File
            // 
            this.Check_Box_Open_File.AutoSize = true;
            this.Check_Box_Open_File.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Check_Box_Open_File.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.Check_Box_Open_File.Location = new System.Drawing.Point(402, 28);
            this.Check_Box_Open_File.Name = "Check_Box_Open_File";
            this.Check_Box_Open_File.Size = new System.Drawing.Size(186, 22);
            this.Check_Box_Open_File.TabIndex = 8;
            this.Check_Box_Open_File.Text = "Open after Conversion";
            this.Check_Box_Open_File.UseVisualStyleBackColor = true;
            this.Check_Box_Open_File.CheckedChanged += new System.EventHandler(this.Check_Box_Open_File_CheckedChanged);
            // 
            // Open_File_Dialog_1
            // 
            this.Open_File_Dialog_1.FileName = "File";
            // 
            // Text_Box_Delimiter
            // 
            this.Text_Box_Delimiter.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_Box_Delimiter.Location = new System.Drawing.Point(288, 26);
            this.Text_Box_Delimiter.Name = "Text_Box_Delimiter";
            this.Text_Box_Delimiter.Size = new System.Drawing.Size(19, 20);
            this.Text_Box_Delimiter.TabIndex = 9;
            this.Text_Box_Delimiter.TextChanged += new System.EventHandler(this.Text_Box_Delimiter_TextChanged);
            // 
            // Label_Delimiter
            // 
            this.Label_Delimiter.AutoSize = true;
            this.Label_Delimiter.Font = new System.Drawing.Font("Georgia", 12F);
            this.Label_Delimiter.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.Label_Delimiter.Location = new System.Drawing.Point(308, 29);
            this.Label_Delimiter.Name = "Label_Delimiter";
            this.Label_Delimiter.Size = new System.Drawing.Size(74, 18);
            this.Label_Delimiter.TabIndex = 10;
            this.Label_Delimiter.Text = "Delimiter";
            // 
            // Main_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(600, 158);
            this.Controls.Add(this.Label_Delimiter);
            this.Controls.Add(this.Text_Box_Delimiter);
            this.Controls.Add(this.Check_Box_Open_File);
            this.Controls.Add(this.Label_File_Path);
            this.Controls.Add(this.Button_Browse);
            this.Controls.Add(this.Button_Open_Txt_File);
            this.Controls.Add(this.Button_Open_Dat_File);
            this.Controls.Add(this.Text_Box_Dat_File);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Main_Window";
            this.Text = "Jorritkarwehr\'s Dat Assambler          (powered by Imperialware UI)";
            this.Load += new System.EventHandler(this.Main_Window_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox Text_Box_Dat_File;
        public System.Windows.Forms.Button Button_Open_Dat_File;
        public System.Windows.Forms.Button Button_Open_Txt_File;
        public System.Windows.Forms.Button Button_Browse;
        public System.Windows.Forms.Label Label_File_Path;
        public System.Windows.Forms.CheckBox Check_Box_Open_File;
        public System.Windows.Forms.OpenFileDialog Open_File_Dialog_1;
        private System.Windows.Forms.TextBox Text_Box_Delimiter;
        public System.Windows.Forms.Label Label_Delimiter;

    }
}