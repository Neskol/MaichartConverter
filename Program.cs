using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml;
using ManyConsole;
using Mono.Options;
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

            //string simaiChart = "";
            //string ma2Chart = "";
            //string ma2ChartID = "";
            //string difficulty = "";
            //string a000Location = "";
            //string imageLocation = "";
            //string bgaLocation = "";
            //string musicLocation = "";
            //string outputLocation = "";
            //string categorize_method = "";
            //bool compileAllChart = false;
            //bool utageChart = false;
            //bool exportBGA = false;

            //foreach (string s in args)
            //{
            //    Console.WriteLine(s);
            //}



            //if (args.Length == 0)
            //{
            //   Console.WriteLine("usage: MaichartConverter [-s --simai simai_chart] [-m --ma2 ma2_chart] [-i --id ma2_chart_id]" +
            //       "[-d --diff difficulty] [-a --all compile_all_chart] [-u --all-utage compile_all_] " +
            //       "[-s --source override_a000_path] [-p --pic override_pic_path] [-v --video override_video_path] " +
            //       "[-b --bgm override_bgm_path] [-o --output override_output_pat] [-c --category categorize_method] [-e --export-bga export bga]");
            //}

            //for (int i = 0; i < args.Count(); i++)
            //{
            //    switch (args[i])
            //    {
            //        case "-s":
            //        case "--simai":
            //            if (i + 1 < args.Length && File.Exists(args[i+1]))
            //            {
            //                string path = args[i + 1];
            //                TestSpecificChart(path);
            //            }
            //            break;
            //        case "-m":
            //        case "--ma2":
            //            if (i + 1 < args.Length && File.Exists(args[i + 1]))
            //            {
            //                string path = args[i + 1];
            //                TestSpecificChart(path);
            //            }
            //            break;
            //        case "-i":
            //        case "--id":
            //            if (i + 1 < args.Length && File.Exists(args[i + 1]))
            //            {
            //                string path = args[i + 1];
            //                TestSpecificChart(path);
            //            }
            //            break;
            //    }

            //TestSpecificChart();
            //TestSpecificChart(@"D:\PandoraCandidate.ma2");
            //TestSpecificChart("000389", "4");
            //CompileChartDatabase();
            //CompileAssignedChartDatabase();
            //CompileUtageChartDatabase();
            //}
            //return 0;
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
        /// Compile Simai Command
        /// </summary>
        public class CompileSimai:ConsoleCommand
        {
            /// <summary>
            /// Retrurn when command successfully executed
            /// </summary>
            private const int Success = 0;
            /// <summary>
            /// Retrurn when command failed to execute
            /// </summary>
            private const int Failed = 2;

            /// <summary>
            /// Source file path
            /// </summary>
            public string? FileLocation { get; set; }
            /// <summary>
            /// Difficulty
            /// </summary>
            public string? Difficulty { get; set; }
            /// <summary>
            /// Destination of output
            /// </summary>
            public string? Destination { get; set; }
            /// <summary>
            /// Target Format of the file
            /// </summary>
            public string? TargetFormat { get; set; }

            /// <summary>
            /// Construct Command
            /// </summary>
            public CompileSimai()
            {
                IsCommand("CompileSimai", "Compile assigned simai chart to assigned format");
                HasLongDescription("This function enables user to compile simai chart specified to the format they want. By default is ma2 for simai.");
                HasRequiredOption("p|path=", "The path to file", path => FileLocation = path);             
                HasOption("d|difficulty=", "The number representing the difficuty of chart -- 1-6 for Easy to Re:Master, 7 for Original/Utage",diff => Difficulty = diff);
                HasOption("f|format=", "The target format - simai or ma2", format => TargetFormat = format);
                HasOption("o|output=","Export compiled chart to location specified", dest => Destination = dest);
            }

            /// <summary>
            /// Execute the command
            /// </summary>
            /// <param name="remainingArguments">Rest of the arguments</param>
            /// <returns>Code of execution indicates if the commands is successfully executed</returns>
            /// <exception cref="FileNotFoundException">Raised when the file is not found</exception>
            public override int Run(string[] remainingArguments)
            {
                try
                {
                    SimaiTokenizer tokenizer = new SimaiTokenizer();
                    tokenizer.UpdateFromPath(FileLocation ?? throw new FileNotFoundException());
                    SimaiParser parser = new SimaiParser();
                    string[] tokensCandidates;
                    if (Difficulty != null)
                    {
                        tokensCandidates = tokenizer.ChartCandidates[Difficulty];
                    }
                    else
                    {
                        tokensCandidates = tokenizer.ChartCandidates.Values.First();
                    }
                    Chart candidate = parser.ChartOfToken(tokensCandidates);
                    SimaiCompiler compiler = new SimaiCompiler();
                    string result = "";
                    switch (TargetFormat)
                    {
                        case "simai":
                            Simai resultChart = new Simai(candidate);
                            result = resultChart.Compose();
                            if (Destination != null && !Destination.Equals(""))
                            {
                                StreamWriter sw = new StreamWriter(Destination + Program.GlobalSep + "maidata.txt", false);
                                {
                                    sw.WriteLine(result);
                                }
                                sw.Close();
                                if (File.Exists(Destination + Program.GlobalSep + "maidata.txt"))
                                {
                                    Console.WriteLine("Successfully compiled at: " + Destination + Program.GlobalSep + "result.ma2");
                                }
                                else
                                {
                                    throw new FileNotFoundException("THE FILE IS NOT SUCCESSFULLY COMPILED.");
                                }
                            }
                            else Console.WriteLine(result);
                            break;
                        case null:
                        case "ma2":
                            if (result.Equals(""))
                            {
                                Ma2 defaultChart = new Ma2(candidate);
                                result = defaultChart.Compose();
                            }
                            if (Destination != null && !Destination.Equals(""))
                            {
                                StreamWriter sw = new StreamWriter(Destination + Program.GlobalSep + "result.ma2", false);
                                {
                                    sw.WriteLine(result);
                                }
                                sw.Close();
                                if (File.Exists(Destination + Program.GlobalSep + "result.ma2"))
                                {
                                    Console.WriteLine("Successfully compiled at: " + Destination + Program.GlobalSep + "result.ma2");
                                }
                                else
                                {
                                    throw new FileNotFoundException("THE FILE IS NOT SUCCESSFULLY COMPILED.");
                                }
                            }
                            else Console.WriteLine(result);
                            break;
                    }

                    return Success;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    Console.Error.WriteLine(ex.StackTrace);
                    return Failed;
                }
            }
        }

        /// <summary>
        /// Compile Ma2 Command
        /// </summary>
        public class CompileMa2 : ConsoleCommand
        {
            /// <summary>
            /// Retrurn when command successfully executed
            /// </summary>
            private const int Success = 0;
            /// <summary>
            /// Retrurn when command failed to execute
            /// </summary>
            private const int Failed = 2;

            /// <summary>
            /// Source file path
            /// </summary>
            public string? FileLocation { get; set; }
            /// <summary>
            /// Difficulty
            /// </summary>
            public string? Difficulty { get; set; }
            /// <summary>
            /// ID
            /// </summary>
            public string? ID { get; set; }
            /// <summary>
            /// Destination of output
            /// </summary>
            public string? Destination { get; set; }
            /// <summary>
            /// Target Format of the file
            /// </summary>
            public string? TargetFormat { get; set; }

            /// <summary>
            /// Construct Command
            /// </summary>
            public CompileMa2()
            {
                IsCommand("CompileMa2", "Compile assigned Ma2 chart to assigned format");
                HasLongDescription("This function enables user to compile ma2 chart specified to the format they want. By default is simai for ma2.");
                HasRequiredOption("p|path=", "REQUIRED: The path to file", path => FileLocation = path);
                HasOption("f|format=", "The target format - simai or ma2", format => TargetFormat = format);
                HasOption("o|output=", "Export compiled chart to location specified", dest => Destination = dest);
            }

            /// <summary>
            /// Execute the command
            /// </summary>
            /// <param name="remainingArguments">Rest of the arguments</param>
            /// <returns>Code of execution indicates if the commands is successfully executed</returns>
            /// <exception cref="FileNotFoundException">Raised when the file is not found</exception>
            public override int Run(string[] remainingArguments)
            {
                try
                {
                    Ma2Tokenizer tokenizer = new Ma2Tokenizer();
                    Ma2Parser parser = new Ma2Parser();
                    Chart candidate = parser.ChartOfToken(tokenizer.Tokens(FileLocation ?? throw new FileNotFoundException()));
                    string result = "";
                    switch (TargetFormat)
                    {
                        case null:
                        case "simai":
                            Simai resultChart = new Simai(candidate);
                            result = resultChart.Compose();
                            if (Destination != null && !Destination.Equals(""))
                            {
                                StreamWriter sw = new StreamWriter(Destination + Program.GlobalSep + "maidata.txt", false);
                                {
                                    sw.WriteLine(result);
                                }
                                sw.Close();
                                if (File.Exists(Destination + Program.GlobalSep + "maidata.txt"))
                                {
                                    Console.WriteLine("Successfully compiled at: " + Destination + Program.GlobalSep + "result.ma2");
                                }
                                else
                                {
                                    throw new FileNotFoundException("THE FILE IS NOT SUCCESSFULLY COMPILED.");
                                }
                            }
                            else Console.WriteLine(result);
                            break;
                        case "ma2":
                            if (result.Equals(""))
                            {
                                Ma2 defaultChart = new Ma2(candidate);
                                result = defaultChart.Compose();
                            }
                            if (Destination != null && !Destination.Equals(""))
                            {
                                StreamWriter sw = new StreamWriter(Destination + Program.GlobalSep + "result.ma2", false);
                                {
                                    sw.WriteLine(result);
                                }
                                sw.Close();
                                if (File.Exists(Destination + Program.GlobalSep + "result.ma2"))
                                {
                                    Console.WriteLine("Successfully compiled at: " + Destination + Program.GlobalSep + "result.ma2");
                                }
                                else
                                {
                                    throw new FileNotFoundException("THE FILE IS NOT SUCCESSFULLY COMPILED.");
                                }
                            }
                            else Console.WriteLine(result);
                            break;
                    }

                    return Success;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    Console.Error.WriteLine(ex.StackTrace);
                    return Failed;
                }
            }
        }

        /// <summary>
        /// Compile Ma2 Command
        /// </summary>
        public class CompileMa2ID : ConsoleCommand
        {
            /// <summary>
            /// Retrurn when command successfully executed
            /// </summary>
            private const int Success = 0;
            /// <summary>
            /// Retrurn when command failed to execute
            /// </summary>
            private const int Failed = 2;

            /// <summary>
            /// Source file path
            /// </summary>
            public string? FileLocation { get; set; }
            /// <summary>
            /// Difficulty
            /// </summary>
            public string? Difficulty { get; set; }
            /// <summary>
            /// ID
            /// </summary>
            public string? ID { get; set; }
            /// <summary>
            /// Destination of output
            /// </summary>
            public string? Destination { get; set; }
            /// <summary>
            /// Target Format of the file
            /// </summary>
            public string? TargetFormat { get; set; }

            /// <summary>
            /// Construct Command
            /// </summary>
            public CompileMa2ID()
            {
                IsCommand("CompileMa2ID", "Compile assigned Ma2 chart to assigned format");
                HasLongDescription("This function enables user to compile ma2 chart specified to the format they want. By default is simai for ma2.");
                HasRequiredOption("d|difficulty=", "REQUIRED: The number representing the difficuty of chart -- 0-4 for Basic to Re:Master", diff => Difficulty = diff);
                HasRequiredOption("i|id=", "REQUIRED: The id of the ma2", id => ID = id);
                HasRequiredOption("p|path=", "REQUIRED: Folder of A000 to override - end with a path separator", path => FileLocation = path);
                //FileLocation = GlobalPaths[0];
                //HasOption("a|a000=", "Folder of A000 to override - end with a path separator", path => FileLocation = path);
                HasOption("f|format=", "The target format - simai or ma2", format => TargetFormat = format);
                HasOption("o|output=", "Export compiled chart to location specified", dest => Destination = dest);
            }

            /// <summary>
            /// Execute the command
            /// </summary>
            /// <param name="remainingArguments">Rest of the arguments</param>
            /// <returns>Code of execution indicates if the commands is successfully executed</returns>
            /// <exception cref="FileNotFoundException">Raised when the file is not found</exception>
            public override int Run(string[] remainingArguments)
            {
                try
                {
                    Ma2Tokenizer tokenizer = new Ma2Tokenizer();
                    Ma2Parser parser = new Ma2Parser();
                    //Chart good = new Ma2(@"/Users/neskol/MaiAnalysis/A000/music/music" + musicID + "/" + musicID + "_0" + difficulty + ".ma2");
                    string tokenLocation = FileLocation ?? throw new FileNotFoundException();
                    Chart candidate = parser.ChartOfToken(tokenizer.Tokens(tokenLocation+"music"+GlobalSep+"music"+CompensateZero(ID??throw new NullReferenceException("ID shall not be null"))+GlobalSep+ CompensateZero(ID ?? throw new NullReferenceException("ID shall not be null")) + "_0"+Difficulty+".ma2"));
                    string result = "";
                    switch (TargetFormat)
                    {
                        case null:
                        case "simai":
                            Simai resultChart = new Simai(candidate);
                            result = resultChart.Compose();
                            if (Destination != null && !Destination.Equals(""))
                            {
                                StreamWriter sw = new StreamWriter(Destination + Program.GlobalSep + "maidata.txt", false);
                                {
                                    sw.WriteLine(result);
                                }
                                sw.Close();
                                if (File.Exists(Destination + Program.GlobalSep + "maidata.txt"))
                                {
                                    Console.WriteLine("Successfully compiled at: " + Destination + Program.GlobalSep + "result.ma2");
                                }
                                else
                                {
                                    throw new FileNotFoundException("THE FILE IS NOT SUCCESSFULLY COMPILED.");
                                }
                            }
                            else Console.WriteLine(result);
                            break;
                        case "ma2":
                            if (result.Equals(""))
                            {
                                Ma2 defaultChart = new Ma2(candidate);
                                result = defaultChart.Compose();
                            }
                            if (Destination != null && !Destination.Equals(""))
                            {
                                StreamWriter sw = new StreamWriter(Destination + Program.GlobalSep + "result.ma2", false);
                                {
                                    sw.WriteLine(result);
                                }
                                sw.Close();
                                if (File.Exists(Destination + Program.GlobalSep + "result.ma2"))
                                {
                                    Console.WriteLine("Successfully compiled at: "+ Destination + Program.GlobalSep + "result.ma2");
                                }
                                else
                                {
                                    throw new FileNotFoundException("THE FILE IS NOT SUCCESSFULLY COMPILED.");
                                }
                            }
                            else Console.WriteLine(result);
                            break;
                    }

                    return Success;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    Console.Error.WriteLine(ex.StackTrace);
                    return Failed;
                }
            }
        }

        /// <summary>
        /// Compile Ma2 Command
        /// </summary>
        public class CompileDatabase : ConsoleCommand
        {
            /// <summary>
            /// Retrurn when command successfully executed
            /// </summary>
            private const int Success = 0;
            /// <summary>
            /// Retrurn when command failed to execute
            /// </summary>
            private const int Failed = 2;

            /// <summary>
            /// Source file path
            /// </summary>
            public string? A000Location { get; set; }
            /// <summary>
            /// Image file path
            /// </summary>
            public string? ImageLocation { get; set; }
            /// <summary>
            /// Music file path
            /// </summary>
            public string? BGMLocation { get; set; }
            /// <summary>
            /// Video file path
            /// </summary>
            public string? VideoLocation { get; set; }
            /// <summary>
            /// Difficulty
            /// </summary>
            public string? Difficulty { get; set; }

            /// <summary>
            /// Categorize Index
            /// </summary>
            private int categorizeIndex = 0;
            /// <summary>
            /// Categorize Index outer shell
            /// </summary>
            public int? CategorizeIndex 
            { get 
                { return categorizeIndex; } 
                set 
                {
                    if (value!=null) categorizeIndex = (int)value; 
                    else categorizeIndex = 0;
                } 
            }
            /// <summary>
            /// Destination of output
            /// </summary>
            public string? Destination { get; set; }
            /// <summary>
            /// Target Format of the file
            /// </summary>
            public string? TargetFormat { get; set; }

            /// <summary>
            /// Stores categorize method for easier access
            /// </summary>
            public string CategorizeMethods { get; set; }

            /// <summary>
            /// Construct Command
            /// </summary>
            public CompileDatabase()
            {
                CategorizeMethods = "";
                for (int i = 0; i < TrackCategorizeMethodSet.Length; i++)
                {
                    CategorizeMethods += "[" + i + "] " + TrackCategorizeMethodSet[i] + "\n";
                }
                IsCommand("CompileDatabase", "Compile whole ma2 database to format assigned");
                HasLongDescription("This function enables user to compile whole database to the format they want. By default is simai for ma2.");
                HasRequiredOption("p|path=", "REQUIRED: Folder of A000 to override - end with a path separator", aPath => A000Location = aPath);
                HasRequiredOption("o|output=", "REQUIRED: Export compiled chart to location specified", dest => Destination = dest);
                HasOption("m|music=", "Folder of Music files to override - end with a path separator", mPath => BGMLocation = mPath);
                HasOption("c|cover=", "Folder of Cover Image to override - end with a path separator", iPath => ImageLocation = iPath);                              
                //FileLocation = GlobalPaths[0];
                //HasOption("a|a000=", "Folder of A000 to override - end with a path separator", path => FileLocation = path);
                HasOption("f|format=", "The target format - simai or ma2", format => TargetFormat = format);
                HasOption("g|genre=", "The preferred categorizing scheme, includes:\n"+CategorizeMethods, genre => Int32.TryParse(genre,out categorizeIndex));
                HasOption("v|video=", "Folder of Video to override - end with a path separator", vPath => VideoLocation = vPath);
            }

            /// <summary>
            /// Execute the command
            /// </summary>
            /// <param name="remainingArguments">Rest of the arguments</param>
            /// <returns>Code of execution indicates if the commands is successfully executed</returns>
            /// <exception cref="FileNotFoundException">Raised when the file is not found</exception>
            public override int Run(string[] remainingArguments)
            {
                try
                {
                    string sep = Program.GlobalSep;
                    bool exportBGA = true;
                    bool exportImage = true;
                    bool exportAudio = true;
                    string a000Location = A000Location ?? throw new FileNotFoundException("A000 location was not specified");
                    if (a000Location == null || a000Location.Equals(""))
                    {
                        a000Location = GlobalPaths[0];
                    }

                    string musicLocation = a000Location + @"music" + sep;
                    string? audioLocation = BGMLocation;
                    if (BGMLocation == null)
                    {
                        exportAudio = false;
                    }
                    else
                    {
                        audioLocation = GlobalPaths[1];
                    }

                    string? imageLocation = ImageLocation;
                    if (ImageLocation == null)
                    {
                        exportImage = false;
                    }
                    else
                    {
                        imageLocation = GlobalPaths[2];
                    }

                    string? bgaLocation = VideoLocation;
                    if (VideoLocation == null)
                    {
                        exportBGA = false;
                    }
                    else
                    {
                        bgaLocation = GlobalPaths[3];
                    }

                    string outputLocation = Destination ?? throw new NullReferenceException("Destination not specified");
                    if (outputLocation.Equals(""))
                    {
                        outputLocation = GlobalPaths[4];
                    }

                    try
                    {
                        if (0 <= categorizeIndex && categorizeIndex < TrackCategorizeMethodSet.Length)
                        {
                            GlobalTrackCategorizeMethod = TrackCategorizeMethodSet[categorizeIndex];
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + " The program will use Genre as default method. Press any key to continue.");
                        GlobalTrackCategorizeMethod = TrackCategorizeMethodSet[0];
                        categorizeIndex = 0;
                        Console.ReadKey();
                    }

                    Dictionary<string, string> bgaMap = new Dictionary<string, string>();
                    if (exportBGA && bgaLocation != null)
                    {
                        string[] bgaFiles = Directory.GetFiles(bgaLocation, "*.mp4");
                        Array.Sort(bgaFiles);

                        foreach (string bgaFile in bgaFiles)
                        {
                            string musicID = bgaFile.Substring(bgaLocation.Length).Substring(0, 6).Substring(2, 4);
                            bgaMap.Add(CompensateZero(musicID), bgaFile);
                            bgaMap.Add("01" + musicID, bgaFile);
                        }
                    }
                    else if (exportBGA) throw new NullReferenceException("BGA LOCATION IS NOT SPECIFIED BUT BGA OPTION IS ENABLED");
                    string[] musicFolders = Directory.GetDirectories(musicLocation);

                    //Create output directory
                    DirectoryInfo output = new DirectoryInfo(outputLocation);

                    NumberTotalTrackCompiled = 0;
                    CompiledTracks = new Dictionary<int, string>();
                    //Iterate music folders
                    foreach (string track in musicFolders)
                    {
                        if (File.Exists(track + sep + "Music.xml"))
                        {
                            TrackInformation trackInfo = new XmlInformation(track + sep + "");
                            Console.WriteLine("There is Music.xml in " + track);
                            string shortID = CompensateZero(trackInfo.TrackID).Substring(2);
                            Console.WriteLine("Name: " + trackInfo.TrackName);
                            Console.WriteLine("ID:" + trackInfo.TrackID);
                            Console.WriteLine("Genre: " + trackInfo.TrackGenre);
                            string[] categorizeScheme = { trackInfo.TrackGenre, trackInfo.TrackSymbolicLevel, trackInfo.TrackVersion, trackInfo.TrackComposer, trackInfo.TrackBPM, trackInfo.StandardDeluxePrefix, "" };
                            string defaultCategorizedPath = outputLocation + categorizeScheme[categorizeIndex];

                            //Cross out if not creating update packs
                            // defaultCategorizedPath += sep + categorizeScheme[0];

                            //Deal with special characters in path
                            string trackNameSubstitute = trackInfo.TrackSortName.Replace("" + sep + "", "of");
                            trackNameSubstitute = trackInfo.TrackSortName.Replace("/", "of");
                            trackNameSubstitute = trackInfo.TrackID + "_" + trackNameSubstitute;
                            if (!Directory.Exists(defaultCategorizedPath))
                            {
                                Directory.CreateDirectory(defaultCategorizedPath);
                                Console.WriteLine("Created folder: " + defaultCategorizedPath);
                            }
                            else
                            {
                                Console.WriteLine("Already exist folder: " + defaultCategorizedPath);
                            }
                            if (!Directory.Exists(defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix))
                            {
                                Directory.CreateDirectory(defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                                Console.WriteLine("Created song folder: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                            }
                            else
                            {
                                Console.WriteLine("Already exist song folder: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                            }
                            SimaiCompiler compiler = new SimaiCompiler(track + sep + "", defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                            Console.WriteLine("Finished compiling maidata " + trackInfo.TrackName + " to: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix + sep + "maidata.txt");

                            if (exportAudio)
                            {
                                string originalMusicLocation = audioLocation ?? throw new NullReferenceException("AUDIO FOLDER NOT SPECIFIED BUT AUDIO LOCATION IS NULL");
                                originalMusicLocation += "music00" + shortID + ".mp3";
                                string newMusicLocation = defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix + sep + "track.mp3";
                                if (!File.Exists(newMusicLocation))
                                {
                                    File.Copy(originalMusicLocation, newMusicLocation);
                                    Console.WriteLine("Exported music to: " + newMusicLocation);
                                }
                                else
                                {
                                    Console.WriteLine("Audio already found in: " + newMusicLocation);
                                }
                                //See if image is existing
                                if (exportAudio && !File.Exists(newMusicLocation))
                                {
                                    Console.WriteLine("Audio exists at " + originalMusicLocation + ": " + File.Exists(originalMusicLocation));
                                    throw new FileNotFoundException("MUSIC NOT FOUND IN:" + newMusicLocation);
                                }
                            }

                            if (exportImage)
                            {
                                string originalImageLocation = imageLocation ?? throw new NullReferenceException("IMAGE FOLDER NOT SPECIFIED BUT AUDIO LOCATION IS NULL");
                                originalImageLocation += "UI_Jacket_00" + shortID + ".png";
                                string newImageLocation = defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix + sep + "bg.png";
                                if (!File.Exists(newImageLocation))
                                {
                                    File.Copy(originalImageLocation, newImageLocation);
                                    Console.WriteLine("Image exported to: " + newImageLocation);
                                }
                                else
                                {
                                    Console.WriteLine("Image already found in: " + newImageLocation);
                                }
                                //Check if Image exists
                                if (exportImage && !File.Exists(newImageLocation))
                                {
                                    Console.WriteLine("Image exists at " + originalImageLocation + ": " + File.Exists(originalImageLocation));
                                    throw new FileNotFoundException("IMAGE NOT FOUND IN: " + newImageLocation);
                                }
                            }
                            // Console.WriteLine("Exported to: " + outputLocation + trackInfo.TrackGenre + sep + trackNameSubstitute + trackInfo.DXChart);

                            string? originalBGALocation = "";
                            bool bgaExists = bgaMap.TryGetValue(CompensateZero(trackInfo.TrackID), out originalBGALocation);
                            if (!bgaExists)
                            {
                                if (trackInfo.TrackID.Length == 5)
                                {
                                    bgaExists = bgaMap.TryGetValue(trackInfo.TrackID.Substring(1, 4), out originalBGALocation);
                                }
                                else if (trackInfo.TrackID.Length == 3)
                                {
                                    bgaExists = bgaMap.TryGetValue(CompensateShortZero(trackInfo.TrackID), out originalBGALocation);
                                }
                            }
                            if (exportBGA && !bgaExists)
                            {
                                Console.WriteLine("BGA NOT FOUND");
                                Console.WriteLine(trackInfo.TrackID);
                                Console.WriteLine(CompensateZero(trackInfo.TrackID));
                                Console.WriteLine(originalBGALocation);
                                Console.ReadKey();
                            }
                            if (exportBGA)
                            {
                                string? newBGALocation = defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix + sep + "pv.mp4";
                                if (bgaExists && !File.Exists(newBGALocation))
                                {
                                    Console.WriteLine("A BGA file was found in " + originalBGALocation);
                                    var originalBGALocationCandidate = originalBGALocation ?? throw new NullReferenceException();
                                    File.Copy(originalBGALocationCandidate, newBGALocation);
                                    Console.WriteLine("Exported BGA file to: " + newBGALocation);
                                }
                                else if (bgaExists && File.Exists(newBGALocation))
                                {
                                    Console.WriteLine("BGA already found in " + newBGALocation);
                                }
                                //Check if BGA exists
                                if (exportBGA && bgaExists && !File.Exists(newBGALocation))
                                {
                                    Console.WriteLine("BGA exists at " + originalBGALocation + ": " + File.Exists(originalBGALocation));
                                    throw new FileNotFoundException("BGA NOT FOUND IN: " + newBGALocation);
                                }
                            }
                            NumberTotalTrackCompiled++;
                            CompiledTracks.Add(int.Parse(trackInfo.TrackID), trackInfo.TrackName);
                            AppendBPM(trackInfo.TrackID, trackInfo.TrackBPM);
                            AppendDebugInformation(trackInfo.TrackID, compiler.SymbolicBPMTable(), compiler.SymbolicFirstNote(false));
                            string[] compiledTrackDetail = { trackInfo.TrackName, trackInfo.TrackGenre, trackInfo.TrackVersion, trackInfo.TrackVersionNumber };
                            CompiledTrackDetailSet.Add(trackInfo.TrackName + trackInfo.TrackID, compiledTrackDetail);
                            Console.WriteLine("Exported to: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("There is no Music.xml in folder " + track);
                        }
                    }
                    Console.WriteLine("Total music compiled: " + NumberTotalTrackCompiled);
                    int index = 1;
                    foreach (KeyValuePair<int, string> pair in CompiledTracks)
                    {
                        Console.WriteLine("[" + index + "]: " + pair.Key + " " + pair.Value);
                        index++;
                    }
                    Log(outputLocation);
                    return Success;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    Console.Error.WriteLine(ex.StackTrace);
                    return Failed;
                }
            }
        }

        /// <summary>
        /// Debug method to test specific song and difficulty
        /// </summary>
        /// <param name="musicID">music ID in 6 digits</param>
        /// <param name="difficulty">0-4 representing Basic-Re:Master</param>
        public static void TestSpecificChart(string musicID, string difficulty)
        {
            Chart good = new Ma2(@"/Users/neskol/MaiAnalysis/A000/music/music" + musicID + "/" + musicID + "_0" + difficulty + ".ma2");
            SimaiCompiler compiler = new SimaiCompiler();
            Console.WriteLine(good.Compose());
            Console.WriteLine(compiler.Compose(good));
            // Console.WriteLine(good.FirstNote.Compose(1));
        }

        /// <summary>
        /// Debug method to test specific chart
        /// </summary>
        /// <param name="path">Path to ma2</param>
        public static void TestSpecificChart(string path)
        {
            Chart good = new Ma2(path);
            SimaiCompiler compiler = new SimaiCompiler();
            Console.WriteLine(good.Compose());
            Console.WriteLine(compiler.Compose(good));
            Console.WriteLine(good.Information);
        }

        /// <summary>
        /// Debug method to test specific chart
        /// </summary>
        public static void TestSpecificChart()
        {
            Console.WriteLine("Give the path to ma2");
            string path = Console.ReadLine() ?? throw new NullReferenceException();
            Chart good = new Ma2(path);
            SimaiCompiler compiler = new SimaiCompiler();
            Console.WriteLine(good.Compose());
            Console.WriteLine(compiler.Compose(good));
            Console.WriteLine(good.Information);
        }

        /// <summary>
        /// Compile all maidata using value provided
        /// </summary>
        public static void CompileChartDatabase()
        {
            string sep = Program.GlobalSep;
            Console.WriteLine("Specify the path separator this script is running on");
            sep = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (sep.Equals(""))
            {
                sep = Program.GlobalSep;
            }

            Console.WriteLine("Specify A000 location: *Be sure to add " + sep + " in the end");
            bool exportBGA = true;
            bool exportImage = true;
            bool exportAudio = true;
            string a000Location = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (a000Location.Equals(""))
            {
                a000Location = GlobalPaths[0];
            }

            string musicLocation = a000Location + @"music" + sep;
            Console.WriteLine("Specify Audio location: *Be sure to add " + sep + " in the end or type n if have not");
            string audioLocation = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (audioLocation.Equals(""))
            {
                audioLocation = GlobalPaths[1];
            }
            else if (audioLocation.Equals("n"))
            {
                exportAudio = false;
            }

            Console.WriteLine("Specify Image location: *Be sure to add " + sep + "in the end or type n if have not");
            string imageLocation = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (imageLocation.Equals(""))
            {
                imageLocation = GlobalPaths[2];
            }
            else if (imageLocation.Equals("n"))
            {
                exportImage = false;
            }

            Console.WriteLine("Specify BGA location: *Be sure to add " + sep + " in the end or type n if have not");
            string bgaLocation = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (bgaLocation.Equals(""))
            {
                bgaLocation = GlobalPaths[3];
            }
            else if (bgaLocation.Equals("n"))
            {
                exportBGA = false;
            }

            Console.WriteLine("Specify Output location: *Be sure to add " + sep + " in the end");
            string outputLocation = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (outputLocation.Equals(""))
            {
                outputLocation = GlobalPaths[4];
            }

            int categorizeIndex = 0;
            Console.WriteLine("Specify the sorting method number the script will be used: ");
            for (int i = 0; i < Program.TrackCategorizeMethodSet.Length; i++)
            {
                Console.WriteLine("[" + i + "]" + " " + TrackCategorizeMethodSet[i]);
            }
            GlobalTrackCategorizeMethod = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            try
            {
                if (0 <= Int32.Parse(GlobalTrackCategorizeMethod) && Int32.Parse(GlobalTrackCategorizeMethod) < TrackCategorizeMethodSet.Length)
                {
                    categorizeIndex = Int32.Parse(GlobalTrackCategorizeMethod);
                    GlobalTrackCategorizeMethod = TrackCategorizeMethodSet[Int32.Parse(GlobalTrackCategorizeMethod)];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " The program will use Genre as default method. Press any key to continue.");
                GlobalTrackCategorizeMethod = TrackCategorizeMethodSet[0];
                categorizeIndex = 0;
                Console.ReadKey();
            }

            Dictionary<string, string> bgaMap = new Dictionary<string, string>();
            if (exportBGA)
            {
                string[] bgaFiles = Directory.GetFiles(bgaLocation, "*.mp4");
                Array.Sort(bgaFiles);

                foreach (string bgaFile in bgaFiles)
                {
                    string musicID = bgaFile.Substring(bgaLocation.Length).Substring(0, 6).Substring(2, 4);
                    bgaMap.Add(CompensateZero(musicID), bgaFile);
                    bgaMap.Add("01" + musicID, bgaFile);
                }
            }
            string[] musicFolders = Directory.GetDirectories(musicLocation);

            //Create output directory
            DirectoryInfo output = new DirectoryInfo(outputLocation);
            // XmlInformation test = new XmlInformation(a000Location+ "music" + sep + "music010706" + sep + "");
            //string shortID = CompensateZero(test.TrackID).Substring(2);
            //Console.WriteLine(shortID);
            //Console.ReadLine();
            //string oldName = imageLocation + "UI_Jacket_00" + shortID + ".png";
            //string newName = @"D:\bg.png";
            //File.Copy(oldName, newName);

            NumberTotalTrackCompiled = 0;
            CompiledTracks = new Dictionary<int, string>();
            //Iterate music folders
            foreach (string track in musicFolders)
            {
                if (File.Exists(track + sep + "Music.xml"))
                {
                    TrackInformation trackInfo = new XmlInformation(track + sep + "");
                    Console.WriteLine("There is Music.xml in " + track);
                    string shortID = CompensateZero(trackInfo.TrackID).Substring(2);
                    Console.WriteLine("Name: " + trackInfo.TrackName);
                    Console.WriteLine("ID:" + trackInfo.TrackID);
                    Console.WriteLine("Genre: " + trackInfo.TrackGenre);
                    string[] categorizeScheme = { trackInfo.TrackGenre, trackInfo.TrackSymbolicLevel, trackInfo.TrackVersion, trackInfo.TrackComposer, trackInfo.TrackBPM, trackInfo.StandardDeluxePrefix, "" };
                    string defaultCategorizedPath = outputLocation + categorizeScheme[categorizeIndex];

                    //Cross out if not creating update packs
                    // defaultCategorizedPath += sep + categorizeScheme[0];

                    //Deal with special characters in path
                    string trackNameSubstitute = trackInfo.TrackSortName.Replace("" + sep + "", "of");
                    trackNameSubstitute = trackInfo.TrackSortName.Replace("/", "of");
                    trackNameSubstitute = trackInfo.TrackID + "_" + trackNameSubstitute;
                    if (!Directory.Exists(defaultCategorizedPath))
                    {
                        Directory.CreateDirectory(defaultCategorizedPath);
                        Console.WriteLine("Created folder: " + defaultCategorizedPath);
                    }
                    else
                    {
                        Console.WriteLine("Already exist folder: " + defaultCategorizedPath);
                    }
                    if (!Directory.Exists(defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix))
                    {
                        Directory.CreateDirectory(defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                        Console.WriteLine("Created song folder: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                    }
                    else
                    {
                        Console.WriteLine("Already exist song folder: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                    }
                    SimaiCompiler compiler = new SimaiCompiler(track + sep + "", defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                    Console.WriteLine("Finished compiling maidata " + trackInfo.TrackName + " to: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix + sep + "maidata.txt");

                    if (exportAudio)
                    {
                        string originalMusicLocation = audioLocation;
                        originalMusicLocation += "music00" + shortID + ".mp3";
                        string newMusicLocation = defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix + sep + "track.mp3";
                        if (!File.Exists(newMusicLocation))
                        {
                            File.Copy(originalMusicLocation, newMusicLocation);
                            Console.WriteLine("Exported music to: " + newMusicLocation);
                        }
                        else
                        {
                            Console.WriteLine("Audio already found in: " + newMusicLocation);
                        }
                        //See if image is existing
                        if (exportAudio && !File.Exists(newMusicLocation))
                        {
                            Console.WriteLine("Audio exists at " + originalMusicLocation + ": " + File.Exists(originalMusicLocation));
                            throw new FileNotFoundException("MUSIC NOT FOUND IN:" + newMusicLocation);
                        }
                    }

                    if (exportImage)
                    {
                        string originalImageLocation = imageLocation;
                        originalImageLocation += "UI_Jacket_00" + shortID + ".png";
                        string newImageLocation = defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix + sep + "bg.png";
                        if (!File.Exists(newImageLocation))
                        {
                            File.Copy(originalImageLocation, newImageLocation);
                            Console.WriteLine("Image exported to: " + newImageLocation);
                        }
                        else
                        {
                            Console.WriteLine("Image already found in: " + newImageLocation);
                        }
                        //Check if Image exists
                        if (exportImage && !File.Exists(newImageLocation))
                        {
                            Console.WriteLine("Image exists at " + originalImageLocation + ": " + File.Exists(originalImageLocation));
                            throw new FileNotFoundException("IMAGE NOT FOUND IN: " + newImageLocation);
                        }
                    }
                    // Console.WriteLine("Exported to: " + outputLocation + trackInfo.TrackGenre + sep + trackNameSubstitute + trackInfo.DXChart);

                    string? originalBGALocation = "";
                    bool bgaExists = bgaMap.TryGetValue(CompensateZero(trackInfo.TrackID), out originalBGALocation);
                    if (!bgaExists)
                    {
                        if (trackInfo.TrackID.Length == 5)
                        {
                            bgaExists = bgaMap.TryGetValue(trackInfo.TrackID.Substring(1, 4), out originalBGALocation);
                        }
                        else if (trackInfo.TrackID.Length == 3)
                        {
                            bgaExists = bgaMap.TryGetValue(CompensateShortZero(trackInfo.TrackID), out originalBGALocation);
                        }
                    }
                    if (exportBGA && !bgaExists)
                    {
                        Console.WriteLine("BGA NOT FOUND");
                        Console.WriteLine(trackInfo.TrackID);
                        Console.WriteLine(CompensateZero(trackInfo.TrackID));
                        Console.WriteLine(originalBGALocation);
                        Console.ReadKey();
                    }
                    if (exportBGA)
                    {
                        string? newBGALocation = defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix + sep + "pv.mp4";
                        if (bgaExists && !File.Exists(newBGALocation))
                        {
                            Console.WriteLine("A BGA file was found in " + originalBGALocation);
                            var originalBGALocationCandidate = originalBGALocation ?? throw new NullReferenceException();
                            File.Copy(originalBGALocationCandidate, newBGALocation);
                            Console.WriteLine("Exported BGA file to: " + newBGALocation);
                        }
                        else if (bgaExists && File.Exists(newBGALocation))
                        {
                            Console.WriteLine("BGA already found in " + newBGALocation);
                        }
                        //Check if BGA exists
                        if (exportBGA && bgaExists && !File.Exists(newBGALocation))
                        {
                            Console.WriteLine("BGA exists at " + originalBGALocation + ": " + File.Exists(originalBGALocation));
                            throw new FileNotFoundException("BGA NOT FOUND IN: " + newBGALocation);
                        }
                    }
                    NumberTotalTrackCompiled++;
                    CompiledTracks.Add(int.Parse(trackInfo.TrackID), trackInfo.TrackName);
                    AppendBPM(trackInfo.TrackID, trackInfo.TrackBPM);
                    AppendDebugInformation(trackInfo.TrackID, compiler.SymbolicBPMTable(),compiler.SymbolicFirstNote(false));
                    string[] compiledTrackDetail = { trackInfo.TrackName, trackInfo.TrackGenre, trackInfo.TrackVersion, trackInfo.TrackVersionNumber };
                    CompiledTrackDetailSet.Add(trackInfo.TrackName + trackInfo.TrackID, compiledTrackDetail);
                    Console.WriteLine("Exported to: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("There is no Music.xml in folder " + track);
                }
            }
            Console.WriteLine("Total music compiled: " + NumberTotalTrackCompiled);
            int index = 1;
            foreach (KeyValuePair<int, string> pair in CompiledTracks)
            {
                Console.WriteLine("[" + index + "]: " + pair.Key + " " + pair.Value);
                index++;
            }
            Log(outputLocation);
        }

        /// <summary>
        /// Compile all maidata using value provided
        /// </summary>
        public static void CompileUtageChartDatabase()
        {
            string sep = Program.GlobalSep;
            Console.WriteLine("Specify the path separator this script is running on");
            sep = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (sep.Equals(""))
            {
                sep = Program.GlobalSep;
            }

            Console.WriteLine("Specify A000 location: *Be sure to add " + sep + " in the end");
            bool exportBGA = true;
            bool exportImage = true;
            bool exportAudio = true;
            string a000Location = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (a000Location.Equals(""))
            {
                a000Location = GlobalPaths[0];
            }

            string musicLocation = a000Location + @"music" + sep;
            Console.WriteLine("Specify Audio location: *Be sure to add " + sep + " in the end or type n if have not");
            string audioLocation = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (audioLocation.Equals(""))
            {
                audioLocation = GlobalPaths[1];
            }
            else if (audioLocation.Equals("n"))
            {
                exportAudio = false;
            }

            Console.WriteLine("Specify Image location: *Be sure to add " + sep + "in the end or type n if have not");
            string imageLocation = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (imageLocation.Equals(""))
            {
                imageLocation = GlobalPaths[2];
            }
            else if (imageLocation.Equals("n"))
            {
                exportImage = false;
            }

            Console.WriteLine("Specify BGA location: *Be sure to add " + sep + " in the end or type n if have not");
            string bgaLocation = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (bgaLocation.Equals(""))
            {
                bgaLocation = GlobalPaths[3];
            }
            else if (bgaLocation.Equals("n"))
            {
                exportBGA = false;
            }

            Console.WriteLine("Specify Output location: *Be sure to add " + sep + " in the end");
            string outputLocation = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (outputLocation.Equals(""))
            {
                outputLocation = @"C:\Users\Neskol\MaiAnalysis\Output_Utage\";
            }

            int categorizeIndex = 0;
            Console.WriteLine("Specify the sorting method number the script will be used: ");
            for (int i = 0; i < Program.TrackCategorizeMethodSet.Length; i++)
            {
                Console.WriteLine("[" + i + "]" + " " + TrackCategorizeMethodSet[i]);
            }
            GlobalTrackCategorizeMethod = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            try
            {
                if (0 <= Int32.Parse(GlobalTrackCategorizeMethod) && Int32.Parse(GlobalTrackCategorizeMethod) < TrackCategorizeMethodSet.Length)
                {
                    categorizeIndex = Int32.Parse(GlobalTrackCategorizeMethod);
                    GlobalTrackCategorizeMethod = TrackCategorizeMethodSet[Int32.Parse(GlobalTrackCategorizeMethod)];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " The program will use Genre as default method. Press any key to continue.");
                GlobalTrackCategorizeMethod = TrackCategorizeMethodSet[0];
                categorizeIndex = 0;
                Console.ReadKey();
            }

            Dictionary<string, string> bgaMap = new Dictionary<string, string>();
            if (exportBGA)
            {
                string[] bgaFiles = Directory.GetFiles(bgaLocation, "*.mp4");
                Array.Sort(bgaFiles);

                foreach (string bgaFile in bgaFiles)
                {
                    string musicID = bgaFile.Substring(bgaLocation.Length).Substring(0, 6).Substring(2, 4);
                    bgaMap.Add(CompensateZero(musicID), bgaFile);
                    bgaMap.Add("01" + musicID, bgaFile);
                }
            }
            string[] musicFolders = Directory.GetDirectories(musicLocation);

            //Create output directory
            DirectoryInfo output = new DirectoryInfo(outputLocation);
            // XmlInformation test = new XmlInformation(a000Location+ "music" + sep + "music010706" + sep + "");
            //string shortID = CompensateZero(test.TrackID).Substring(2);
            //Console.WriteLine(shortID);
            //Console.ReadLine();
            //string oldName = imageLocation + "UI_Jacket_00" + shortID + ".png";
            //string newName = @"D:\bg.png";
            //File.Copy(oldName, newName);

            NumberTotalTrackCompiled = 0;
            CompiledTracks = new Dictionary<int, string>();
            //Iterate music folders
            foreach (string track in musicFolders)
            {
                if (File.Exists(track + sep + "Music.xml"))
                {
                    TrackInformation trackInfo = new XmlInformation(track + sep + "");
                    Console.WriteLine("There is Music.xml in " + track);
                    string shortID = CompensateZero(trackInfo.TrackID).Substring(2);
                    Console.WriteLine("Name: " + trackInfo.TrackName);
                    Console.WriteLine("ID:" + trackInfo.TrackID);
                    Console.WriteLine("Genre: " + trackInfo.TrackGenre);
                    string[] categorizeScheme = { trackInfo.TrackGenre, trackInfo.TrackSymbolicLevel, trackInfo.TrackVersion, trackInfo.TrackComposer, trackInfo.TrackBPM, trackInfo.StandardDeluxePrefix, "" };
                    string defaultCategorizedPath = outputLocation + categorizeScheme[categorizeIndex];

                    //Cross out if not creating update packs
                    // defaultCategorizedPath += sep + categorizeScheme[0];

                    //Deal with special characters in path
                    string trackNameSubstitute = trackInfo.TrackSortName.Replace("" + sep + "", "of");
                    trackNameSubstitute = trackInfo.TrackSortName.Replace("/", "of");
                    trackNameSubstitute = trackInfo.TrackID + "_" + trackNameSubstitute;
                    if (!Directory.Exists(defaultCategorizedPath))
                    {
                        Directory.CreateDirectory(defaultCategorizedPath);
                        Console.WriteLine("Created folder: " + defaultCategorizedPath);
                    }
                    else
                    {
                        Console.WriteLine("Already exist folder: " + defaultCategorizedPath);
                    }
                    if (!Directory.Exists(defaultCategorizedPath + sep + trackNameSubstitute + "_Utage"))
                    {
                        Directory.CreateDirectory(defaultCategorizedPath + sep + trackNameSubstitute + "_Utage");
                        Console.WriteLine("Created song folder: " + defaultCategorizedPath + sep + trackNameSubstitute + "_Utage");
                    }
                    else
                    {
                        Console.WriteLine("Already exist song folder: " + defaultCategorizedPath + sep + trackNameSubstitute + "_Utage");
                    }
                    SimaiCompiler compiler = new SimaiCompiler(track + sep + "", defaultCategorizedPath + sep + trackNameSubstitute + "_Utage", true);
                    Console.WriteLine("Finished compiling maidata " + trackInfo.TrackName + " to: " + defaultCategorizedPath + sep + trackNameSubstitute + "_Utage" + sep + "maidata.txt");

                    if (exportAudio)
                    {
                        string originalMusicLocation = audioLocation;
                        originalMusicLocation += "music00" + shortID + ".mp3";
                        string newMusicLocation = defaultCategorizedPath + sep + trackNameSubstitute + "_Utage" + sep + "track.mp3";
                        if (!File.Exists(newMusicLocation))
                        {
                            File.Copy(originalMusicLocation, newMusicLocation, true);
                            Console.WriteLine("Exported music to: " + newMusicLocation);
                        }
                        else
                        {
                            Console.WriteLine("Audio already found in: " + newMusicLocation);
                        }
                        //See if image is existing
                        if (exportAudio && !File.Exists(newMusicLocation))
                        {
                            Console.WriteLine("Audio exists at " + originalMusicLocation + ": " + File.Exists(originalMusicLocation));
                            throw new FileNotFoundException("MUSIC NOT FOUND IN:" + newMusicLocation);
                        }
                    }

                    if (exportImage)
                    {
                        string originalImageLocation = imageLocation;
                        originalImageLocation += "UI_Jacket_00" + shortID + ".png";
                        string newImageLocation = defaultCategorizedPath + sep + trackNameSubstitute + "_Utage" + sep + "bg.png";
                        if (!File.Exists(newImageLocation))
                        {
                            File.Copy(originalImageLocation, newImageLocation);
                            Console.WriteLine("Image exported to: " + newImageLocation);
                        }
                        else
                        {
                            Console.WriteLine("Image already found in: " + newImageLocation);
                        }
                        //Check if Image exists
                        if (exportImage && !File.Exists(newImageLocation))
                        {
                            Console.WriteLine("Image exists at " + originalImageLocation + ": " + File.Exists(originalImageLocation));
                            throw new FileNotFoundException("IMAGE NOT FOUND IN: " + newImageLocation);
                        }
                    }
                    // Console.WriteLine("Exported to: " + outputLocation + trackInfo.TrackGenre + sep + trackNameSubstitute + trackInfo.DXChart);

                    string? originalBGALocation = "";
                    bool bgaExists = bgaMap.TryGetValue(CompensateZero(trackInfo.TrackID), out originalBGALocation);
                    if (!bgaExists)
                    {
                        if (trackInfo.TrackID.Length == 5)
                        {
                            bgaExists = bgaMap.TryGetValue(trackInfo.TrackID.Substring(1, 4), out originalBGALocation);
                        }
                        else if (trackInfo.TrackID.Length == 3)
                        {
                            bgaExists = bgaMap.TryGetValue(CompensateShortZero(trackInfo.TrackID), out originalBGALocation);
                        }
                    }
                    if (exportBGA && !bgaExists)
                    {
                        Console.WriteLine("BGA NOT FOUND");
                        Console.WriteLine(trackInfo.TrackID);
                        Console.WriteLine(CompensateZero(trackInfo.TrackID));
                        Console.WriteLine(originalBGALocation);
                        Console.ReadKey();
                    }
                    if (exportBGA)
                    {
                        string? newBGALocation = defaultCategorizedPath + sep + trackNameSubstitute + "_Utage" + sep + "pv.mp4";
                        if (bgaExists && !File.Exists(newBGALocation))
                        {
                            Console.WriteLine("A BGA file was found in " + originalBGALocation);
                            var originalBGALocationCandidate = originalBGALocation ?? throw new NullReferenceException();
                            File.Copy(originalBGALocationCandidate, newBGALocation);
                            Console.WriteLine("Exported BGA file to: " + newBGALocation);
                        }
                        else if (bgaExists && File.Exists(newBGALocation))
                        {
                            Console.WriteLine("BGA already found in " + newBGALocation);
                        }
                        //Check if BGA exists
                        if (exportBGA && bgaExists && !File.Exists(newBGALocation))
                        {
                            Console.WriteLine("BGA exists at " + originalBGALocation + ": " + File.Exists(originalBGALocation));
                            throw new FileNotFoundException("BGA NOT FOUND IN: " + newBGALocation);
                        }
                    }
                    NumberTotalTrackCompiled++;
                    CompiledTracks.Add(int.Parse(trackInfo.TrackID), trackInfo.TrackName + trackInfo.TrackID);
                    AppendBPM(trackInfo.TrackID, trackInfo.TrackBPM);
                    string[] compiledTrackDetail = { trackInfo.TrackName, trackInfo.TrackGenre, trackInfo.TrackVersion, trackInfo.TrackVersionNumber };
                    CompiledTrackDetailSet.Add(trackInfo.TrackName + trackInfo.TrackID, compiledTrackDetail);
                    Console.WriteLine("Exported to: " + defaultCategorizedPath + sep + trackNameSubstitute + "_Utage");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("There is no Music.xml in folder " + track);
                }
            }
            Console.WriteLine("Total music compiled: " + NumberTotalTrackCompiled);
            int index = 1;
            foreach (KeyValuePair<int, string> pair in CompiledTracks)
            {
                Console.WriteLine("[" + index + "]: " + pair.Key + "," + pair.Value);
                index++;
            }
            Log(outputLocation);
        }

        /// <summary>
        /// Compile maidata with specified version
        /// </summary>
        public static void CompileAssignedChartDatabase()
        {
            string sep = Program.GlobalSep;
            Console.WriteLine("Specify the path separator this script is running on");
            sep = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (sep.Equals(""))
            {
                sep = Program.GlobalSep;
            }

            Console.WriteLine("Specify A000 location: *Be sure to add " + sep + " in the end");
            bool exportBGA = true;
            bool exportImage = true;
            bool exportAudio = true;
            string a000Location = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (a000Location.Equals(""))
            {
                a000Location = @"/Users/neskol/MaiAnalysis/A000/";
            }

            string musicLocation = a000Location + @"music" + sep;
            Console.WriteLine("Specify Audio location: *Be sure to add " + sep + " in the end or type n if have not");
            string audioLocation = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (audioLocation.Equals(""))
            {
                audioLocation = @"/Users/neskol/MaiAnalysis/Sound/";
            }
            else if (audioLocation.Equals("n"))
            {
                exportAudio = false;
            }

            Console.WriteLine("Specify Image location: *Be sure to add " + sep + "in the end or type n if have not");
            string imageLocation = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (imageLocation.Equals(""))
            {
                imageLocation = @"/Users/neskol/MaiAnalysis/Image/Texture2D/";
            }
            else if (imageLocation.Equals("n"))
            {
                exportImage = false;
            }

            Console.WriteLine("Specify BGA location: *Be sure to add " + sep + " in the end or type n if have not");
            string bgaLocation = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (bgaLocation.Equals(""))
            {
                bgaLocation = @"/Users/neskol/MaiAnalysis/DXBGA_HEVC/";
            }
            else if (bgaLocation.Equals("n"))
            {
                exportBGA = false;
            }

            Console.WriteLine("Specify Output location: *Be sure to add " + sep + " in the end");
            string outputLocation = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (outputLocation.Equals(""))
            {
                outputLocation = @"/Users/neskol/MaiAnalysis/Output/";
            }

            int categorizeIndex = 0;
            Console.WriteLine("Specify the music released version the script will be used: ");
            for (int i = 0; i < TrackInformation.version.Length; i++)
            {
                Console.WriteLine("[" + i + "]" + " " + TrackInformation.version[i]);
            }
            GlobalTrackCategorizeMethod = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            try
            {
                if (0 <= Int32.Parse(GlobalTrackCategorizeMethod) && Int32.Parse(GlobalTrackCategorizeMethod) < TrackInformation.version.Length)
                {
                    categorizeIndex = Int32.Parse(GlobalTrackCategorizeMethod);
                    GlobalTrackCategorizeMethod = TrackInformation.version[Int32.Parse(GlobalTrackCategorizeMethod)];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " The program will use Genre as default method. Press any key to continue.");
                GlobalTrackCategorizeMethod = TrackInformation.version[TrackInformation.version.Length - 1];
                categorizeIndex = TrackInformation.version.Length - 1;
                Console.ReadKey();
            }

            Dictionary<string, string> bgaMap = new Dictionary<string, string>();
            if (exportBGA)
            {
                string[] bgaFiles = Directory.GetFiles(bgaLocation, "*.mp4");
                Array.Sort(bgaFiles);

                foreach (string bgaFile in bgaFiles)
                {
                    string musicID = bgaFile.Substring(bgaLocation.Length).Substring(0, 6).Substring(2, 4);
                    bgaMap.Add(CompensateZero(musicID), bgaFile);
                    bgaMap.Add("01" + musicID, bgaFile);
                }
            }
            string[] musicFolders = Directory.GetDirectories(musicLocation);

            //Create output directory
            DirectoryInfo output = new DirectoryInfo(outputLocation);
            // XmlInformation test = new XmlInformation(a000Location+ "music" + sep + "music010706" + sep + "");
            //string shortID = CompensateZero(test.TrackID).Substring(2);
            //Console.WriteLine(shortID);
            //Console.ReadLine();
            //string oldName = imageLocation + "UI_Jacket_00" + shortID + ".png";
            //string newName = @"D:\bg.png";
            //File.Copy(oldName, newName);

            NumberTotalTrackCompiled = 0;
            CompiledTracks = new Dictionary<int,string>();
            //Iterate music folders
            foreach (string track in musicFolders)
            {
                if (File.Exists(track + sep + "Music.xml"))
                {
                    TrackInformation trackInfo = new XmlInformation(track + sep + "");
                    Console.WriteLine("There is Music.xml in " + track);
                    string shortID = CompensateZero(trackInfo.TrackID).Substring(2);
                    Console.WriteLine("Name: " + trackInfo.TrackName);
                    Console.WriteLine("ID:" + trackInfo.TrackID);
                    Console.WriteLine("Genre: " + trackInfo.TrackGenre);
                    string[] categorizeScheme = { trackInfo.TrackGenre, trackInfo.TrackSymbolicLevel, trackInfo.TrackVersion, trackInfo.TrackComposer, trackInfo.TrackBPM, trackInfo.StandardDeluxePrefix, "" };
                    string defaultCategorizedPath = outputLocation + categorizeScheme[categorizeIndex];

                    //Cross out if not creating update packs
                    // defaultCategorizedPath += sep + categorizeScheme[0];

                    //Deal with special characters in path
                    string trackNameSubstitute = trackInfo.TrackSortName.Replace("" + sep + "", "of");
                    trackNameSubstitute = trackInfo.TrackSortName.Replace("/", "of");
                    trackNameSubstitute = trackInfo.TrackID + "_" + trackNameSubstitute;
                    if (!Directory.Exists(defaultCategorizedPath))
                    {
                        Directory.CreateDirectory(defaultCategorizedPath);
                        Console.WriteLine("Created folder: " + defaultCategorizedPath);
                    }
                    else
                    {
                        Console.WriteLine("Already exist folder: " + defaultCategorizedPath);
                    }
                    if (!Directory.Exists(defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix))
                    {
                        Directory.CreateDirectory(defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                        Console.WriteLine("Created song folder: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                    }
                    else
                    {
                        Console.WriteLine("Already exist song folder: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                    }
                    SimaiCompiler compiler = new SimaiCompiler(track + sep + "", defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                    Console.WriteLine("Finished compiling maidata " + trackInfo.TrackName + " to: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix + sep + "maidata.txt");

                    if (exportAudio)
                    {
                        string originalMusicLocation = audioLocation;
                        originalMusicLocation += "music00" + shortID + ".mp3";
                        string newMusicLocation = defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix + sep + "track.mp3";
                        if (!File.Exists(newMusicLocation))
                        {
                            File.Copy(originalMusicLocation, newMusicLocation);
                            Console.WriteLine("Exported music to: " + newMusicLocation);
                        }
                        else
                        {
                            Console.WriteLine("Audio already found in: " + newMusicLocation);
                        }
                        //See if image is existing
                        if (exportAudio && !File.Exists(newMusicLocation))
                        {
                            Console.WriteLine("Audio exists at " + originalMusicLocation + ": " + File.Exists(originalMusicLocation));
                            throw new FileNotFoundException("MUSIC NOT FOUND IN:" + newMusicLocation);
                        }
                    }

                    if (exportImage)
                    {
                        string originalImageLocation = imageLocation;
                        originalImageLocation += "UI_Jacket_00" + shortID + ".png";
                        string newImageLocation = defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix + sep + "bg.png";
                        if (!File.Exists(newImageLocation))
                        {
                            File.Copy(originalImageLocation, newImageLocation);
                            Console.WriteLine("Image exported to: " + newImageLocation);
                        }
                        else
                        {
                            Console.WriteLine("Image already found in: " + newImageLocation);
                        }
                        //Check if Image exists
                        if (exportImage && !File.Exists(newImageLocation))
                        {
                            Console.WriteLine("Image exists at " + originalImageLocation + ": " + File.Exists(originalImageLocation));
                            throw new FileNotFoundException("IMAGE NOT FOUND IN: " + newImageLocation);
                        }
                    }
                    // Console.WriteLine("Exported to: " + outputLocation + trackInfo.TrackGenre + sep + trackNameSubstitute + trackInfo.DXChart);

                    string? originalBGALocation = "";
                    bool bgaExists = bgaMap.TryGetValue(CompensateZero(trackInfo.TrackID), out originalBGALocation);
                    if (!bgaExists)
                    {
                        if (trackInfo.TrackID.Length == 5)
                        {
                            bgaExists = bgaMap.TryGetValue(trackInfo.TrackID.Substring(1, 4), out originalBGALocation);
                        }
                        else if (trackInfo.TrackID.Length == 3)
                        {
                            bgaExists = bgaMap.TryGetValue(CompensateShortZero(trackInfo.TrackID), out originalBGALocation);
                        }
                    }
                    if (exportBGA && !bgaExists)
                    {
                        Console.WriteLine("BGA NOT FOUND");
                        Console.WriteLine(trackInfo.TrackID);
                        Console.WriteLine(CompensateZero(trackInfo.TrackID));
                        Console.WriteLine(originalBGALocation);
                        Console.ReadKey();
                    }
                    if (exportBGA)
                    {
                        string? newBGALocation = defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix + sep + "pv.mp4";
                        if (bgaExists && !File.Exists(newBGALocation))
                        {
                            Console.WriteLine("A BGA file was found in " + originalBGALocation);
                            var originalBGALocationCandidate = originalBGALocation ?? throw new NullReferenceException();
                            File.Copy(originalBGALocationCandidate, newBGALocation);
                            Console.WriteLine("Exported BGA file to: " + newBGALocation);
                        }
                        else if (bgaExists && File.Exists(newBGALocation))
                        {
                            Console.WriteLine("BGA already found in " + newBGALocation);
                        }
                        //Check if BGA exists
                        if (exportBGA && bgaExists && !File.Exists(newBGALocation))
                        {
                            Console.WriteLine("BGA exists at " + originalBGALocation + ": " + File.Exists(originalBGALocation));
                            throw new FileNotFoundException("BGA NOT FOUND IN: " + newBGALocation);
                        }
                    }
                    NumberTotalTrackCompiled++;
                    CompiledTracks.Add(int.Parse(trackInfo.TrackID),trackInfo.TrackName + trackInfo.TrackID);
                    string[] compiledTrackDetail = { trackInfo.TrackName, trackInfo.TrackGenre, trackInfo.TrackVersion, trackInfo.TrackVersionNumber };
                    CompiledTrackDetailSet.Add(trackInfo.TrackName + trackInfo.TrackID, compiledTrackDetail);
                    Console.WriteLine("Exported to: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("There is no Music.xml in folder " + track);
                }
            }
            Console.WriteLine("Total music compiled: " + NumberTotalTrackCompiled);
            int index = 1;
            foreach (KeyValuePair<int,string> pair in CompiledTracks)
            {
                Console.WriteLine("[" + index + "]: " + pair.Key+", "+pair.Value);
                index++;
            }
            Log(outputLocation);
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
            result += "Rev "+ Assembly.GetExecutingAssembly().GetName().Version + " by Neskol\n";
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
        public static void AppendDebugInformation(string idValue, BPMChanges bpmTable,Note firstNoteValue)
        {
            XmlElement node = DebugInformationTable.CreateElement("Node");
            XmlElement id = DebugInformationTable.CreateElement("ID");
            id.InnerText = idValue;
            XmlElement bpm = DebugInformationTable.CreateElement("BPM");
            XmlElement firstNote = DebugInformationTable.CreateElement("FirstNote");
            XmlElement firstNoteType = DebugInformationTable.CreateElement("Type");
            firstNoteType.InnerText = firstNoteValue.Compose(1);
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