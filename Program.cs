using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using ManyConsole;
using MaiLib;

namespace MaichartConverter
{
    /// <summary>
    /// Main program of converter
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines Windows path separator
        /// </summary>
        public static string WindowsPathSep = "\\";

        /// <summary>
        /// Defines MacOS path separator
        /// </summary>
        public static string MacPathSep = "/";


        public static string[] WinPaths = { @"C:\Users\Neskol\MaiAnalysis\A000\",
        @"C:\Users\Neskol\MaiAnalysis\Sound\",
        @"C:\Users\Neskol\MaiAnalysis\Image\Texture2D\",
        @"C:\Users\Neskol\MaiAnalysis\DXBGA_HEVC\",
        @"C:\Users\Neskol\MaiAnalysis\Output\"};

        public static string[] MacPaths = { @"/Users/neskol/MaiAnalysis/A000/",
        @"/Users/neskol/MaiAnalysis/Sound/",
        @"/Users/neskol/MaiAnalysis/Image/Texture2D/",
        @"/Users/neskol/MaiAnalysis/DXBGA_HEVC/",
        @"/Users/neskol/MaiAnalysis/Output/"};

        /// <summary>
        /// Defines which separator is using in programs
        /// </summary>
        public static string GlobalSep = MacPathSep;

        /// <summary>
        /// Defines which path is using in programs
        /// </summary>
        public static string[] GlobalPaths = MacPaths;

        /// <summary>
        /// Defines possible sorting scheme
        /// </summary>
        /// <value>Sorting scheme</value>
        public static readonly string[] TrackCategorizeMethodSet = { "Genre", "Level", "Cabinet", "Composer", "BPM", "SD//DX Chart", "No Separate Folder" };

        /// <summary>
        /// Program will sort output according to this
        /// </summary>
        public static string GlobalTrackCategorizeMethod = TrackCategorizeMethodSet[0];

        /// <summary>
        /// Records total track number compiled by program
        /// </summary>
        public static int NumberTotalTrackCompiled;
        public static Dictionary<int, string> CompiledTracks = new();
        public static List<string> CompiledChart = new();

        public static List<string> ErrorMessage = new();
        public static Dictionary<string, string[]> CompiledTrackDetailSet = new Dictionary<string, string[]>();

        public static XmlDocument BPMCollection = new XmlDocument();
        public static XmlDocument DebugInformationTable = new XmlDocument();

        /// <summary>
        /// Main method to process charts
        /// </summary>
        /// <param name="args">Parameters to take in</param>
        public static int Main(string[] args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                GlobalPaths = WinPaths;
                GlobalSep = WindowsPathSep;
            }
            else
            {
                GlobalPaths = MacPaths;
                GlobalSep = MacPathSep;
            }

            Console.WriteLine(ComposeHeader());

            XmlDeclaration xmlDecl = BPMCollection.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlDeclaration xmlDeclBPM = DebugInformationTable.CreateXmlDeclaration("1.0", "UTF-8", null);
            BPMCollection.AppendChild(xmlDecl);
            DebugInformationTable.AppendChild(xmlDeclBPM);
            XmlElement root = BPMCollection.CreateElement("BPM");
            XmlElement btRoot = DebugInformationTable.CreateElement("BPM-Table");
            BPMCollection.AppendChild(root);
            DebugInformationTable.AppendChild(btRoot);

            //TestSpecificChart();
            //TestSpecificChart(@"D:\PandoraCandidate.ma2");
            //TestSpecificChart("000389", "4");
            //CompileChartDatabase();
            //CompileAssignedChartDatabase();
            // CompileUtageChartDatabase();
            //}
            // return 0;
            var commands = GetCommands();
            return ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }

        /// <summary>
        /// Get commands
        /// </summary>
        /// <returns>ProperCommands</returns>
        public static IEnumerable<ConsoleCommand> GetCommands()
        {
            return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
        }

        /// <summary>
        /// Compensate 0 for music IDs
        /// </summary>
        /// <param name="intake">Music ID</param>
        /// <returns>0..+#Music ID and |Music ID|==6</returns>
        public static string CompensateZero(string intake)
        {
            try
            {
                string result = intake;
                while (result.Length < 6 && intake != null)
                {
                    result = "0" + result;
                }
                return result;
            }
            catch (NullReferenceException ex)
            {
                return "Exception raised: " + ex.Message;
            }
        }

        /// <summary>
        /// Compensate 0 for short music IDs
        /// </summary>
        /// <param name="intake">Music ID</param>
        /// <returns>0..+#Music ID and |Music ID|==4</returns>
        public static string CompensateShortZero(string intake)
        {
            try
            {
                string result = intake;
                while (result.Length < 4 && intake != null)
                {
                    result = "0" + result;
                }
                return result;
            }
            catch (NullReferenceException ex)
            {
                return "Exception raised: " + ex.Message;
            }
        }

