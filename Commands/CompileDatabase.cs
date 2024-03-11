using ManyConsole;
using MaiLib;
using System.Configuration.Assemblies;

namespace MaichartConverter
{
    /// <summary>
    ///     Compile Ma2 Database
    /// </summary>
    public class CompileDatabase : ConsoleCommand
    {
        public const int Success = 0;
        public const int Failed = 0;
        public bool StrictDecimal { get; set; }

        /// <summary>
        ///     Source file path
        /// </summary>
        public string? A000Location { get; set; }

        /// <summary>
        ///     Image file path
        /// </summary>
        public string? ImageLocation { get; set; }

        /// <summary>
        ///     Music file path
        /// </summary>
        public string? BGMLocation { get; set; }

        /// <summary>
        ///     Video file path
        /// </summary>
        public string? VideoLocation { get; set; }

        /// <summary>
        ///     Difficulty
        /// </summary>
        public string? Difficulty { get; set; }

        /// <summary>
        ///     Categorize Index outer shell
        /// </summary>
        public int CategorizeIndex { get; set; }

        /// <summary>
        ///     Destination of output
        /// </summary>
        public string? Destination { get; set; }

        /// <summary>
        ///     Target Format of the file
        /// </summary>
        public string? TargetFormat { get; set; }

        /// <summary>
        ///     Stores categorize method for easier access
        /// </summary>
        public string CategorizeMethods { get; set; }

        /// <summary>
        ///     Rotation option for charts
        /// </summary>
        /// <value>Clockwise90/180, Counterclockwise90/180, UpsideDown, LeftToRight</value>
        public string? Rotate { get; set; }

        /// <summary>
        ///     OverallTick Shift for the chart: if the shift tick exceeds the 0 Bar 0 Tick, any note before 0 bar 0 tick will be
        ///     discarded.
        /// </summary>
        /// <value>Tick, 384 tick = 1 bar</value>
        public int? ShiftTick { get; set; }

        /// <summary>
        ///     Construct Command
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
            HasLongDescription(
                "This function enables user to compile whole database to the format they want. By default is simai for ma2.");
            HasRequiredOption("p|path=", "REQUIRED: Folder of A000 to override - end with a path separator",
                aPath => A000Location = aPath);
            HasRequiredOption("o|output=", "REQUIRED: Export compiled chart to location specified",
                dest => Destination = dest);
            HasOption("m|music=", "Folder of Music files to override - end with a path separator",
                mPath => BGMLocation = mPath);
            HasOption("c|cover=", "Folder of Cover Image to override - end with a path separator",
                iPath => ImageLocation = iPath);
            //FileLocation = GlobalPaths[0];
            //HasOption("a|a000=", "Folder of A000 to override - end with a path separator", path => FileLocation = path);
            HasOption("f|format=", "The target format - Simai, SimaiFes, Ma2_103, Ma2_104",
                format => TargetFormat = format);
            HasOption("g|genre=", "The preferred categorizing scheme, includes:\n" + CategorizeMethods,
                genre => CategorizeIndex = Int32.Parse(genre));
            HasOption("r|rotate=",
                "Rotating method to rotate a chart: Clockwise90/180, Counterclockwise90/180, UpsideDown, LeftToRight",
                rotate => Rotate = rotate);
            HasOption("s|shift=", "Overall shift to the chart in unit of tick", tick => ShiftTick = int.Parse(tick));
            HasOption("v|video=", "Folder of Video to override - end with a path separator",
                vPath => VideoLocation = vPath);
            HasOption("d|decimal:", "Force output chart to have levels rated by decimal", _ => StrictDecimal = true);
        }

