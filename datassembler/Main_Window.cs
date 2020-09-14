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



        private void Button_Open_Dat_File_Click(object sender, EventArgs e)
        {  string[] Arguments = new string[] { "/e" };
           //if (Check_Box_Open_File.Checked) { Arguments = new string[] { "/e /a" }; }
           Program.Run(Text_Box_Dat_File.Text, Arguments, Text_Box_Delimiter.Text[0]);

           if (Check_Box_Open_File.Checked)
           {   // Thread.Sleep(1000);            
               try
               {   string Extension = ".txt";
                   if (Text_Box_Delimiter.Text[0] == ';') { Extension = ".csv"; }
                   System.Diagnostics.Process.Start(Text_Box_Dat_File.Text + Extension);
               }
               catch
               {   if (Text_Box_Dat_File.Text == "") { MessageBox.Show("Please add filepath of the .dat or .csv/.txt into the model textbox."); }
                   { MessageBox.Show("Error; No File wasfound."); }
                   // { MessageBox.Show("Error; Maybe you need to assign .txt files to any Editor? I recommend PsPad."); }
               }
           }
        }


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


        private void Button_Open_Txt_File_Click(object sender, EventArgs e)
        {
            string[] Arguments = new string[] { "/b" };
            //if (Check_Box_Open_File.Checked)  { Arguments = new string[] { "/b /a" }; }
            Program.Run(Text_Box_Dat_File.Text, Arguments, Text_Box_Delimiter.Text[0]);

            if (Check_Box_Open_File.Checked)
            {   // Thread.Sleep(1000);            
                try 
                {   string Extension = ".txt";
                    if (Text_Box_Delimiter.Text[0] == ';') { Extension = ".csv"; }
                    System.Diagnostics.Process.Start(Text_Box_Dat_File.Text + Extension); 
                }
                catch
                {
                    if (Text_Box_Dat_File.Text == "") { MessageBox.Show("Please add filepath of the .dat or .csv/.txt into the model textbox."); }
                    else { MessageBox.Show("Error; No File wasfound."); }
                    // { MessageBox.Show("Error; Maybe you need to assign .dat files to the dat Editor in this Windows account."); }
                }
            }         
        }


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
        {               
            // Setting Innitial Filename and Data for the Open Menu
            Open_File_Dialog_1.FileName = "";
            Open_File_Dialog_1.InitialDirectory = Directory.GetCurrentDirectory();
            Open_File_Dialog_1.Filter = "txt files (*.txt)|*.txt|csv files (*.csv)|*.csv";

            Open_File_Dialog_1.FilterIndex = 1;
            Open_File_Dialog_1.RestoreDirectory = true;
            Open_File_Dialog_1.CheckFileExists = true;
            Open_File_Dialog_1.CheckPathExists = true;


            Button_Get_Difference.ForeColor = Color.Red;
            Button_Get_Difference.Text = "Please Wait";

            try
            {   if (Open_File_Dialog_1.ShowDialog() == DialogResult.OK)
                {   var The_Program = new Program();
                    string The_Path = Path.GetDirectoryName(Open_File_Dialog_1.FileName) + @"\" + Path.GetFileNameWithoutExtension(Open_File_Dialog_1.FileName);
                    The_Program.Disassambly(Text_Box_Dat_File.Text, The_Path, Text_Box_Delimiter.Text[0], Compare_Mode);
                    // MessageBox.Show(The_Path);
                }
            } catch {}

            // GradientActiveCaption
            Button_Get_Difference.ForeColor = Color.Black;
            Button_Get_Difference.Text = "Compare Keys Of";
        }


        private void Button_Merge_Into_File_Click(object sender, EventArgs e)
        {        
            Button_Merge_Into_File.ForeColor = Color.Red;
            Button_Merge_Into_File.Text = "because this";

            Button_Compare_Values.ForeColor = Color.Red;
            Button_Compare_Values.Text = "takes long..";

            Compare_Mode = 3;


            // Setting Innitial Filename and Data for the Open Menu
            Open_File_Dialog_1.FileName = "";
            Open_File_Dialog_1.InitialDirectory = Directory.GetCurrentDirectory();
            Open_File_Dialog_1.Filter = "txt files (*.txt)|*.txt|csv files (*.csv)|*.csv";

            Open_File_Dialog_1.FilterIndex = 1;
            Open_File_Dialog_1.RestoreDirectory = true;
            Open_File_Dialog_1.CheckFileExists = true;
            Open_File_Dialog_1.CheckPathExists = true;
            try
            {
                if (Open_File_Dialog_1.ShowDialog() == DialogResult.OK)
                {
                    var The_Program = new Program(); // Removing .txt extension, because Disassambly() expects the path without it.                   
                    string The_Path = Path.GetDirectoryName(Open_File_Dialog_1.FileName) + @"\" + Path.GetFileNameWithoutExtension(Open_File_Dialog_1.FileName);

                    // CAUTION, We're switching file of the Selected_File and Second_File parameters!
                    The_Program.Disassambly(The_Path, Text_Box_Dat_File.Text, Text_Box_Delimiter.Text[0], Compare_Mode);
                    // MessageBox.Show(The_Path);
                }
            } catch {}

            Compare_Mode = 1; // Resetting    

            Button_Merge_Into_File.ForeColor = Color.Black;
            Button_Merge_Into_File.Text = "Overwrite Sync Into";

            Button_Compare_Values.ForeColor = Color.Black;
            Button_Compare_Values.Text = "Compare Values Of";
        }


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






    // =================================== End of File ===================================
    }
}
