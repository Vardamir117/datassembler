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

      


    }
}
