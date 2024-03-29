﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Threading.Tasks;
using System.Windows.Forms;




namespace datassembler
{
    public static class crcGlobals {
        public static uint[] crcTable = new uint[256];

        public static void initTable()
        {
            for (uint i = 0; i < 256; i++)
            {
                uint crc = i;
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 1) == 1)
                    {
                        crc = (crc >> 1) ^ 0xEDB88320;
                    }
                    else
                    {
                        crc = (crc >> 1);
                    }
                }
                crcTable[i] = crc & 0xFFFFFFFF;
            }
        }
    }

    class Text_Entry : IComparable<Text_Entry>
    {
        public string identifier;
        public string entry;
        public uint crc;

        public static byte[] toBytes(char[] value)
        {
            byte[] corenne = new byte[2 * value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                byte[] temp = BitConverter.GetBytes(value[i]);
                corenne[2 * i] = temp[0];
                corenne[2 * i + 1] = temp[1];
            }
            return corenne;
        }

        public static Text_Entry FromCsv(string csvLine, char delimiter)
        {
            if (csvLine.Length == 0 || !csvLine.Contains(delimiter))
            {
                return new Text_Entry();
            }           

            Text_Entry entry = new Text_Entry();
            int firstdelimit = csvLine.IndexOf(delimiter);
            entry.identifier = csvLine.Substring(0, firstdelimit);
            entry.entry = csvLine.Substring(firstdelimit + 1, csvLine.Length - firstdelimit - 1);

            uint check = 0xFFFFFFFF;
            byte[] win1252Bytes = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding("windows-1252"), toBytes(entry.identifier.ToCharArray(0, entry.identifier.Length)));
            for (int j = 0; j < win1252Bytes.Length; j++)
            {
                check = ((check >> 8) & 0x00FFFFFF) ^ crcGlobals.crcTable[(check ^ win1252Bytes[j]) & 0xFF];
            }
            check ^= 0xFFFFFFFF;
            entry.crc = check;

            return entry;
        }
        public int CompareTo(Text_Entry other)
        {
            // Numeric sort by crc
            return this.crc.CompareTo(other.crc);
        }
    }

    class Key_Pair : IComparable<Key_Pair>
    {
        public string identifier;
        public string entry;

        public int CompareTo(Key_Pair other)
        {
            // Alphabetize sort by id
            return this.identifier.CompareTo(other.identifier);
        }
    }

    public class Program
    {
        static byte[] tobytesLE(uint value)
        {
            byte[] corenne = new byte[4];
            corenne[0] = Convert.ToByte(value & 0xff);
            corenne[1] = Convert.ToByte((value & 0xff00) >> 8);
            corenne[2] = Convert.ToByte((value & 0xff0000) >> 16);
            corenne[3] = Convert.ToByte((value & 0xff000000) >> 24);

            return corenne;
        }

        static int make32(byte[] source, int startindex ) {
            int corenne;
            corenne = Convert.ToInt32(source[startindex]);
            corenne += Convert.ToInt32(source[startindex + 1]) << 8;
            corenne += Convert.ToInt32(source[startindex + 2]) << 16;
            corenne += Convert.ToInt32(source[startindex + 3]) << 24;

            return corenne;
        }

        static string readentry(byte[] source, int startindex, int length)
        {
            byte[] entrybytes = new byte[2*length];
            for (int i = 0; i < 2*length; i++) {
                entrybytes[i] = source[startindex + i];
            }

            return System.Text.Encoding.Unicode.GetString(entrybytes);
        }

        static string readid(byte[] source, int startindex, int length)
        {
            byte[] entrybytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                entrybytes[i] = source[startindex + i];
            }

            return System.Text.Encoding.GetEncoding("windows-1252").GetString(entrybytes);
        }

        static void decompileDat(string source, string dest, char delimiter, int sorttype)
        {
            if (!File.Exists(source)) {
                Console.WriteLine("File " + source + " not found" );
                return;
            }
            byte[] datfile = System.IO.File.ReadAllBytes(source);

            int total_entries = make32(datfile, 0);
            int index = 4;

            int[] textlength = new int[total_entries];
            int[] idlength = new int[total_entries];
            Key_Pair[] entries = new Key_Pair[total_entries];

            for (int i = 0; i < total_entries; i++)
            {
                index += 4;
                textlength[i] = make32(datfile, index);
                index += 4;
                idlength[i] = make32(datfile, index);
                index += 4;
            }
            for (int i = 0; i < total_entries; i++)
            {
                entries[i] = new Key_Pair();
                entries[i].entry = readentry(datfile, index, textlength[i]);
                index += 2 * textlength[i];
            }
            for (int i = 0; i < total_entries; i++)
            {
                entries[i].identifier = readid(datfile, index, idlength[i]);
                index += idlength[i];
            }

            if(sorttype == 1)
            {
                for (int i = 0; i < total_entries; i++)
                {
                    string temp = entries[i].identifier;
                    entries[i].identifier = entries[i].entry;
                    entries[i].entry = temp;
                }
            }

            if (sorttype != 2)
            {
                Array.Sort(entries);
            }

            if (sorttype == 1)
            {
                for (int i = 0; i < total_entries; i++)
                {
                    string temp = entries[i].identifier;
                    entries[i].identifier = entries[i].entry;
                    entries[i].entry = temp;
                }
            }

            using (
                var sw = new StreamWriter(
                    new FileStream(dest, FileMode.Create, FileAccess.ReadWrite),
                    Encoding.UTF8
                )
            )
            {
                for (int i = 0; i < total_entries; i++) sw.WriteLine(entries[i].identifier + delimiter + entries[i].entry);
            }
        }

        static void compileDat(string[][] sources, string outFile, char delimiter) {
            uint total_entries = 0;
            File.Delete(outFile);
            crcGlobals.initTable();
            List<Text_Entry> Entries = new List<Text_Entry>();

            for (int level = 0; level < sources.Length; level++)
            {
                List<Text_Entry> Level_Entries = new List<Text_Entry>();
                uint level_entries = 0;
                for (uint file = 0; file < sources[level].Length; file++)
                {
                    if (!File.Exists(sources[level][file]))
                    {
                        Console.WriteLine("File " + sources[level][file] + " not found");
                    }
                    else
                    {
                        string[] lines = File.ReadAllLines(sources[level][file], Encoding.UTF8);

                        for (uint i = 0; i < lines.Length; i++)
                        {
                            Text_Entry item = Text_Entry.FromCsv(lines[i], delimiter);
                            if (item.identifier != null)
                            {
                                if (level == 0)
                                {
                                    Entries.Add(item);
                                    total_entries++;
                                }
                                else {
                                    Level_Entries.Add(item);
                                    level_entries++;
                                }
                            }
                        }
                    }
                }
                if (level > 0)
                {
                    Entries.Sort();
                    int id = -1;
                    uint start_entries = total_entries;
                    for (int i = 0; i < level_entries; i++)
                    {
                        int result;
                        do {
                            id++;
                            if(id >= start_entries)
                            {
                                result = 1;
                            }
                            else
                            {
                                result = string.Compare(Level_Entries[i].identifier, Entries[id].identifier, StringComparison.OrdinalIgnoreCase);
                            }
                        } while (result <= 0);
                        if (result == 0)
                        {
                            Entries[i].entry = Level_Entries[i].entry;
                        }
                        else
                        {
                            Entries.Add(Level_Entries[i]);
                            total_entries++;
                        }
                    }
                }
            }
           

            Entries.Sort();

            List<byte> datfile = new List<byte>();


            byte[] bytes = tobytesLE(total_entries);
            for (int i = 0; i < 4; i++)
            {
                datfile.Add(bytes[i]);
            }


            for (int i = 0; i < total_entries; i++)
            {
                byte[] crcbytes = tobytesLE(Entries[i].crc);
                for (int j = 0; j < 4; j++)
                {
                    datfile.Add(crcbytes[j]);
                }
                crcbytes = tobytesLE(Convert.ToUInt32(Entries[i].entry.Length));
                for (int j = 0; j < 4; j++)
                {
                    datfile.Add(crcbytes[j]);
                }
                crcbytes = tobytesLE(Convert.ToUInt32(Entries[i].identifier.Length));
                for (int j = 0; j < 4; j++)
                {
                    datfile.Add(crcbytes[j]);
                }
            }

            for (int i = 0; i < total_entries; i++)
            {
                byte[] letters = Encoding.Convert(Encoding.Unicode, Encoding.Unicode, Text_Entry.toBytes(Entries[i].entry.ToCharArray(0, Entries[i].entry.Length)));
                for (int j = 0; j < letters.Length; j++)
                {
                    datfile.Add(Convert.ToByte(letters[j]));
                }
            }

            for (int i = 0; i < total_entries; i++)
            {
                byte[] letters = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding("windows-1252"), Text_Entry.toBytes(Entries[i].identifier.ToCharArray(0, Entries[i].identifier.Length)));
                for (int j = 0; j < letters.Length; j++)
                {
                    datfile.Add(Convert.ToByte(letters[j]));
                }
            }


            System.IO.File.WriteAllBytes(outFile, datfile.ToArray());
        }

        static void alphabetize(string targetFile, char indelimiter, char outdelimiter, bool sortvalue, bool nocoll, bool notrail) {
            string[] lines = File.ReadAllLines(targetFile, Encoding.UTF8);
            List<Key_Pair> Entries = new List<Key_Pair>();
            int total_entries = 0;

            for (Int64 i = 0; i < lines.Length; i++)
            {
                if (!(lines[i].Length == 0 || !lines[i].Contains(indelimiter)))
                {
                    Key_Pair entry = new Key_Pair();
                    int firstdelimit = lines[i].IndexOf(indelimiter);
                    entry.identifier = lines[i].Substring(0, firstdelimit);
                    entry.entry = lines[i].Substring(firstdelimit + 1, lines[i].Length - firstdelimit - 1);
                    if (!notrail && entry.identifier != null)
                    {
                        if (entry.entry.Length > 1) { //Clear trailing delimiters
                            if (entry.entry.Substring(entry.entry.Length - 1, 1).Contains(indelimiter))
                            {
                                entry.entry = entry.entry.Substring(0, entry.entry.Length - 1);
                            }
                        }

                        if (sortvalue)
                        {
                            string temp = entry.identifier;
                            entry.identifier = entry.entry;
                            entry.entry = temp;
                        }
                        Entries.Add(entry);
                        total_entries++;
                    }
                }
            }

            Entries.Sort();

            if (sortvalue)
            {
                for (int i = 0; i < total_entries; i++)
                {
                    string temp = Entries[i].identifier;
                    Entries[i].identifier = Entries[i].entry;
                    Entries[i].entry = temp;
                }
            }

            using (
                var sw = new StreamWriter(
                    new FileStream(targetFile, FileMode.Create, FileAccess.ReadWrite),
                    Encoding.UTF8
                )
            )
            {
                for (int i = 0; i < total_entries; i++) sw.WriteLine(Entries[i].identifier + outdelimiter + Entries[i].entry);
            }

            using (
                var log = new StreamWriter(
                    new FileStream("buildlog.txt", FileMode.Create, FileAccess.ReadWrite),
                    Encoding.UTF8
                )
            )
            {
                if (!nocoll) {
                    if(!sortvalue)
                    {
                        for (int i = 0; i < total_entries - 1; i++)
                        {
                            if (Entries[i].identifier == Entries[i + 1].identifier)
                            {
                                log.WriteLine("Duplicate identifier: " + Entries[i].identifier);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < total_entries - 1; i++)
                        {
                            if (Entries[i].entry == Entries[i + 1].entry)
                            {
                                log.WriteLine("Duplicate text: " + Entries[i].entry);
                            }
                        }
                    }
                }
            }

        }


        [STAThread] // Prevents an exception in Open_File_Dialog_1

        // renamed original Main to Run because this is supposed to fire once only.
        static public void Main(string[] args) 
        {
            if (args.Length == 0) // Start in UI Mode
            {   Main_Window The_Window = new Main_Window();
                Application.Run(The_Window);           
            }


            else
            {   string The_File = ""; // Automatic grabbing of first valid file               
                
                if (args.Length > 1)
                {   The_File = args[1];

                    if (File.Exists(The_File))
                    {   // Works even with .dat instead of .txt and vice versa!
                        if (The_File.EndsWith(".dat") | The_File.EndsWith(".csv") | The_File.EndsWith(".txt"))
                        { The_File = args[1].Remove(args[1].Length - 4); } // Removing Extension
                        else { The_File = args[1]; } // Seems to have no extension specified
                    }
                }

                else if (The_File == "")
                {   foreach (string The_Path in Directory.GetFiles(Directory.GetCurrentDirectory()))
                    {   if (The_File == "" & args[0] == "/b") // If not already assigned, auto assigning
                        {   if (The_Path.EndsWith(".csv") | The_Path.EndsWith(".txt"))
                            {   The_File = The_Path.Remove(The_Path.Length - 4); // MessageBox.Show("Assigned " + The_File);      
                                break;
                            }
                        }

                        if (The_File == "" & The_Path.EndsWith(".dat") & args[0] == "/e")
                        {   The_File = The_Path.Remove(The_Path.Length - 4); // MessageBox.Show("Assigned " + The_File);
                            break;
                        }
                    }
                }

                // MessageBox.Show(args[0] + ", " + The_File); 
                // In console, the mode is position 1, Filepath is position 2, Delimiter is position 3
                try { Run(The_File, args, '0'); } catch { MessageBox.Show("Error in parameter execution."); }              
            }
           
        }



        static public void Run(string File_Path, string[] args, char delimiter)
        {   // string Current_Directory = Directory.GetCurrentDirectory();       
 
            string outFile = File_Path + ".dat";
            string sourceFile = File_Path + ".txt";
            if (delimiter == ';') { sourceFile = File_Path + ".csv"; }
            // MessageBox.Show(outFile + Add_Line + "and   " + sourceFile);

            bool nooutset = true;
            char outdelimiter = delimiter;

            if (delimiter == '0') // In console mode we grab it from settings
            {   delimiter = datassembler.Properties.Settings.Default.Delimiter_Sign;
                outdelimiter = delimiter;
            } // MessageBox.Show(delimiter.ToString());



  
            // Messes with the code above
            //if (args.Length > 1) {
            //    if (File.Exists(args[1])) {
            //        sourceFile = args[1];
            //        if (args.Length > 2 && !(args[2].Substring(0, 1).Contains("-"))) {
            //            outFile = args[2];
            //        }
            //    }
            //}

            for (Int64 i = 0; i < args.Length; i++)
            {
                if (args[i].Length > 1) {
                    if (args[i].Substring(0, 1).Contains("-")) {
                        switch (args[i].Substring(1, 1))
                        {
                            case "s":
                                if (args[i].Length > 2) {
                                    delimiter = args[i][2];
                                }
                                break;
                            case "o":
                                if (args[i].Length > 1)
                                {
                                    nooutset = false;
                                    outdelimiter = args[i][2];
                                }
                                break;
                        }
                    }
                }
            }

            if (nooutset) {
                outdelimiter = delimiter;
            }

            switch (args[0]) {
                case "/h":
                case "\\h":
                    Console.WriteLine();
                    Console.WriteLine("Petroglyph dat file assembler 1.0");
                    Console.WriteLine("  by Jorritkarwehr   September 2019");
                    Console.WriteLine("  build dat: </b txtfile datfile -s:; -a:txt2 -a:txt3... -r:txt4>");
                    Console.WriteLine("  export txt: /e <datfile txtfile -s:; -v -c>");
                    Console.WriteLine("  alphabetize/clean: /a <txtfile -v -n -t -s:; -o:,>");
                    Console.WriteLine("  extra documentation: /d");
                    Console.WriteLine();
                    Console.WriteLine("-s: separator -a: additional txt -r: replacing txt -v: sort by value -c: sort by CRC -n: no collision file -t: no trailing separator trim -o: output separator");
                    Console.WriteLine();
                    break;
                case "/b":
                case "\\b":
                    string[][] sources = new string[1][];
                    sources[0] = new string[1];
                    sources[0][0] = sourceFile;
                    int csvid = 0;
                    int csvid2 = 0;

                    for (Int64 i = 1; i < args.Length; i++)
                    {
                        if (args[i].Substring(0, 1) == "-")
                        {
                            if (args[i].Substring(1, 1) == "a") {
                                Array.Resize(ref sources[csvid], csvid2+1);
                                sources[csvid][csvid2] = args[i].Substring(3, args[i].Length - 3);
                                csvid2++;
                            }
                            if (args[i].Substring(1, 1) == "r")
                            {
                                csvid++;
                                csvid2 = 0;
                                Array.Resize(ref sources, csvid+1);
                                sources[csvid] = new string[1];
                                sources[csvid][csvid2] = args[i].Substring(3, args[i].Length - 3);
                            }
                        }
                    }

                    compileDat(sources, outFile, delimiter);
                    break;
                case "/e":
                case "\\e":
                    int sorttype = 0;
                    for (Int64 i = 1; i < args.Length; i++)
                    {
                        if (args[i].Substring(0, 1) == "-")
                        {
                            if (args[i].Substring(1, 1) == "v")
                            {
                                sorttype = 1;
                            }
                            if (args[i].Substring(1, 1) == "c")
                            {
                                sorttype = 2;
                            }
                        }
                    }

                    decompileDat(outFile, sourceFile, delimiter, sorttype);
                    break;
                case "/a":
                case "\\a":
                    bool valuesort = false;
                    bool nocoll = false;
                    bool notrail = false;
                    for (Int64 i = 1; i < args.Length; i++)
                    {
                        if (args[i].Substring(0, 1) == "-")
                        {
                            if (args[i].Substring(1, 1) == "v") {
                                valuesort = true;
                            }
                            if (args[i].Substring(1, 1) == "n")
                            {
                                nocoll = true;
                            }
                            if (args[i].Substring(1, 1) == "t")
                            {
                                notrail = true;
                            }
                        }
                    }
                    alphabetize(sourceFile, delimiter, outdelimiter, valuesort, nocoll, notrail);
                    break;
                case "/d":
                case "\\d":
                    Console.WriteLine("Txt files consist lines of a text id, a separator, and a text entry.");
                    Console.WriteLine("Only the first separator is parsed and additional ones may be used in the text entry.");
                    Console.WriteLine("Ordering of the txt lines is not important.");
                    Console.WriteLine("Empty lines or lines without a separator will be ignored when assembling a dat file");
                    Console.WriteLine();

                    Console.WriteLine("Filenames are optional and will default to MasterTextFile_ENGLISH.");
                    Console.WriteLine("If provided, they must be the first arguments as listed in help. Flags are not order sensitive");
                    Console.WriteLine("Calling the program with no arguments is equivalent to a build command with default arguments");
                    Console.WriteLine();

                    Console.WriteLine("Build (/b) writes a Petroglyph dat file from the specified txt");
                    Console.WriteLine("The -a flag can be repeated to specify two or more txt files");
                    Console.WriteLine("Any duplicate entries in -a files will both be written and noted in the log file");
                    Console.WriteLine("The -r flag works similarly, but duplicates with replace the original and not be logged");
                    Console.WriteLine("Files listed with -a after -r will be counted as part of the -r rather than the main file");
                    Console.WriteLine();

                    Console.WriteLine("Export (/e) creates a txt from the specified dat file");
                    Console.WriteLine();

                    Console.WriteLine("Alphabetize (/a) sorts the specified txt file by key value and removes blank lines and trailing separators");
                    Console.WriteLine("Unless suppressed, duplicate lines will be recorded in a log in the source directory");
                    Console.WriteLine("Duplicate values instead of ids will be logged when using -v");
                    Console.WriteLine("If no out separator is specified, it will follow the input separator");
                    Console.WriteLine();
                    break;
                default:
                    Console.WriteLine("Unrecognized command. Use /h for a command list");
                    break;
            }
        }



        // ============================= Adding Difference Selection =============================
        public static string Add_Line = Environment.NewLine;


        public void Inform(string Content)
        {   Form The_Window = Application.OpenForms["Main_Window"];
            Main_Window Active_Window = (Main_Window)The_Window; // This is a Hack!

            Active_Window.Toggle_Control_Vision(false);
            Active_Window.Text_Box_Console.Text += Content;
        
            // Get the Text Box alternative
            //RichTextBox Box = (RichTextBox)The_Window.Controls.Find("Text_Box_Console", true)[0];
            //Box.Text = "Test This";              
        }


       

        // Disassambly(Text_Box_Dat_File.Text, "Small_File", Text_Box_Dat_File.Text + "_Difference.txt", Text_Box_Delimiter.Text[0]);
        // Use NO Extensions for the 2 filepaths!
        public string Disassambly(string Selected_File, string Second_File, string The_Extension, char Delimiter, int Compare_Mode)
        {   string The_Text = "";
            bool Found_In_Keys = false;
            bool Found_In_Values = false;            
            List<string> Key_Cache = new List<string>();
            List<string> Value_Cache = new List<string>();
            List<string> Line_Cache = new List<string>();
            List<string> Result_Cache = new List<string>();

          

            // Inform(Selected_File + ", " + Second_File);
            if (Selected_File == Second_File) 
            { Inform("You are trying to compare " + Add_Line + "    " + Selected_File + Add_Line + "    To itself."); return ""; }


            int Failed_Tags = 0;
            string Current_Line = "";
            try // Loading the second file into Cache
            {
                
                foreach (string Line in File.ReadLines(Second_File + The_Extension))
                {   //try {   
                        Current_Line = Line;
                        // if (Line.Contains("#NAME?")){ continue; }
                        Key_Cache.Add(Line.Split(Delimiter)[0]);
                        Value_Cache.Add(Line.Split(Delimiter)[1]);
                        Line_Cache.Add(Line); // Key + Value
                    //} catch { Failed_Tags++; }
                }
            }
            catch
            {   Current_Line = Add_Line + "    The last line was:  " + Current_Line + Add_Line; 
                if (Failed_Tags > 0) { Current_Line +=  "Failed in " + Failed_Tags + " entries." + Add_Line; }              
                Inform("Crashed listing of " + Path.GetFileName(Second_File + The_Extension) + Current_Line); return "";
            }

      

            try // Checking the selected file
            {   IEnumerable<string> Selected_Lines = File.ReadLines(Selected_File + The_Extension);

                if (!Selected_Lines.First().Contains(Delimiter) & !Selected_Lines.Last().Contains(Delimiter))
                { Inform("Delimiter Error. Please reconvert one of their 2 .dat to have the same Delimiter - or change the sign in the UI."); return ""; }


                foreach (string Line in Selected_Lines)
                {                  
                    // 1 = Keys, 2 = Values, 3 = Sync Values into File
                    if (Compare_Mode != 1) // 2 or 3 String comparsion is slower then Key comparsion, because of longer Char arrays
                    {   Found_In_Values = false;
                       
                        foreach (string Entry in Value_Cache)
                        {
                            if (Entry == Line.Split(Delimiter)[1]) { Found_In_Values = true; break; }
                        }

                        if (Found_In_Values == false) { The_Text += Line + Add_Line; Result_Cache.Add(Line); } // Appending to the matched values
                    }
                    else if (Compare_Mode != 2) // 1 or 3 From the Keys that match in both files we test whether the Values are identical
                    {   Found_In_Keys = false;
                   
                        foreach (string Entry in Key_Cache)
                        {   // Split by Delimiter and get Slot 0 of the resulting array as Current_Key
                            
                            //if (System.Text.RegularExpressions.Regex.IsMatch(Line.Split(Delimiter)[0], "(?i).*?" + Entry)) // + ".*?"))
                            // Better not use Regex, it takes just way too long...
                            if (Entry == Line.Split(Delimiter)[0]) { Found_In_Keys = true; break; } // Case Sensitive
                          
                        }

                        // If not matched we know it is a user generated string cause it didn't matched.
                        if (Found_In_Keys == false) { The_Text += Line + Add_Line; Result_Cache.Add(Line); }
                    }


                }
            } catch { Inform("Something crashed the comparsion between the Files."); return ""; }



          

            // ==================== Synchronizing Entry Table ====================
            string Result_Extension = "_Difference" + The_Extension;
                           
            if (Compare_Mode == 3)
            {   The_Text = ""; // Resetting
                Result_Extension = "_Synced" + The_Extension;

                try // UPDATE EXISTING ENTRIES
                {   foreach (string Line in Line_Cache) // From First File
                    {   Found_In_Keys = false;

                        foreach (string Result in Result_Cache) // Of the Second File
                        {  
                            if (Result.Split(Delimiter)[0] == Line.Split(Delimiter)[0]) 
                            {   Found_In_Keys = true;
                               
                                The_Text += Result + Add_Line; // If Key found we Choose the Result
                                break;
                            }                          
                        }
                        if (Found_In_Keys == false) { The_Text += Line + Add_Line; } // Choose Original Line 
                    }
                } catch { Inform("Function that updates existing entries has crashed."); return ""; }



                try // APPEND MISSING ENTRIES
                {   foreach (string Result in Result_Cache)                  
                    {   Found_In_Keys = false;
                        
                        foreach (string Line in Line_Cache) 
                        {   // Split by Delimiter and get Slot 0
                            if (Result.Split(Delimiter)[0] == Line.Split(Delimiter)[0]) { Found_In_Keys = true; break; }
                        }
                        if (Found_In_Keys == false) { The_Text += Result + Add_Line; }
                    }
                } catch { Inform("Function that appends missing entries has crashed."); return ""; }
            }


            if (The_Text == "") // Using the Results we collected
            {   string Query_Type = "Values";
                if (Compare_Mode == 2) { Query_Type = "Keys"; }
                Inform("No " + Query_Type + " found."); // MessageBox.Show();
            }
            else 
            { 
                File.WriteAllText(Selected_File + Result_Extension, The_Text);
                return Selected_File + Result_Extension;
            }

            return ""; 
        }




        // =================================== End of File ===================================
    }
}
