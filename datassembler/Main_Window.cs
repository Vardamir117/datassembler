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
using Microsoft.VisualBasic.FileIO;


namespace datassembler
{
    public partial class Main_Window : Form
    {
        public Main_Window()
        {
            InitializeComponent();   

            Button_Backup.BackColor = Color.Transparent;  
            Set_Resource_Image(Button_Backup, global::datassembler.Properties.Resources.Button_Backup);

        
        }

        public bool Console_Mode = false;
        // Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string Program_Directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Dat_Assambler";



        //=====================//
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


        //=====================//
        public void Set_Resource_Image(Control Button_Name, Bitmap Image_Name)
        {   try
            {   Bitmap New_Image = new Bitmap(Button_Name.Size.Width, Button_Name.Size.Height);
                Graphics Picture = Graphics.FromImage((Image)New_Image);
                Picture.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                Picture.DrawImage(new Bitmap(Image_Name), 0, 0, Button_Name.Size.Width, Button_Name.Size.Height);
                Button_Name.BackgroundImage = New_Image;
            }
            catch { } 
        }


        //=====================//
        public void Deleting(string Data)
        {
            int Error_Count = 0;

            if (!Directory.Exists(Data)) { Error_Count = Error_Count + 10; }
            else
            {
                try { Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(Data, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin); }
                catch { Error_Count++; }
            }

            if (!File.Exists(Data)) { Error_Count = Error_Count + 10; }
            else
            {
                try { Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(Data, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin); }
                catch { Error_Count++; }
            }

            // If both methods failed that probably means no Visual Basic is installed.
            // if (Error_Count != 0 & Error_Count < 3) { Imperial_Console(600, 100, "Error: You seem to be missing Net Framework 4.0 and Visual Basic.FileIO on your System."); }
            // else if (Debug_Mode == "true" & Error_Count > 19) { Imperial_Console(600, 100, "   Could not find the selected Object for deletion."); }
        }

        //=====================//

        public void Renaming(string Path_and_File, string New_File_Name)
        {   string Debug_Mode = "false";

            if (Debug_Mode == "true")
            {
                FileAttributes The_Attribute = File.GetAttributes(Path_and_File);

                if ((The_Attribute & FileAttributes.Directory) == FileAttributes.Directory)
                { Directory.Move(Path_and_File, Path.GetDirectoryName(Path_and_File) + @"\" + New_File_Name); }
                else { File.Move(Path_and_File, Path.GetDirectoryName(Path_and_File) + @"\" + New_File_Name); }
            }

            else
            {
                try
                {   // get the file attributes for file or directory
                    FileAttributes The_Attribute = File.GetAttributes(Path_and_File);

                    //detect whether its a directory or file
                    if ((The_Attribute & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        try
                        {   // Getting the Path only for the right parameter and appending the new name to it
                            Directory.Move(Path_and_File, Path.GetDirectoryName(Path_and_File) + @"\" + New_File_Name);
                        }
                        catch { }
                    }
                    else
                    {
                        try { File.Move(Path_and_File, Path.GetDirectoryName(Path_and_File) + @"\" + New_File_Name); }
                        catch { }
                    }
                }
                catch { }
            }
        }
        //=====================//

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
        //=====================//

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
        //=====================//





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





            string Extension = ".txt";
            if (Text_Box_Delimiter.Text[0] == ';') { Extension = ".csv"; }

            string Backup_Directory = Program_Directory + @"\Backup\" + Path.GetFileNameWithoutExtension(Text_Box_Dat_File.Text + Extension);
         
            if (!Directory.Exists(Backup_Directory)) { Directory.CreateDirectory(Backup_Directory); }
            string[] All_Files = Directory.GetFiles(Backup_Directory);
            
            int Temporal_C = 0;
            // foreach (string File in All_Files)
            for (int i = All_Files.Count() -1; i >= 0; --i)
            {   try
                {   string[] Current = null; Temporal_C = 0;
                    Current = Path.GetFileName(All_Files[i]).Split('.');
                    Int32.TryParse(Current[0], out Temporal_C);


                    if (All_Files.Count() > 29 & Temporal_C > 29) { Deleting(All_Files[i]); } // getting rid of first and last slots.    
                    else { Renaming(All_Files[i], (Temporal_C + 1).ToString() + '.' + Current[1]); } // Current[1] is the Extension               
                }  catch {}
            }
     

            try { File.Copy(Text_Box_Dat_File.Text + Extension, Backup_Directory + @"\" + 1 + Extension); } catch {}
            Button_Backup_MouseHover(null, null); // Just a shiny effect that indicates it was saved
            // Show_File(Text_Box_Dat_File.Text, true); // This isn't supposed to show here    
        }


        private void Button_Open_Txt_File_MouseHover(object sender, EventArgs e)
        { Button_Open_Txt_File.ForeColor = Color.White; }

        private void Button_Open_Txt_File_MouseLeave(object sender, EventArgs e)
        { Button_Open_Txt_File.ForeColor = Color.Black; }




        private void Button_Backup_Click(object sender, EventArgs e)
        {
            string Backup_Directory = Program_Directory + @"\Backup";
            if (!Directory.Exists(Backup_Directory)) { Directory.CreateDirectory(Backup_Directory); }
            try { System.Diagnostics.Process.Start(Backup_Directory); } catch {}
        }

        private void Button_Backup_MouseHover(object sender, EventArgs e)
        { Set_Resource_Image(Button_Backup, global::datassembler.Properties.Resources.Button_Backup_Lit); }

        private void Button_Backup_MouseLeave(object sender, EventArgs e)
        { Set_Resource_Image(Button_Backup, global::datassembler.Properties.Resources.Button_Backup);  }




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
