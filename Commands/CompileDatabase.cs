using System.IO.Compression;
using MaiLib;
using ManyConsole;

namespace MaichartConverter
{
    /// <summary>
    ///     Compile Ma2 Database
    /// </summary>
    public class CompileDatabase : ConsoleCommand
    {
        public const int Success = 0;
        public const int Failed = 2;
        public bool StrictDecimal { get; set; }
        public bool IgnoreIncompleteAssets { get; set; }
        public bool MusicIDFolderName { get; set; }
        public bool LogTracksInJson { get; set; }
        public bool ExportAsZipFile { get; set; }
        public bool CompileCollectionFiles { get; set; }

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
                CategorizeMethods += $"[{i}]{Program.TrackCategorizeMethodSet[i]}\n";

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
                genre => CategorizeIndex = int.Parse(genre));
            HasOption("r|rotate=",
                "Rotating method to rotate a chart: Clockwise90/180, Counterclockwise90/180, UpsideDown, LeftToRight",
                rotate => Rotate = rotate);
            HasOption("s|shift=", "Overall shift to the chart in unit of tick", tick => ShiftTick = int.Parse(tick));
            HasOption("v|video=", "Folder of Video to override - end with a path separator",
                vPath => VideoLocation = vPath);
            HasOption("d|decimal:", "Force output chart to have levels rated by decimal", _ => StrictDecimal = true);
            HasOption("i|ignore:", "Ignore incomplete assets and proceed converting",
                _ => IgnoreIncompleteAssets = true);
            HasOption("n|number:", "Use musicID as folder name instead of sort name", _ => MusicIDFolderName = true);
            HasOption("j|json:", "Create a log file of compiled tracks in JSON", _ => LogTracksInJson = true);
            HasOption("z|zip:", "Export Tracks as Zip Files", _ => ExportAsZipFile = true);
            HasOption("k|collection", "Compile a soring file like manifest.json for collection indexing",
                _ => CompileCollectionFiles = true);
        }

        /// <summary>
        ///     Execute the command
        /// </summary>
        /// <param name="remainingArguments">Rest of the arguments</param>
        /// <returns>Code of execution indicates if the commands is successfully executed</returns>
        /// <exception cref="FileNotFoundException">Raised when the file is not found</exception>
        public override int Run(string[] remainingArguments)
        {
            // try
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
                if (a000Location is null or "") a000Location = Program.DefaultPaths[0];

                string musicLocation = $"{a000Location}/music/";
                string? audioLocation = BGMLocation;
                // if (remainingArguments.Length == 0)
                // {
                //     Console.WriteLine("Step 2: Provide BGM Location");
                //     audioLocation = Console.ReadLine() ?? "";
                // }
                if (BGMLocation == null)
                    exportAudio = false;
                else if (BGMLocation.Equals("")) audioLocation = Program.DefaultPaths[1];

                string? imageLocation = ImageLocation;
                if (ImageLocation == null)
                    exportImage = false;
                else if (ImageLocation.Equals("")) imageLocation = Program.DefaultPaths[2];

                string? bgaLocation = VideoLocation;
                if (VideoLocation == null)
                    exportBGA = false;
                else if (VideoLocation.Equals("")) bgaLocation = Program.DefaultPaths[3];

                string outputLocation = Destination ?? throw new NullReferenceException("Destination not specified");
                if (outputLocation.Equals("")) outputLocation = Program.DefaultPaths[4];

                try
                {
                    if (0 <= CategorizeIndex && CategorizeIndex < Program.TrackCategorizeMethodSet.Length)
                        Program.GlobalTrackCategorizeMethod = Program.TrackCategorizeMethodSet[CategorizeIndex];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message +
                                      " The program will use Genre as default method. Press any key to continue.");
                    Program.GlobalTrackCategorizeMethod = Program.TrackCategorizeMethodSet[0];
                    CategorizeIndex = 0;
                    Console.ReadKey();
                }

                string[] musicFolders = Directory.GetDirectories(musicLocation);

                //Create output directory
                Program.NumberTotalTrackCompiled = 0;
                Program.CompiledTracks = [];
                //Iterate music folders
                foreach (string track in musicFolders)
                {
                    Console.WriteLine("Iterating on folder {0}", track);
                    // Check the file status
                    string[] files = Directory.GetFiles(track, "*.ma2");
                    if (files.Length == 0)
                    {
                        Console.WriteLine("This folder does not contain any charts, skipping {0}: ", track);
                    }
                    else if (File.Exists($"{track}/Music.xml"))
                    {
                        TrackInformation trackInfo = new XmlInformation($"{track}/");
                        Program.CompiledTrackInformationList.Add(trackInfo);
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
                        string trackNameSubstitute = MusicIDFolderName
                            ? trackInfo.TrackID
                            : $"{trackInfo.TrackID}_{trackInfo.TrackSortName}";
                        if (!Directory.Exists(defaultCategorizedPath))
                        {
                            Directory.CreateDirectory(defaultCategorizedPath);
                            Console.WriteLine("Created folder: {0}", defaultCategorizedPath);
                        }
                        else
                        {
                            Console.WriteLine("Already exist folder: {0}", defaultCategorizedPath);
                        }

                        // Check if assets are complete
                        string trackPath = MusicIDFolderName
                            ? $"{defaultCategorizedPath}/{trackNameSubstitute}"
                            : $"{defaultCategorizedPath}/{trackNameSubstitute}{trackInfo.DXChartTrackPathSuffix}";
                        string baseMusicPath = $"{audioLocation}/music00{shortID}";
                        string[] audioExtensions = { ".ogg", ".mp3" };
                        string? originalMusicLocation = FindExistingAsset(baseMusicPath, audioExtensions);
                        string baseImagePath = $"{imageLocation}/UI_Jacket_00{shortID}";
                        string[] imageExtensions = { ".png", ".jpg" };
                        string? originalImageLocation = FindExistingAsset(baseImagePath, imageExtensions);
                        string baseBGAPath = $"{bgaLocation}/00{shortID}";
                        string[] videoExtensions = { ".mp4" };
                        string? originalBGALocation = FindExistingAsset(baseBGAPath, videoExtensions);
                        bool trackAssetIncomplete = false;

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
                        }
                        else
                        {
                            compiler = new SimaiCompiler(StrictDecimal, $"{track}/", trackPath);
                            compiler.WriteOut(trackPath, true);
                            Program.CompiledChart.Add(compiler.GenerateOneLineSummary());
                        }

                        Console.WriteLine("Finished compiling maidata {0} to: {1}", trackInfo.TrackName,
                            $"{trackPath}/maidata.txt");

                        ProcessAssetExport(trackInfo, exportAudio, originalMusicLocation, baseMusicPath, trackPath, "track", "Music", ref trackAssetIncomplete);
                        ProcessAssetExport(trackInfo, exportImage, originalImageLocation, baseImagePath, trackPath, "bg", "Image", ref trackAssetIncomplete);
                        ProcessAssetExport(trackInfo, exportBGA, originalBGALocation, baseBGAPath, trackPath, "pv", "BGA", ref trackAssetIncomplete);

                        // Console.WriteLine("Exported to: " + outputLocation + trackInfo.TrackGenre + sep + trackNameSubstitute + trackInfo.DXChart);

                        if (trackAssetIncomplete && !IgnoreIncompleteAssets)
                        {
                            if (Directory.Exists(trackPath))
                            {
                                Directory.Move(trackPath, $"{trackPath}_Incomplete");
                                Console.WriteLine("Due to incomplete asset, this track is marked as incomplete");
                            }
                            else
                            {
                                Console.WriteLine("This track is skipped");
                            }
                        }
                        else
                        {
                            Program.CompiledChart.Add(compiler.GenerateOneLineSummary());
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
                            if (ExportAsZipFile)
                            {
                                Console.WriteLine("Zip file compressing to: {0}.zip", trackPath);
                                ZipFile.CreateFromDirectory(trackPath, $"{trackPath}.zip");
                                Console.WriteLine("Compressed: {0}.zip, removing original folder", trackPath);
                                Directory.Delete(trackPath, true);
                            }
                        }

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
                if (LogTracksInJson) Program.LogTracksInJson(outputLocation);
                if (CompileCollectionFiles) Program.CompileSortingCollection($"{outputLocation}/collections");
                return Success;
            }
            // catch (Exception ex)
            // {
            //     Console.WriteLine("Program cannot proceed because of following error returned: \n{0}", ex.GetType());
            //     Console.Error.WriteLine(ex.Message);
            //     Console.Error.WriteLine(ex.StackTrace);
            //     Console.ReadKey();
            //     // throw ex; // For debug use
            //     return Failed;
            // }
        }
        /// <summary>
        /// Finds the asset that exists from a base path and a list of extensions.
        /// </summary>
        /// <param name="basePath">The full path to the asset without an extension.</param>
        /// <param name="extensions">An array of extensions to try (e.g., [".png", ".jpg"]).</param>
        /// <returns>The full path of the first matching asset, or null if no asset is found.</returns>
        private string? FindExistingAsset(string basePath, string[] extensions)
        {
            foreach (var ext in extensions)
            {
                string fullPath = basePath + ext;
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }
            return null;
        }
        /// <summary>
        /// Processes the export of a single asset (like audio or image).
        /// </summary>
        private void ProcessAssetExport(TrackInformation trackInfo, bool shouldExport, string? originalPath, string basePathForLog,
            string destinationDirectory, string destinationFileName, string assetTypeForLog, ref bool trackAssetIncomplete)
        {
            if (!shouldExport) return;

            if (string.IsNullOrEmpty(originalPath))
            {
                Console.WriteLine($"{assetTypeForLog.ToUpper()} FILE NOT FOUND AT: {basePathForLog}[.*]");
                Program.ErrorMessage.Add($"{assetTypeForLog} not found: {trackInfo.TrackName} with ID {trackInfo.TrackID}");
                trackAssetIncomplete = true;
                if (!IgnoreIncompleteAssets) Console.ReadLine();
            }
            else
            {
                string extension = Path.GetExtension(originalPath);
                string newLocation = $"{destinationDirectory}/{destinationFileName}{extension}";

                if (!File.Exists(newLocation))
                {
                    File.Copy(originalPath, newLocation);
                    Console.WriteLine($"{assetTypeForLog} exported to: {newLocation}");
                }
                else
                {
                    Console.WriteLine($"{assetTypeForLog} already found in: {newLocation}");
                }

                //See if asset exists
                if (!IgnoreIncompleteAssets && !File.Exists(newLocation))
                {
                    Console.WriteLine($"{assetTypeForLog} exists at " + originalPath + ": " +
                                        File.Exists(originalPath));
                    throw new FileNotFoundException($"{assetTypeForLog.ToUpper()} NOT FOUND IN: {newLocation}");
                }
            }
        }
    }
}
