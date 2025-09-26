using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml;
using MaiLib;
using ManyConsole;

namespace MaichartConverter
{
    /// <summary>
    ///     Main program of converter
    /// </summary>
    class Program
    {
        protected static string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        /// <summary>
        ///     Defines which path is using in programs
        /// </summary>
        public static string[] DefaultPaths =
        [
            $"{home}/MaiAnalysis/A000/",
            $"{home}/MaiAnalysis/Sound/",
            $"{home}/MaiAnalysis/Image/Texture2D/",
            $"{home}/MaiAnalysis/DXBGA_HEVC/",
            $"{home}/MaiAnalysis/Output/"
        ];

        /// <summary>
        ///     Defines possible sorting scheme
        /// </summary>
        /// <value>Sorting scheme</value>
        public static readonly string[] TrackCategorizeMethodSet =
            ["Genre", "Level", "Cabinet", "Composer", "BPM", "SD/DX Chart", "No Separate Folder"];

        /// <summary>
        ///     Program will sort output according to this
        /// </summary>
        public static string GlobalTrackCategorizeMethod = TrackCategorizeMethodSet[0];

        /// <summary>
        ///     Records total track number compiled by program
        /// </summary>
        public static int NumberTotalTrackCompiled;

        public static Dictionary<int, string> CompiledTracks = [];
        public static List<string> CompiledChart = [];

        public static List<string> ErrorMessage = [];
        public static Dictionary<string, string[]> CompiledTrackDetailSet = [];
        public static List<TrackInformation> CompiledTrackInformationList = [];

        public static XmlDocument BPMCollection = new();
        public static XmlDocument DebugInformationTable = new();

        public class TrackGroup
        {
            public string? name { get; set; }
            public string[]? levelIds { get; set; }

            protected internal TrackGroup()
            {
            }
        }

        /// <summary>
        ///     Main method to process charts
        /// </summary>
        /// <param name="args">Parameters to take in</param>
        public static int Main(string[] args)
        {
            Console.WriteLine(ComposeHeader());

            // XmlDeclaration xmlDecl = BPMCollection.CreateXmlDeclaration("1.0", "UTF-8", null);
            // XmlDeclaration xmlDeclBPM = DebugInformationTable.CreateXmlDeclaration("1.0", "UTF-8", null);
            // BPMCollection.AppendChild(xmlDecl);
            // DebugInformationTable.AppendChild(xmlDeclBPM);
            // XmlElement root = BPMCollection.CreateElement("BPM");
            // XmlElement btRoot = DebugInformationTable.CreateElement("BPM-Table");
            // BPMCollection.AppendChild(root);
            // DebugInformationTable.AppendChild(btRoot);

            //TestSpecificChart();
            //TestSpecificChart(@"D:\PandoraCandidate.ma2");
            //TestSpecificChart("000389", "4");
            //CompileChartDatabase();
            //CompileAssignedChartDatabase();
            // CompileUtageChartDatabase();
            //}
            // return 0;
            IEnumerable<ConsoleCommand>? commands = GetCommands();
            // foreach (ConsoleCommand x in commands)
            // {
            //     Console.WriteLine(x.Command.ToString());
            // }
            // Console.WriteLine(string.Join(' ',args));
            return ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
            // else
            // {
            //     ConsoleCommand reverseDatabase = new ReverseMa2FromSimaiDatabase();
            //     return reverseDatabase.Run(args);
            // }
        }

        /// <summary>
        ///     Get commands
        /// </summary>
        /// <returns>ProperCommands</returns>
        public static IEnumerable<ConsoleCommand> GetCommands()
        {
            return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
        }

        /// <summary>
        ///     Compensate 0 for music IDs
        /// </summary>
        /// <param name="intake">Music ID</param>
        /// <returns>0..+#Music ID and |Music ID|==6</returns>
        public static string CompensateZero(string intake)
        {
            try
            {
                string result = intake;
                while (result.Length < 6 && intake != null) result = "0" + result;

                return result;
            }
            catch (NullReferenceException ex)
            {
                return "Exception raised: " + ex.Message;
            }
        }

        /// <summary>
        ///     Compensate 0 for short music IDs
        /// </summary>
        /// <param name="intake">Music ID</param>
        /// <returns>0..+#Music ID and |Music ID|==4</returns>
        public static string CompensateShortZero(string intake)
        {
            try
            {
                string result = intake;
                while (result.Length < 4 && intake != null) result = "0" + result;

                return result;
            }
            catch (NullReferenceException ex)
            {
                return "Exception raised: " + ex.Message;
            }
        }

