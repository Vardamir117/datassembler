using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Threading;


namespace datassembler
{
    public partial class Main_Window : Form
    {
        public Main_Window()
        {
            InitializeComponent();       
        }

        public bool Console_Mode = false;


        public void Main_Window_Load(object sender, EventArgs e)
        {
            Text_Box_Dat_File.Text = datassembler.Properties.Settings.Default.Last_Path;
            Text_Box_Delimiter.Text = datassembler.Properties.Settings.Default.Delimiter_Sign.ToString();
            Label_File_Path.Focus(); // Removing Focus from the Text Box above

            if (datassembler.Properties.Settings.Default.Auto_Open_File) { Check_Box_Open_File.Checked = true; }
            
            if (Text_Box_Delimiter.Text[0] == ';')
            {   Button_Open_Dat_File.Text = @"Dat to Csv -->";
                Button_Open_Txt_File.Text = @"<-- Csv to Dat";
            }
        }


        public void Show_File(string The_Path, bool Append_Suffix) // Without Extension please
        {   if (Check_Box_Open_File.Checked)
            {   // Thread.Sleep(1000); 
                string Extension = ".txt";
                try
                {   if (!Append_Suffix) { Extension = ""; } 
                    else if (Text_Box_Delimiter.Text[0] == ';') { Extension = ".csv"; }
                    
                    System.Diagnostics.Process.Start(The_Path + Extension);
                }
                catch
                {   // if (The_Path == "") { MessageBox.Show("Please add filepath of the .dat or .csv/.txt into the model textbox."); }
                    // else { MessageBox.Show("Error; " + The_Path + Extension + Environment.NewLine + " was not found."); }           
                }
            }
        }

        public void Toggle_Control_Vision(bool Is_Visible)
        {
            List<Control> Controls = new List<Control>
            {   Label_File_Path, Label_Delimiter, Check_Box_Open_File, Text_Box_Delimiter, Text_Box_Dat_File,
                Button_Open_Dat_File, Button_Browse, Button_Open_Txt_File, Button_Get_Difference, Button_Compare_Values
            };

            foreach (Control Item in Controls) { Item.Visible = Is_Visible; }
            Text_Box_Console.Text = Environment.NewLine + "    "; // Clearing for Usage


            if (Is_Visible) { Console_Mode = false; Button_Merge_Into_File.Text = "Overwrite Sync Into"; }
            else { Console_Mode = true; Button_Merge_Into_File.Text = "   Ok"; }
        }


        private void Button_Open_Dat_File_Click(object sender, EventArgs e)
        {  string[] Arguments = new string[] { "/e" };
           //if (Check_Box_Open_File.Checked) { Arguments = new string[] { "/e /a" }; }
           Program.Run(Text_Box_Dat_File.Text, Arguments, Text_Box_Delimiter.Text[0]);

           Show_File(Text_Box_Dat_File.Text, true);
        }

        private void Button_Open_Dat_File_MouseHover(object sender, EventArgs e)
        { Button_Open_Dat_File.ForeColor = Color.White; }

        private void Button_Open_Dat_File_MouseLeave(object sender, EventArgs e)
        { Button_Open_Dat_File.ForeColor = Color.Black; }


