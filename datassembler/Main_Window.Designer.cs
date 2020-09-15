namespace datassembler
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
            this.Button_Get_Difference = new System.Windows.Forms.Button();
            this.Button_Compare_Values = new System.Windows.Forms.Button();
            this.Button_Merge_Into_File = new System.Windows.Forms.Button();
            this.Text_Box_Console = new System.Windows.Forms.RichTextBox();
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
            this.Button_Open_Dat_File.Location = new System.Drawing.Point(21, 100);
            this.Button_Open_Dat_File.Name = "Button_Open_Dat_File";
            this.Button_Open_Dat_File.Size = new System.Drawing.Size(176, 34);
            this.Button_Open_Dat_File.TabIndex = 1;
            this.Button_Open_Dat_File.Text = "Dat to Txt -->";
            this.Button_Open_Dat_File.UseVisualStyleBackColor = false;
            this.Button_Open_Dat_File.Click += new System.EventHandler(this.Button_Open_Dat_File_Click);
            this.Button_Open_Dat_File.MouseLeave += new System.EventHandler(this.Button_Open_Dat_File_MouseLeave);
            this.Button_Open_Dat_File.MouseHover += new System.EventHandler(this.Button_Open_Dat_File_MouseHover);
            // 
            // Button_Open_Txt_File
            // 
            this.Button_Open_Txt_File.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Button_Open_Txt_File.Font = new System.Drawing.Font("Georgia", 13F);
            this.Button_Open_Txt_File.Location = new System.Drawing.Point(407, 100);
            this.Button_Open_Txt_File.Name = "Button_Open_Txt_File";
            this.Button_Open_Txt_File.Size = new System.Drawing.Size(176, 34);
            this.Button_Open_Txt_File.TabIndex = 3;
            this.Button_Open_Txt_File.Text = "<-- Txt to Dat";
            this.Button_Open_Txt_File.UseVisualStyleBackColor = false;
            this.Button_Open_Txt_File.Click += new System.EventHandler(this.Button_Open_Txt_File_Click);
            this.Button_Open_Txt_File.MouseLeave += new System.EventHandler(this.Button_Open_Txt_File_MouseLeave);
            this.Button_Open_Txt_File.MouseHover += new System.EventHandler(this.Button_Open_Txt_File_MouseHover);
            // 
            // Button_Browse
            // 
            this.Button_Browse.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Button_Browse.Font = new System.Drawing.Font("Georgia", 13F);
            this.Button_Browse.Location = new System.Drawing.Point(214, 100);
            this.Button_Browse.Name = "Button_Browse";
            this.Button_Browse.Size = new System.Drawing.Size(176, 34);
            this.Button_Browse.TabIndex = 4;
            this.Button_Browse.Text = "Browse";
            this.Button_Browse.UseVisualStyleBackColor = false;
            this.Button_Browse.Click += new System.EventHandler(this.Button_Browse_Click);
            this.Button_Browse.MouseLeave += new System.EventHandler(this.Button_Browse_MouseLeave);
            this.Button_Browse.MouseHover += new System.EventHandler(this.Button_Browse_MouseHover);
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
            // Button_Get_Difference
            // 
            this.Button_Get_Difference.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Button_Get_Difference.Font = new System.Drawing.Font("Georgia", 13F);
            this.Button_Get_Difference.Location = new System.Drawing.Point(21, 146);
            this.Button_Get_Difference.Name = "Button_Get_Difference";
            this.Button_Get_Difference.Size = new System.Drawing.Size(176, 34);
            this.Button_Get_Difference.TabIndex = 11;
            this.Button_Get_Difference.Text = "Compare Keys Of";
            this.Button_Get_Difference.UseVisualStyleBackColor = false;
            this.Button_Get_Difference.Click += new System.EventHandler(this.Button_Get_Difference_Click);
            this.Button_Get_Difference.MouseLeave += new System.EventHandler(this.Button_Get_Difference_MouseLeave);
            this.Button_Get_Difference.MouseHover += new System.EventHandler(this.Button_Get_Difference_MouseHover);
            // 
            // Button_Compare_Values
            // 
            this.Button_Compare_Values.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Button_Compare_Values.Font = new System.Drawing.Font("Georgia", 11F);
            this.Button_Compare_Values.Location = new System.Drawing.Point(407, 146);
            this.Button_Compare_Values.Name = "Button_Compare_Values";
            this.Button_Compare_Values.Size = new System.Drawing.Size(176, 34);
            this.Button_Compare_Values.TabIndex = 12;
            this.Button_Compare_Values.Text = "Compare Values Of";
            this.Button_Compare_Values.UseVisualStyleBackColor = false;
            this.Button_Compare_Values.Click += new System.EventHandler(this.Button_Compare_Values_Click);
            this.Button_Compare_Values.MouseLeave += new System.EventHandler(this.Button_Compare_Values_MouseLeave);
            this.Button_Compare_Values.MouseHover += new System.EventHandler(this.Button_Compare_Values_MouseHover);
            // 
            // Button_Merge_Into_File
            // 
            this.Button_Merge_Into_File.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Button_Merge_Into_File.Font = new System.Drawing.Font("Georgia", 13F);
            this.Button_Merge_Into_File.Location = new System.Drawing.Point(214, 146);
            this.Button_Merge_Into_File.Name = "Button_Merge_Into_File";
            this.Button_Merge_Into_File.Size = new System.Drawing.Size(176, 34);
            this.Button_Merge_Into_File.TabIndex = 13;
            this.Button_Merge_Into_File.Text = "Overwrite Sync Into";
            this.Button_Merge_Into_File.UseVisualStyleBackColor = false;
            this.Button_Merge_Into_File.Click += new System.EventHandler(this.Button_Merge_Into_File_Click);
            this.Button_Merge_Into_File.MouseLeave += new System.EventHandler(this.Button_Merge_Into_File_MouseLeave);
            this.Button_Merge_Into_File.MouseHover += new System.EventHandler(this.Button_Merge_Into_File_MouseHover);
            // 
            // Text_Box_Console
            // 
            this.Text_Box_Console.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Text_Box_Console.Font = new System.Drawing.Font("Georgia", 20F);
            this.Text_Box_Console.ForeColor = System.Drawing.SystemColors.MenuBar;
            this.Text_Box_Console.Location = new System.Drawing.Point(-2, -2);
            this.Text_Box_Console.Name = "Text_Box_Console";
            this.Text_Box_Console.Size = new System.Drawing.Size(604, 198);
            this.Text_Box_Console.TabIndex = 14;
            this.Text_Box_Console.Text = "    ";
            // 
            // Main_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(600, 195);
            this.Controls.Add(this.Button_Merge_Into_File);
            this.Controls.Add(this.Button_Compare_Values);
            this.Controls.Add(this.Button_Get_Difference);
            this.Controls.Add(this.Label_Delimiter);
            this.Controls.Add(this.Text_Box_Delimiter);
            this.Controls.Add(this.Check_Box_Open_File);
            this.Controls.Add(this.Label_File_Path);
            this.Controls.Add(this.Button_Browse);
            this.Controls.Add(this.Button_Open_Txt_File);
            this.Controls.Add(this.Button_Open_Dat_File);
            this.Controls.Add(this.Text_Box_Dat_File);
            this.Controls.Add(this.Text_Box_Console);
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
        public System.Windows.Forms.Button Button_Get_Difference;
        public System.Windows.Forms.Button Button_Compare_Values;
        public System.Windows.Forms.Button Button_Merge_Into_File;
        public System.Windows.Forms.RichTextBox Text_Box_Console;

    }
}