        /// <summary>
        ///     Execute the command
        /// </summary>
        /// <param name="remainingArguments">Rest of the arguments</param>
        /// <returns>Code of execution indicates if the commands is successfully executed</returns>
        /// <exception cref="FileNotFoundException">Raised when the file is not found</exception>
        public override int Run(string[] remainingArguments)
        {
            try
            {
                // Console.ReadKey();
                bool exportBGA = true;
                bool exportImage = true;
                bool exportAudio = true;
                string a000Location =
                    A000Location ?? throw new FileNotFoundException("A000 location was not specified");
                // if (remainingArguments.Length == 0)
                // {
                //     Console.WriteLine("Step 1: Provide A000 Location");
                //     a000Location = Console.ReadLine() ?? "";
                // }
                if (a000Location == null || a000Location.Equals(""))
                {
                    a000Location = Program.GlobalPaths[0];
                }

                string musicLocation = $"{a000Location}/music/";
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
                    if (0 <= CategorizeIndex && CategorizeIndex < Program.TrackCategorizeMethodSet.Length)
                    {
                        Program.GlobalTrackCategorizeMethod = Program.TrackCategorizeMethodSet[(int)CategorizeIndex];
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message +
                                      " The program will use Genre as default method. Press any key to continue.");
                    Program.GlobalTrackCategorizeMethod = Program.TrackCategorizeMethodSet[0];
                    CategorizeIndex = 0;
                    Console.ReadKey();
                }

                Dictionary<string, string> bgaMap = [];
                if (exportBGA && bgaLocation != null)
                {
                    string[] bgaFiles = Directory.GetFiles(bgaLocation, "*.mp4");
                    Array.Sort(bgaFiles);

                    foreach (string bgaFile in bgaFiles)
                    {
                        string musicID = bgaFile.Substring(bgaLocation.Length).Substring(0, 6).Substring(2, 4);
                        if (!bgaMap.Keys.Contains(Program.CompensateZero(musicID)))
                            bgaMap.Add(Program.CompensateZero(musicID), bgaFile);
                        bgaMap.Add("01" + musicID, bgaFile);
                        bgaMap.Add("10" + musicID, bgaFile);
                        bgaMap.Add("11" + musicID, bgaFile);
                        bgaMap.Add("12" + musicID, bgaFile);
                    }
                }
                else if (exportBGA)
                    throw new NullReferenceException("BGA LOCATION IS NOT SPECIFIED BUT BGA OPTION IS ENABLED");

                string[] musicFolders = Directory.GetDirectories(musicLocation);

                //Create output directory
                DirectoryInfo output = new DirectoryInfo(outputLocation);

                Program.NumberTotalTrackCompiled = 0;
                Program.CompiledTracks = [];
                //Iterate music folders
                foreach (string track in musicFolders)
                {
                    Console.WriteLine("Iterating on folder {0}", track);
                    if (File.Exists($"{track}/Music.xml"))
                    {
                        TrackInformation trackInfo = new XmlInformation($"{track}/");
                        Console.WriteLine("There is Music.xml in {0}", track);
                        string shortID = Program.CompensateZero(trackInfo.TrackID).Substring(2);
                        Console.WriteLine($"Name: {trackInfo.TrackName}");
                        Console.WriteLine($"ID: {trackInfo.TrackID}");
                        Console.WriteLine($"Genre: {trackInfo.TrackGenre}");
                        string[] categorizeScheme =
                        [
                            trackInfo.TrackGenre, trackInfo.TrackSymbolicLevel, trackInfo.TrackVersion,
                            trackInfo.TrackComposer, trackInfo.TrackBPM, trackInfo.StandardDeluxePrefix, ""
                        ];
                        string defaultCategorizedPath = $"{outputLocation}/{categorizeScheme[CategorizeIndex]}";

                        //Cross out if not creating update packs
                        // defaultCategorizedPath += sep + categorizeScheme[0];

                        //Deal with special characters in path
                        string trackNameSubstitute = $"{trackInfo.TrackID}_{trackInfo.TrackSortName}";
                        if (!Directory.Exists(defaultCategorizedPath))
                        {
                            Directory.CreateDirectory(defaultCategorizedPath);
                            Console.WriteLine("Created folder: {0}", defaultCategorizedPath);
                        }
                        else
                        {
                            Console.WriteLine("Already exist folder: {0}", defaultCategorizedPath);
                        }

                        string trackPath =
                            $"{defaultCategorizedPath}/{trackNameSubstitute}{trackInfo.DXChartTrackPathSuffix}";
                        if (!Directory.Exists(trackPath))
                        {
                            Directory.CreateDirectory(trackPath);
                            Console.WriteLine("Created song folder: {0}", trackPath);
                        }
                        else
                        {
                            Console.WriteLine("Already exist song folder: {0}", trackPath);
                        }

                        SimaiCompiler compiler;
                        if (trackInfo.InformationDict["Utage"] != "")
                        {
                            compiler = new SimaiCompiler(StrictDecimal, $"{track}/",
                                $"{defaultCategorizedPath}/{trackNameSubstitute}_Utage", true);
                            compiler.WriteOut(trackPath, true);
                            Program.CompiledChart.Add(compiler.GenerateOneLineSummary());
                        }
                        else
                        {
                            compiler = new SimaiCompiler(StrictDecimal, $"{track}/", trackPath);
                            compiler.WriteOut(trackPath, true);
                            Program.CompiledChart.Add(compiler.GenerateOneLineSummary());
                        }

                        Console.WriteLine("Finished compiling maidata {0} to: {1}", trackInfo.TrackName,
                            $"{trackPath}/maidata.txt");

                        if (exportAudio)
                        {
                            string originalMusicLocation = $"{audioLocation}/music00{shortID}.mp3" ??
                                                           throw new NullReferenceException(
                                                               "AUDIO FOLDER NOT SPECIFIED BUT AUDIO LOCATION IS NULL");
                            string newMusicLocation = $"{trackPath}/track.mp3";
                            if (!File.Exists(newMusicLocation))
                            {
                                File.Copy(originalMusicLocation, newMusicLocation);
                                Console.WriteLine("Exported music to: {0}", newMusicLocation);
                            }
                            else
                            {
                                Console.WriteLine("Audio already found in: {0}", newMusicLocation);
                            }

                            //See if image is existing
                            if (exportAudio && !File.Exists(newMusicLocation))
                            {
                                Console.WriteLine("Audio exists at " + originalMusicLocation + ": " +
                                                  File.Exists(originalMusicLocation));
                                throw new FileNotFoundException("MUSIC NOT FOUND IN:" + newMusicLocation);
                            }
                        }

                        if (exportImage)
                        {
                            string originalImageLocation = $"{imageLocation}/UI_Jacket_00{shortID}.png" ??
                                                           throw new NullReferenceException(
                                                               "IMAGE FOLDER NOT SPECIFIED BUT AUDIO LOCATION IS NULL");
                            string newImageLocation = $"{trackPath}/bg.png";
                            if (!File.Exists(newImageLocation))
                            {
                                File.Copy(originalImageLocation, newImageLocation);
                                Console.WriteLine("Image exported to: {0}", newImageLocation);
                            }
                            else
                            {
                                Console.WriteLine("Image already found in: {0}", newImageLocation);
                            }

                            //Check if Image exists
                            if (exportImage && !File.Exists(newImageLocation))
                            {
                                Console.WriteLine("Image exists at {0}: {1}", originalImageLocation,
                                    File.Exists(originalImageLocation));
                                throw new FileNotFoundException("IMAGE NOT FOUND IN: " + newImageLocation);
                            }
                        }
                        // Console.WriteLine("Exported to: " + outputLocation + trackInfo.TrackGenre + sep + trackNameSubstitute + trackInfo.DXChart);

                        bool bgaExists = bgaMap.TryGetValue(Program.CompensateZero(trackInfo.TrackID),
                            out string? originalBGALocation);
                        if (!bgaExists)
                        {
                            // Compensate on DX Utage
                            if (trackInfo.TrackID.Length == 6)
                            {
                                bgaExists = bgaMap.TryGetValue(trackInfo.TrackID.Substring(2, 4),
                                    out originalBGALocation);
                            }
                            // Compensate on DX Standard
                            else if (trackInfo.TrackID.Length == 5)
                            {
                                bgaExists = bgaMap.TryGetValue(trackInfo.TrackID.Substring(1, 4),
                                    out originalBGALocation);
                            }
                            // Compensate on Standard
                            else if (trackInfo.TrackID.Length == 3)
                            {
                                bgaExists = bgaMap.TryGetValue(Program.CompensateShortZero(trackInfo.TrackID),
                                    out originalBGALocation);
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
                            string? newBGALocation = $"{trackPath}/pv.mp4";
                            if (bgaExists && !File.Exists(newBGALocation))
                            {
                                Console.WriteLine("A BGA file was found in {0}", originalBGALocation);
                                string originalBGALocationCandidate =
                                    originalBGALocation ?? throw new NullReferenceException();
                                File.Copy(originalBGALocationCandidate, newBGALocation);
                                Console.WriteLine("Exported BGA file to: {0}", newBGALocation);
                            }
                            else if (bgaExists && File.Exists(newBGALocation))
                            {
                                Console.WriteLine("BGA already found in {0}", newBGALocation);
                            }

                            //Check if BGA exists
                            if (exportBGA && bgaExists && !File.Exists(newBGALocation))
                            {
                                Console.WriteLine("BGA exists at {0}: {1}", originalBGALocation,
                                    File.Exists(originalBGALocation));
                                throw new FileNotFoundException("BGA NOT FOUND IN: " + newBGALocation);
                            }
                        }

                        Program.NumberTotalTrackCompiled++;
                        Program.CompiledTracks.Add(int.Parse(trackInfo.TrackID), trackInfo.TrackName);
                        // Program.AppendBPM(trackInfo.TrackID, trackInfo.TrackBPM);
                        // Program.AppendDebugInformation(trackInfo.TrackID, compiler.SymbolicBPMTable(), compiler.SymbolicFirstNote(false));
                        string[] compiledTrackDetail =
                        [
                            trackInfo.TrackName, trackInfo.TrackGenre, trackInfo.TrackVersion,
                            trackInfo.TrackVersionNumber
                        ];
                        Program.CompiledTrackDetailSet.Add(trackInfo.TrackName + trackInfo.TrackID,
                            compiledTrackDetail);
                        // Program.CompiledChart.Add(trackInfo.TrackName + compiler.GenerateOneLineSummary());
                        Console.WriteLine("Exported to: {0}", trackPath);
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("There is no Music.xml in folder {0}", track);
                    }
                }

                Console.WriteLine("Total music compiled: {0}", Program.NumberTotalTrackCompiled);
                int index = 1;
                foreach (KeyValuePair<int, string> pair in Program.CompiledTracks)
                {
                    Console.WriteLine($"[{index}]: {pair.Key} {pair.Value}");
                    index++;
                }

                Program.Log(outputLocation);
                return Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Program cannot proceed because of following error returned: \n{0}", ex.GetType());
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
                Console.ReadKey();
                return Failed;
            }
        }
    }
}