        /// <summary>
        /// Compose fancy header
        /// </summary>
        /// <returns>MaichartConverter.fancy</returns>
        public static string ComposeHeader()
        {
            string result = "";
            result += (@"     _____         .__       .__                   __   _________                                   __                " + "\n");
            result += (@"    /     \ _____  |__| ____ |  |__ _____ ________/  |_ \_   ___ \  ____   _______  __ ____________/  |_  ___________ " + "\n");
            result += (@"   /  \ /  \\__  \ |  |/ ___\|  |  \\__  \\_  __ \   __\/    \  \/ /  _ \ /    \  \/ // __ \_  __ \   __\/ __ \_  __ \" + "\n");
            result += (@"  /    Y    \/ __ \|  \  \___|   Y  \/ __ \|  | \/|  |  \     \___(  <_> )   |  \   /\  ___/|  | \/|  | \  ___/|  | \/" + "\n");
            result += (@"  \____|__  (____  /__|\___  >___|  (____  /__|   |__|   \______  /\____/|___|  /\_/  \___  >__|   |__|  \___  >__|   " + "\n");
            result += (@"          \/     \/        \/     \/     \/                     \/            \/          \/                 \/       " + "\n");
            result += "a GUtils component for rhythm games\n";
            result += "Rev " + Assembly.GetExecutingAssembly().GetName().Version + " by Neskol\n";
            result += "Check https://github.com/Neskol/MaichartConverter for updates and instructions\n";
            return result;
        }

        /// <summary>
        /// Log to given position.
        /// </summary>
        /// <param name="outputLocation">Place to log</param>
        public static void Log(string outputLocation)
        {
            StreamWriter sw = new StreamWriter(outputLocation + "log.txt", false);
            // sw.WriteLine("Total music compiled: " + NumberTotalTrackCompiled);
            int index = 1;
            // sw.WriteLine("Index\tTitle\tGenre\tVersion\tPatch Number");
            // foreach (KeyValuePair<int,string> pair in CompiledTracks)
            // {
            //     string[]? compiledDetailArray = new string[0];
            //     CompiledTrackDetailSet.TryGetValue(pair.Key.ToString(), out compiledDetailArray);
            //     if (compiledDetailArray == null)
            //     {
            //         compiledDetailArray = new string[0];
            //     }
            //     sw.WriteLine("[" + index + "]\t" + compiledDetailArray[0] + "\t" + compiledDetailArray[1] + "\t" + compiledDetailArray[2] + "\t" + compiledDetailArray[3] ?? throw new NullReferenceException());
            //     index++;
            // }
            index = 1;

            sw.WriteLine("Total chart compiled: " + CompiledChart.Count);
            foreach (string title in CompiledChart)
            {
                sw.WriteLine("[" + index + "]\t" + title);
                index++;
            }
            sw.WriteLine();
            if (ErrorMessage.Count()>0)
            {
                sw.WriteLine("Warnings:");
                foreach (string error in ErrorMessage)
                {
                    sw.WriteLine(error);
                }
            }
            sw.Close();
            BPMCollection.Save(outputLocation + "bpm.xml");
            DebugInformationTable.Save(outputLocation + "debug.xml");
        }

        /// <summary>
        /// Append Nodes to BPMCollection
        /// </summary>
        /// <param name="idValue">ID</param>
        /// <param name="bpmValue">BPM</param>
        public static void AppendBPM(string idValue, string bpmValue)
        {
            XmlElement node = BPMCollection.CreateElement("Node");
            XmlElement id = BPMCollection.CreateElement("ID");
            id.InnerText = idValue;
            XmlElement bpm = BPMCollection.CreateElement("BPM");
            bpm.InnerText = bpmValue;
            node.AppendChild(id);
            node.AppendChild(bpm);
            XmlNode root = BPMCollection.ChildNodes[1] ?? throw new NullReferenceException();
            root.AppendChild(node);
        }

        /// <summary>
        /// Append debug information to save
        /// </summary>
        /// <param name = "idValue">ID of the chart</param>
        /// <param name="bpmTable">BPMTable</param>
        /// <param name="firstNoteValue">First note of the Master Chart</param>
        public static void AppendDebugInformation(string idValue, BPMChanges bpmTable, Note firstNoteValue)
        {
            XmlElement node = DebugInformationTable.CreateElement("Node");
            XmlElement id = DebugInformationTable.CreateElement("ID");
            id.InnerText = idValue;
            XmlElement bpm = DebugInformationTable.CreateElement("BPM");
            XmlElement firstNote = DebugInformationTable.CreateElement("FirstNote");
            XmlElement firstNoteType = DebugInformationTable.CreateElement("Type");
            firstNote.AppendChild(firstNoteType);
            Console.WriteLine(firstNoteValue.ToString());
            node.AppendChild(id);
            node.AppendChild(bpm);
            node.AppendChild(firstNote);
            foreach (BPMChange note in bpmTable.ChangeNotes)
            {
                XmlElement changeNote = DebugInformationTable.CreateElement("Note");
                XmlElement bar = DebugInformationTable.CreateElement("Bar");
                bar.InnerText = note.Bar.ToString();
                XmlElement tick = DebugInformationTable.CreateElement("Tick");
                tick.InnerText = note.Tick.ToString();
                XmlElement noteBPM = DebugInformationTable.CreateElement("BPM");
                noteBPM.InnerText = note.BPM.ToString();
                changeNote.AppendChild(bar);
                changeNote.AppendChild(tick);
                changeNote.AppendChild(noteBPM);
                bpm.AppendChild(changeNote);
            }
            XmlNode root = DebugInformationTable.ChildNodes[1] ?? throw new NullReferenceException();
            root.AppendChild(node);
        }
    }


}