        /// <summary>
        ///     Compose fancy header
        /// </summary>
        /// <returns>MaichartConverter.fancy</returns>
        public static string ComposeHeader()
        {
            string result = "";
            result +=
                @"     _____         .__       .__                   __   _________                                   __                " +
                "\n";
            result +=
                @"    /     \ _____  |__| ____ |  |__ _____ ________/  |_ \_   ___ \  ____   _______  __ ____________/  |_  ___________ " +
                "\n";
            result +=
                @"   /  \ /  \\__  \ |  |/ ___\|  |  \\__  \\_  __ \   __\/    \  \/ /  _ \ /    \  \/ // __ \_  __ \   __\/ __ \_  __ \" +
                "\n";
            result +=
                @"  /    Y    \/ __ \|  \  \___|   Y  \/ __ \|  | \/|  |  \     \___(  <_> )   |  \   /\  ___/|  | \/|  | \  ___/|  | \/" +
                "\n";
            result +=
                @"  \____|__  (____  /__|\___  >___|  (____  /__|   |__|   \______  /\____/|___|  /\_/  \___  >__|   |__|  \___  >__|   " +
                "\n";
            result +=
                @"          \/     \/        \/     \/     \/                     \/            \/          \/                 \/       " +
                "\n";
            result += "a GUtils component for rhythm games\n";
            result += "Rev. " + Assembly.GetExecutingAssembly().GetName().Version + " by Neskol\n";
            result += "Check https://github.com/Neskol/MaichartConverter for updates and instructions\n";
            return result;
        }

        /// <summary>
        ///     Log to given position.
        /// </summary>
        /// <param name="outputLocation">Place to log</param>
        public static void Log(string outputLocation)
        {
            StreamWriter sw = new(outputLocation + "_log.txt", false);
            int index = 1;

            Console.WriteLine("Total music compiled: {0}", NumberTotalTrackCompiled);
            foreach (KeyValuePair<int, string> pair in CompiledTracks)
            {
                sw.WriteLine($"[{index}]: {pair.Key} {pair.Value}");
                index++;
            }

            sw.WriteLine();
            if (ErrorMessage.Count > 0)
            {
                sw.WriteLine("Warnings:");
                foreach (string error in ErrorMessage) sw.WriteLine(error);
            }

            sw.Close();
            // BPMCollection.Save(outputLocation + "bpm.xml");
            // DebugInformationTable.Save(outputLocation + "debug.xml");
        }

        /// <summary>
        ///     Log to given position in Json format.
        /// </summary>
        /// <param name="outputLocation">Place to log</param>
        public static void LogTracksInJson(string outputLocation)
        {
            StreamWriter sw = new(outputLocation + "_index.json", false);
            JsonSerializerOptions? JsonOptions = new()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true,
            };
            sw.WriteLine(JsonSerializer.Serialize(new SortedDictionary<int, string>(CompiledTracks), JsonOptions));
            sw.Close();
        }

        /// <summary>
        ///     Compile a sorting collection file for output
        /// </summary>
        /// <param name="outputLocation">Place to save</param>
        public static void CompileSortingCollection(string outputLocation)
        {
            JsonSerializerOptions? JsonOptions = new()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true,
            };
            string[] trackVersions =
                CompiledTrackInformationList.Select(info => info.TrackVersion).Distinct().ToArray();
            string[] trackGenres = CompiledTrackInformationList.Select(info => info.TrackGenre).Distinct().ToArray();
            string[] trackSDDXPrefixes = CompiledTrackInformationList.Select(info => info.StandardDeluxePrefix)
                .Distinct().ToArray();

            List<TrackGroup> trackGroups = [];

            trackGroups.AddRange(trackVersions.Select(trackVersion => new TrackGroup
            {
                name = trackVersion,
                levelIds = (from track in CompiledTrackInformationList
                    where track.TrackVersion.Equals(trackVersion)
                    select track.TrackID).ToArray()
            }));
            trackGroups.AddRange(trackGenres.Select(trackGenre => new TrackGroup
            {
                name = trackGenre is "maimai" ? "Original" : trackGenre,
                levelIds = (from track in CompiledTrackInformationList
                    where track.TrackGenre.Equals(trackGenre)
                    select track.TrackID).ToArray()
            }));
            trackGroups.AddRange(trackSDDXPrefixes.Select(prefix => new TrackGroup
            {
                name = $"{prefix} Chart",
                levelIds = (from track in CompiledTrackInformationList
                    where track.StandardDeluxePrefix.Equals(prefix)
                    select track.TrackID).ToArray()
            }));

            foreach (TrackGroup group in trackGroups)
            {
                Directory.CreateDirectory($"{outputLocation}/{group.name}");
                StreamWriter sw = new($"{outputLocation}/{group.name}/manifest.json", false);
                sw.WriteLine(JsonSerializer.Serialize(group, JsonOptions));
                sw.Close();
            }
        }

        /// <summary>
        ///     Append Nodes to BPMCollection
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
        ///     Append debug information to save
        /// </summary>
        /// <param name="idValue">ID of the chart</param>
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