        private void Button_Browse_Click(object sender, EventArgs e)    
        {   // Setting Innitial Filename and Data for the Open Menu
            Open_File_Dialog_1.FileName = "";
            Open_File_Dialog_1.InitialDirectory = Directory.GetCurrentDirectory();
            Open_File_Dialog_1.Filter = "dat files (*.dat)|*.dat|csv files (*.csv)|*.csv|txt files (*.txt)|*.txt";
            // "dat files (*.dat)|*.dat|txt files (*.txt)|*.txt";
            
            Open_File_Dialog_1.FilterIndex = 1;
            Open_File_Dialog_1.RestoreDirectory = true;
            Open_File_Dialog_1.CheckFileExists = true;
            Open_File_Dialog_1.CheckPathExists = true;


            try 
            {   if (Open_File_Dialog_1.ShowDialog() == DialogResult.OK)
                {   string The_Path = Path.GetDirectoryName(Open_File_Dialog_1.FileName) 
                    + @"\" + Path.GetFileNameWithoutExtension(Open_File_Dialog_1.FileName);

                    Text_Box_Dat_File.Text = The_Path;

                    datassembler.Properties.Settings.Default.Last_Path = The_Path;
                    datassembler.Properties.Settings.Default.Save();
                }
            } catch {}
            //catch (Exception The_Exception)
            //{ MessageBox.Show(The_Exception.Message, "MainWindow", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Button_Browse_MouseHover(object sender, EventArgs e)
        { Button_Browse.ForeColor = Color.White; }

        private void Button_Browse_MouseLeave(object sender, EventArgs e)
        { Button_Browse.ForeColor = Color.Black; }




        private void Button_Open_Txt_File_Click(object sender, EventArgs e)
        {
            string[] Arguments = new string[] { "/b" };
            //if (Check_Box_Open_File.Checked)  { Arguments = new string[] { "/b /a" }; }
            Program.Run(Text_Box_Dat_File.Text, Arguments, Text_Box_Delimiter.Text[0]);
            Show_File(Text_Box_Dat_File.Text, true);     
        }

        private void Button_Open_Txt_File_MouseHover(object sender, EventArgs e)
        { Button_Open_Txt_File.ForeColor = Color.White; }

        private void Button_Open_Txt_File_MouseLeave(object sender, EventArgs e)
        { Button_Open_Txt_File.ForeColor = Color.Black; }



        private void Check_Box_Open_File_CheckedChanged(object sender, EventArgs e)
        {
            if (Check_Box_Open_File.Checked) { datassembler.Properties.Settings.Default.Auto_Open_File = true; }
            else { datassembler.Properties.Settings.Default.Auto_Open_File = false; }
            datassembler.Properties.Settings.Default.Save();
        }

        private void Text_Box_Delimiter_TextChanged(object sender, EventArgs e)
        {   if (Text_Box_Delimiter.Text != "")
            {   datassembler.Properties.Settings.Default.Delimiter_Sign = Text_Box_Delimiter.Text[0];
                datassembler.Properties.Settings.Default.Save();

                Text_Box_Delimiter.Text = Text_Box_Delimiter.Text[0].ToString();

                if (Text_Box_Delimiter.Text[0] == ';')
                {   Button_Open_Dat_File.Text = @"Dat to Csv -->";
                    Button_Open_Txt_File.Text = @"<-- Csv to Dat";
                }
                else
                {   Button_Open_Dat_File.Text = @"Dat to Txt -->";
                    Button_Open_Txt_File.Text = @"<-- Txt to Dat";
                }
            }
        }




        int Compare_Mode = 1;


        private void Button_Get_Difference_Click(object sender, EventArgs e)
        {   string Result_File = "";

            // Setting Innitial Filename and Data for the Open Menu
            Open_File_Dialog_1.FileName = "";
            Open_File_Dialog_1.InitialDirectory = Directory.GetCurrentDirectory();

            if (Text_Box_Delimiter.Text[0] == ';') { Open_File_Dialog_1.Filter = "csv files (*.csv)|*.csv| txt files (*.txt)|*.txt"; }
            else { Open_File_Dialog_1.Filter = "txt files (*.txt)|*.txt|csv files (*.csv)|*.csv"; }

            Open_File_Dialog_1.FilterIndex = 1;
            Open_File_Dialog_1.RestoreDirectory = true;
            Open_File_Dialog_1.CheckFileExists = true;
            Open_File_Dialog_1.CheckPathExists = true;


            Button_Get_Difference.ForeColor = Color.Red;
            Button_Get_Difference.Text = "Please Wait";

            try
            {
                if (Open_File_Dialog_1.ShowDialog() == DialogResult.OK)
                {
                    var The_Program = new Program();
                    string The_Path = Path.GetDirectoryName(Open_File_Dialog_1.FileName) + @"\" + Path.GetFileNameWithoutExtension(Open_File_Dialog_1.FileName);
                    string The_Extension = Path.GetExtension(Open_File_Dialog_1.FileName);

                    Result_File = The_Program.Disassambly(Text_Box_Dat_File.Text, The_Path, The_Extension, Text_Box_Delimiter.Text[0], Compare_Mode);
                    // MessageBox.Show(The_Path);
                }
            } catch {}

            // GradientActiveCaption
            Button_Get_Difference.ForeColor = Color.Black;
            Button_Get_Difference.Text = "Compare Keys Of";

            Show_File(Result_File, false);
        }

        private void Button_Get_Difference_MouseHover(object sender, EventArgs e)
        { Button_Get_Difference.ForeColor = Color.White; }

        private void Button_Get_Difference_MouseLeave(object sender, EventArgs e)
        { Button_Get_Difference.ForeColor = Color.Black; }





        private void Button_Merge_Into_File_Click(object sender, EventArgs e)
        {
            if (Console_Mode) { Toggle_Control_Vision(true); return; } // This button acts as "OK" button in Console mode

            Button_Merge_Into_File.ForeColor = Color.Red;
            Button_Merge_Into_File.Text = "because this";

            Button_Compare_Values.ForeColor = Color.Red;
            Button_Compare_Values.Text = "takes long..";

            Compare_Mode = 3;   
            Button_Get_Difference_Click(null, null);
            Compare_Mode = 1; // Resetting    

            Button_Merge_Into_File.ForeColor = Color.Black;
            Button_Merge_Into_File.Text = "Overwrite Sync Into";

            Button_Compare_Values.ForeColor = Color.Black;
            Button_Compare_Values.Text = "Compare Values Of";
        }

        private void Button_Merge_Into_File_MouseHover(object sender, EventArgs e)
        { Button_Merge_Into_File.ForeColor = Color.White; }

        private void Button_Merge_Into_File_MouseLeave(object sender, EventArgs e)
        { Button_Merge_Into_File.ForeColor = Color.Black; }





        private void Button_Compare_Values_Click(object sender, EventArgs e)
        {
            Button_Compare_Values.ForeColor = Color.Red;
            Button_Compare_Values.Text = "takes long..";

            Compare_Mode = 2;
            // Runns the full code of the other button
            Button_Get_Difference_Click(null, null); 
            Compare_Mode = 1; // Resetting    
      
            Button_Compare_Values.ForeColor = Color.Black;
            Button_Compare_Values.Text = "Compare Values Of";
        }

        private void Button_Compare_Values_MouseHover(object sender, EventArgs e)
        { Button_Compare_Values.ForeColor = Color.White; }

        private void Button_Compare_Values_MouseLeave(object sender, EventArgs e)
        { Button_Compare_Values.ForeColor = Color.Black; }





 





    // =================================== End of File ===================================
    }
}
