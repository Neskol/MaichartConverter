using ManyConsole;
using MaiLib;
using System.Configuration.Assemblies;

namespace MaichartConverter
{
    /// <summary>
    /// Compile Ma2 Database
    /// </summary>
    public class CompileDatabase : ConsoleCommand
    {
        public const int Success = 0;
        public const int Failed = 0;
        public bool StrictDecimal { get; set; }

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
        {
            get
            { return categorizeIndex; }
            set
            {
                if (value != null) categorizeIndex = (int)value;
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
        /// Rotation option for charts
        /// </summary>
        /// <value>Clockwise90/180, Counterclockwise90/180, UpsideDown, LeftToRight</value>
        public string? Rotate { get; set; }
        /// <summary>
        /// OverallTick Shift for the chart: if the shift tick exceeds the 0 Bar 0 Tick, any note before 0 bar 0 tick will be discarded.
        /// </summary>
        /// <value>Tick, 384 tick = 1 bar</value>
        public int? ShiftTick { get; set; }

        /// <summary>
        /// Construct Command
        /// </summary>
        public CompileDatabase()
        {
            CategorizeMethods = "";
            for (int i = 0; i < Program.TrackCategorizeMethodSet.Length; i++)
            {
                CategorizeMethods += $"[{i}]{Program.TrackCategorizeMethodSet[i]}\n";
            }

            StrictDecimal = false;
            IsCommand("CompileDatabase", "Compile whole ma2 database to format assigned");
            HasLongDescription("This function enables user to compile whole database to the format they want. By default is simai for ma2.");
            HasRequiredOption("p|path=", "REQUIRED: Folder of A000 to override - end with a path separator", aPath => A000Location = aPath);
            HasRequiredOption("o|output=", "REQUIRED: Export compiled chart to location specified", dest => Destination = dest);
            HasOption("m|music=", "Folder of Music files to override - end with a path separator", mPath => BGMLocation = mPath);
            HasOption("c|cover=", "Folder of Cover Image to override - end with a path separator", iPath => ImageLocation = iPath);
            //FileLocation = GlobalPaths[0];
            //HasOption("a|a000=", "Folder of A000 to override - end with a path separator", path => FileLocation = path);
            HasOption("f|format=", "The target format - Simai, SimaiFes, Ma2_103, Ma2_104", format => TargetFormat = format);
            HasOption("g|genre=", "The preferred categorizing scheme, includes:\n" + CategorizeMethods, genre => Int32.TryParse(genre, out categorizeIndex));
            HasOption("r|rotate=", "Rotating method to rotate a chart: Clockwise90/180, Counterclockwise90/180, UpsideDown, LeftToRight", rotate => Rotate = rotate);
            HasOption("s|shift=", "Overall shift to the chart in unit of tick", tick => ShiftTick = int.Parse(tick));
            HasOption("v|video=", "Folder of Video to override - end with a path separator", vPath => VideoLocation = vPath);
            HasOption("d|decimal:", "Force output chart to have levels rated by decimal", _ => StrictDecimal = true);
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
                // Console.ReadKey();
                string sep = Program.GlobalSep;
                bool exportBGA = true;
                bool exportImage = true;
                bool exportAudio = true;
                string a000Location = A000Location ?? throw new FileNotFoundException("A000 location was not specified");
                // if (remainingArguments.Length == 0)
                // {
                //     Console.WriteLine("Step 1: Provide A000 Location");
                //     a000Location = Console.ReadLine() ?? "";
                // }
                if (a000Location == null || a000Location.Equals(""))
                {
                    a000Location = Program.GlobalPaths[0];
                }

                string musicLocation = a000Location + @"music" + sep;
                string? audioLocation = BGMLocation;
                // if (remainingArguments.Length == 0)
                // {
                //     Console.WriteLine("Step 2: Provide BGM Location");
                //     audioLocation = Console.ReadLine() ?? "";
                // }
                if (BGMLocation == null)
                {
                    exportAudio = false;
                }
                else if (BGMLocation.Equals(""))
                {
                    audioLocation = Program.GlobalPaths[1];
                }

                string? imageLocation = ImageLocation;
                if (ImageLocation == null)
                {
                    exportImage = false;
                }
                else if (ImageLocation.Equals(""))
                {
                    imageLocation = Program.GlobalPaths[2];
                }

                string? bgaLocation = VideoLocation;
                if (VideoLocation == null)
                {
                    exportBGA = false;
                }
                else if (VideoLocation.Equals(""))
                {
                    bgaLocation = Program.GlobalPaths[3];
                }

                string outputLocation = Destination ?? throw new NullReferenceException("Destination not specified");
                if (outputLocation.Equals(""))
                {
                    outputLocation = Program.GlobalPaths[4];
                }

                try
                {
                    if (0 <= categorizeIndex && categorizeIndex < Program.TrackCategorizeMethodSet.Length)
                    {
                        Program.GlobalTrackCategorizeMethod = Program.TrackCategorizeMethodSet[categorizeIndex];
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + " The program will use Genre as default method. Press any key to continue.");
                    Program.GlobalTrackCategorizeMethod = Program.TrackCategorizeMethodSet[0];
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
                        if (!bgaMap.Keys.Contains(Program.CompensateZero(musicID))) bgaMap.Add(Program.CompensateZero(musicID), bgaFile);
                        bgaMap.Add("01" + musicID, bgaFile);
                        bgaMap.Add("10" + musicID, bgaFile);
                        bgaMap.Add("11" + musicID, bgaFile);
                        bgaMap.Add("12" + musicID, bgaFile);
                    }
                }
                else if (exportBGA) throw new NullReferenceException("BGA LOCATION IS NOT SPECIFIED BUT BGA OPTION IS ENABLED");
                string[] musicFolders = Directory.GetDirectories(musicLocation);

                //Create output directory
                DirectoryInfo output = new DirectoryInfo(outputLocation);

                Program.NumberTotalTrackCompiled = 0;
                Program.CompiledTracks = new Dictionary<int, string>();
                //Iterate music folders
                foreach (string track in musicFolders)
                {
                    Console.WriteLine("Iterating on folder {0}", track);
                    if (File.Exists(track + sep + "Music.xml"))
                    {
                        TrackInformation trackInfo = new XmlInformation(track + sep + "");
                        Console.WriteLine("There is Music.xml in " + track);
                        string shortID = Program.CompensateZero(trackInfo.TrackID).Substring(2);
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
                        SimaiCompiler compiler;
                        if (trackInfo.Information["Utage"] != "")
                        {
                            compiler = new SimaiCompiler(StrictDecimal, track + sep + "", defaultCategorizedPath + sep + trackNameSubstitute + "_Utage", true);
                            compiler.WriteOut(defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix, true);
                            Program.CompiledChart.Add(compiler.GenerateOneLineSummary());
                        }
                        else
                        {
                            compiler = new SimaiCompiler(StrictDecimal, track + sep + "", defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                            compiler.WriteOut(defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix, true);
                            Program.CompiledChart.Add(compiler.GenerateOneLineSummary());
                        }
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
                        bool bgaExists = bgaMap.TryGetValue(Program.CompensateZero(trackInfo.TrackID), out originalBGALocation);
                        if (!bgaExists)
                        {
                            if (trackInfo.TrackID.Length == 6)
                            {
                                bgaExists = bgaMap.TryGetValue(trackInfo.TrackID.Substring(2, 4), out originalBGALocation);
                            }
                            else if (trackInfo.TrackID.Length == 5)
                            {
                                bgaExists = bgaMap.TryGetValue(trackInfo.TrackID.Substring(1, 4), out originalBGALocation);
                            }
                            else if (trackInfo.TrackID.Length == 3)
                            {
                                bgaExists = bgaMap.TryGetValue(Program.CompensateShortZero(trackInfo.TrackID), out originalBGALocation);
                            }
                        }
                        if (exportBGA && !bgaExists)
                        {
                            Console.WriteLine("BGA NOT FOUND");
                            Console.WriteLine(trackInfo.TrackID);
                            Console.WriteLine(Program.CompensateZero(trackInfo.TrackID));
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
                        Program.NumberTotalTrackCompiled++;
                        Program.CompiledTracks.Add(int.Parse(trackInfo.TrackID), trackInfo.TrackName);
                        // Program.AppendBPM(trackInfo.TrackID, trackInfo.TrackBPM);
                        // Program.AppendDebugInformation(trackInfo.TrackID, compiler.SymbolicBPMTable(), compiler.SymbolicFirstNote(false));
                        string[] compiledTrackDetail = { trackInfo.TrackName, trackInfo.TrackGenre, trackInfo.TrackVersion, trackInfo.TrackVersionNumber };
                        Program.CompiledTrackDetailSet.Add(trackInfo.TrackName + trackInfo.TrackID, compiledTrackDetail);
                        // Program.CompiledChart.Add(trackInfo.TrackName + compiler.GenerateOneLineSummary());
                        Console.WriteLine("Exported to: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChartTrackPathSuffix);
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("There is no Music.xml in folder " + track);
                    }
                }
                Console.WriteLine("Total music compiled: " + Program.NumberTotalTrackCompiled);
                int index = 1;
                foreach (KeyValuePair<int, string> pair in Program.CompiledTracks)
                {
                    Console.WriteLine("[" + index + "]: " + pair.Key + " " + pair.Value);
                    index++;
                }
                Program.Log(outputLocation);
                return Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Program cannot proceed becasue of following error returned: \n{0}", ex.GetType());
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
                Console.ReadKey();
                return Failed;
            }
        }
    }

}
