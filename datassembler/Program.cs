using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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

    class Program
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

        static List<Text_Entry> ReadDat(string source, char delimiter, int sorttype)
        {
            List<Text_Entry> entries = new List<Text_Entry>();
            if (!File.Exists(source))
            {
                Console.WriteLine("File " + source + " not found");
                return entries;
            }

            byte[] datfile = System.IO.File.ReadAllBytes(source);

            int total_entries = make32(datfile, 0);
            int index = 4;

            int[] textlength = new int[total_entries];
            int[] idlength = new int[total_entries];

            for (int i = 0; i < total_entries; i++)
            {
                Text_Entry nuevo = new Text_Entry();
                nuevo.crc = (uint)make32(datfile, index);
                entries.Add(nuevo);
                index += 4;
                textlength[i] = make32(datfile, index);
                index += 4;
                idlength[i] = make32(datfile, index);
                index += 4;
            }
            for (int i = 0; i < total_entries; i++)
            {
                entries[i].entry = readentry(datfile, index, textlength[i]);
                index += 2 * textlength[i];
            }
            for (int i = 0; i < total_entries; i++)
            {
                entries[i].identifier = readid(datfile, index, idlength[i]);
                index += idlength[i];
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

            if (sorttype != 2)
            {
                entries.Sort();
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

            return entries;
        }

        static void writeTxt(List<Text_Entry> entries, char delimiter, string dest)
        {
            using (
                    var sw = new StreamWriter(
                        new FileStream(dest, FileMode.Create, FileAccess.ReadWrite),
                        Encoding.UTF8
                    )
                )
            {
                for (int i = 0; i < entries.Count; i++) sw.WriteLine(entries[i].identifier + delimiter + entries[i].entry);
            }
        }

        static void decompileDat(string source, string dest, char delimiter, int sorttype)
        {
            List<Text_Entry> entries = ReadDat(source, delimiter, sorttype);
            if (entries.Count == 0) {
                return;
            }

            writeTxt(entries, delimiter, dest);
        }

        static List<Text_Entry> ReadText(string[][] sources, char delimiter)
        {
            uint total_entries = 0;
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
                                else
                                {
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
                    int id;
                    uint start_entries = total_entries;
                    for (int i = 0; i < level_entries; i++)
                    {
                        id = -1;
                        int result = -1;
                        do
                        {
                            id++;
                            if (id >= start_entries)
                            {
                                break;
                            }
                            else
                            {
                                result = string.Compare(Level_Entries[i].identifier, Entries[id].identifier, StringComparison.OrdinalIgnoreCase);
                            }
                        } while (result != 0);
                        if (result == 0)
                        {
                            Entries[id].entry = Level_Entries[i].entry;
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

            return Entries;
        }



        static void compileDat(string[][] sources, string outFile, char delimiter) {
            List<Text_Entry> Entries = ReadText(sources, delimiter);
            uint total_entries = (uint)Entries.Count;

            File.Delete(outFile);
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

        //Despite the double arrays to reuse broader code, this is intended for use on single text files, especially for the target
        static void syncTexts(string[][] source, string[][] target, char sdelimiter, char tdelimiter)
        {
            List<Text_Entry> sources = ReadText(source, sdelimiter);
            List<Text_Entry> targets = ReadText(target, tdelimiter);
            List<Text_Entry> output = new List<Text_Entry>();

            sources.Sort();
            targets.Sort();

            int tIndex = 0;

            //loop through source. For each entry, loop through targets starting at tIndex.
            for (int sIndex = 0; sIndex < sources.Count; sIndex++)
            {
                Boolean dontgotit = true;
                for (int i = tIndex; i < targets.Count; i++)
                {
                    if (sources[sIndex].identifier == targets[i].identifier)
                    {
                        output.Add(targets[i]);
                        tIndex = i + 1;
                        dontgotit = false;
                    }
                }
                if (dontgotit)
                {
                    Text_Entry nuevo = new Text_Entry
                    {
                        crc = sources[sIndex].crc,
                        identifier = sources[sIndex].identifier,
                        entry = "TRANSLATE:"+sources[sIndex].entry
                    };
                    output.Add(nuevo);
                }
            }

            writeTxt(output, tdelimiter, target[0][0]);
        }

        static List<Text_Entry> compareEntries(List<Text_Entry> sources, List<Text_Entry> targets, Boolean Track_Deletions)
        {
            List<Text_Entry> output = new List<Text_Entry>();
            sources.Sort();
            targets.Sort();
            int tIndex = 0;

            for (int sIndex = 0; sIndex < sources.Count; sIndex++)
            {
                Boolean dontgotit = true;
                for (int i = tIndex; i < targets.Count; i++)
                {
                    if (sources[sIndex].identifier == targets[i].identifier)
                    {
                        if (sources[sIndex].entry != targets[i].entry)
                        {
                            output.Add(sources[sIndex]);
                            tIndex = i + 1;
                        }
                        dontgotit = false;
                    }
                }
                if (dontgotit)
                {
                    output.Add(sources[sIndex]);
                }
            }

            if (Track_Deletions)
            {
                int s = 0;
                for (int t = 0; t < targets.Count; t++)
                {
                    Boolean dontgotit = true;
                    for (int i = s; i < sources.Count; i++)
                    {
                        if (targets[t].identifier == sources[i].identifier)
                        {
                            s = i + 1;
                            dontgotit = false;
                        }
                    }
                    if (dontgotit)
                    {
                        Text_Entry nuevo = new Text_Entry
                        {
                            crc = targets[t].crc,
                            identifier = targets[t].identifier,
                            entry = ""
                        };
                        output.Add(nuevo);
                    }
                }
            }

            return output;
        }

        static void compareDats(string source, string target, char delimiter)
        {
            List<Text_Entry> sources = ReadDat(source, delimiter, 0);
            List<Text_Entry> targets = ReadDat(target, delimiter, 0);

            List<Text_Entry> output = compareEntries(sources, targets, true);

            writeTxt(output, delimiter, "compare-output.txt");
        }

        static void compareTranslate(string[][] sources, string target, char delimiter)
        {
            List<Text_Entry> entries = ReadText(sources, delimiter);
            List<Text_Entry> targets = ReadDat(target, delimiter, 0);

            List<Text_Entry> output = compareEntries(entries, targets, false);

            List<Text_Entry>[][] filevalues = new List<Text_Entry>[sources.Length][];
            List<Text_Entry>[][] filedeltas = new List<Text_Entry>[sources.Length][];

            for (int i = 0; i<sources.Length; i++)
            {
                filevalues[i] = new List<Text_Entry>[sources[i].Length];
                filedeltas[i] = new List<Text_Entry>[sources[i].Length];
                for (int j = 0; j < sources[i].Length; j++)
                {
                    string[][] sArray = new string[1][];
                    sArray[0] = new string[1];
                    sArray[0][0] = sources[i][j];
                    filevalues[i][j] = ReadText(sArray, delimiter);
                    filedeltas[i][j] = new List<Text_Entry>();
                }
            }

            foreach (Text_Entry entry in output)
            {
                for (int i = sources.Length-1; i >= 0; i--)
                {
                    for (int j = sources[i].Length - 1; j >= 0; j--)
                    {
                        foreach (Text_Entry entry2 in filevalues[i][j])
                        {
                            if (entry.identifier == entry2.identifier)
                            {
                                filedeltas[i][j].Add(entry);
                                i = 0; //break after finding
                                j = 0;
                                break;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < sources.Length; i++)
            {
                for (int j = 0; j < sources[i].Length; j++)
                {
                    if (filedeltas[i][j].Count > 0)
                    {
                        string filename = sources[i][j];
                        if (filename.Contains("."))
                        {
                            filename = filename.Substring(0, filename.LastIndexOf("."));
                        }
                        filename += "-to-translate.txt";

                        if (File.Exists(filename))
                        {
                            string[][] wrapper = new string[1][];
                            wrapper[0] = new string[1];
                            wrapper[0][0] = filename;
                            List<Text_Entry> olddeltas = ReadText(wrapper, delimiter);

                            foreach (Text_Entry outer in olddeltas)
                            {
                                Boolean nothere = true;
                                foreach (Text_Entry inner in filedeltas[i][j])
                                {
                                    if (outer.identifier == inner.identifier)
                                    {
                                        nothere = false;
                                        break;
                                    }
                                }
                                if (nothere)
                                {
                                    filedeltas[i][j].Add(outer);
                                }
                            }
                        }
                        filedeltas[i][j].Sort();
                        writeTxt(filedeltas[i][j], delimiter, filename);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 0) {
                args = new string[1];
                args[0] = "/b";
            }
            char delimiter = ',';
            char outdelimiter = ',';
            bool nooutset = true;

            string currentDirectory = Directory.GetCurrentDirectory();
            string sourceFile = "MasterTextFile_ENGLISH.txt";
            string outFile = "MasterTextFile_ENGLISH.dat";
            Boolean default_source = true;
            Boolean default_out = true;

            if (args.Length > 1) {
                if (File.Exists(args[1])) {
                    sourceFile = args[1];
                    default_source = false;
                    if (args.Length > 2 && !(args[2].Substring(0, 1).Contains("-"))) {
                        outFile = args[2];
                        default_out = false;
                    }
                }
            }

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

            string[][] sources = new string[1][];
            sources[0] = new string[1];
            sources[0][0] = sourceFile;
            int csvid = 0;
            int csvid2 = 1;

            for (Int64 i = 1; i < args.Length; i++)
            {
                if (args[i].Substring(0, 1) == "-")
                {
                    if (args[i].Substring(1, 1) == "a")
                    {
                        Array.Resize(ref sources[csvid], csvid2 + 1);
                        sources[csvid][csvid2] = args[i].Substring(3, args[i].Length - 3);
                        csvid2++;
                    }
                    if (args[i].Substring(1, 1) == "r")
                    {
                        csvid++;
                        csvid2 = 0;
                        Array.Resize(ref sources, csvid + 1);
                        sources[csvid] = new string[1];
                        sources[csvid][csvid2] = args[i].Substring(3, args[i].Length - 3);
                    }
                }
            }

            switch (args[0]) {
                case "/h":
                case "\\h":
                    Console.WriteLine();
                    Console.WriteLine("Petroglyph dat file assembler 2.A");
                    Console.WriteLine("  by Jorritkarwehr   April 2023");
                    Console.WriteLine("  build dat: </b txtfile datfile -s:; -a:txt2 -a:txt3... -r:txt4>");
                    Console.WriteLine("  export txt: /e <datfile txtfile -s:; -v -c>");
                    Console.WriteLine("  alphabetize/clean: /a <txtfile -v -n -t -s:; -o:,>");
                    Console.WriteLine("  sync txt: /s <source target -s:; -o:,>");
                    Console.WriteLine("  compare dat: /c <source target -s:;>");
                    Console.WriteLine("  translation compare: /t <txtfile datfile -s:; -a:txt2 -a:txt3... -r:txt4>");
                    Console.WriteLine("  extra documentation: /d");
                    Console.WriteLine();
                    Console.WriteLine("-s: separator -a: additional txt -r: replacing txt -v: sort by value -c: sort by CRC -n: no collision file -t: no trailing separator trim -o: output separator");
                    Console.WriteLine();
                    break;
                case "/b":
                case "\\b":
                    compileDat(sources, outFile, delimiter);
                    break;
                case "/e":
                case "\\e":
                    if (default_source)
                    {
                        sourceFile = "MasterTextFile_ENGLISH.dat";
                    }
                    if (default_out)
                    {
                        outFile = "MasterTextFile_ENGLISH.txt";
                    }
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

                    decompileDat(sourceFile, outFile, delimiter, sorttype);
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
                case "/s":
                case "\\s":
                    string[][] sArray = new string[1][];
                    sArray[0] = new string[1];
                    sArray[0][0] = sourceFile;

                    string[][] tArray = new string[1][];
                    tArray[0] = new string[1];
                    tArray[0][0] = outFile;

                    syncTexts(sArray, tArray, delimiter, outdelimiter);
                    break;
                case "/c":
                case "\\c":
                    compareDats(sourceFile, outFile, delimiter);
                    break;
                case "/t":
                case "\\t":
                    compareTranslate(sources, outFile, delimiter);
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

                    Console.WriteLine("Sync (/s) sets the collection of identifiers in the target to match the source");
                    Console.WriteLine("Extra identifiers in the target will be deleted, missing ones will be added with a TRANSLATE:sourcestring value");
                    Console.WriteLine("Values for identifiers in both will not be changed.");
                    Console.WriteLine();

                    Console.WriteLine("Compare dat (/c) runs a comparison on two dat files and writes the changes to compare-output.txt");
                    Console.WriteLine("Changes and entries only in the source will be written in full.");
                    Console.WriteLine("Entries only in the target will have have an empty string value.");
                    Console.WriteLine();

                    Console.WriteLine("Translation compare (/t) runs a comparison on a txt file (or set of files) and dat file");
                    Console.WriteLine("The changes specific to each text file are written to txtfilename-to-translate.txt");
                    Console.WriteLine("If the log file already exists, changes will be merged with the old file contents");
                    break;
                default:
                    Console.WriteLine("Unrecognized command. Use /h for a command list");
                    break;
            }
        }
    }
}
