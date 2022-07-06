using System.Xml;
using Microsoft.Win32.SafeHandles;

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


        public readonly static string[] WinPaths = { @"C:\Users\Neskol\MaiAnalysis\A000\",
        @"C:\Users\Neskol\MaiAnalysis\Sound\",
        @"C:\Users\Neskol\MaiAnalysis\Image\Texture2D\",
        @"C:\Users\Neskol\MaiAnalysis\DXBGA_HEVC\",
        @"C:\Users\Neskol\MaiAnalysis\Output\"};

        public readonly static string[] MacPaths = { @"/Users/neskol/MaiAnalysis/A000/",
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
        public static List<string> CompiledTracks = new();
        public static List<string> CompiledChart = new();
        public static Dictionary<string, string[]> CompiledTrackDetailSet = new Dictionary<string, string[]>();

        public static XmlDocument BPMCollection = new XmlDocument();
        public static XmlDocument BPMChangeTable = new XmlDocument();

        /// <summary>
        /// Main method to process charts
        /// </summary>
        /// <param name="args">Parameters to take in</param>     
        public static void Main(string[] args)
        {
            Console.WriteLine(ComposeHeader());

            XmlDeclaration xmlDecl = BPMCollection.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlDeclaration xmlDeclBPM = BPMChangeTable.CreateXmlDeclaration("1.0", "UTF-8", null);
            BPMCollection.AppendChild(xmlDecl);
            BPMChangeTable.AppendChild(xmlDeclBPM);
            XmlElement root = BPMCollection.CreateElement("BPM");
            XmlElement btRoot = BPMChangeTable.CreateElement("BPM-Table");
            BPMCollection.AppendChild(root);
            BPMChangeTable.AppendChild(btRoot);

            // CompileUtageChartDatabase();
            // TestSpecificChart();
            // TestSpecificChart(@"D:\PandoraCandidate.ma2");
            TestSpecificChart("000799", "4");
            // CompileChartDatabase();
            // CompileAssignedChartDatabase();
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
            Console.WriteLine(good.FirstNote.Compose(1));

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

            string musiclocation = a000Location + @"music" + sep;
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
            string[] musicFolders = Directory.GetDirectories(musiclocation);

            //Create output directory
            DirectoryInfo output = new DirectoryInfo(outputLocation);
            // XmlInformation test = new XmlInformation(a000Location+ "music" + sep + "music010706" + sep + "");
            //string shortID = ComponsateZero(test.TrackID).Substring(2);
            //Console.WriteLine(shortID);
            //Console.ReadLine();
            //string oldName = imageLocation + "UI_Jacket_00" + shortID + ".png";
            //string newName = @"D:\bg.png";
            //File.Copy(oldName, newName);

            NumberTotalTrackCompiled = 0;
            CompiledTracks = new List<string>();
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
                            bgaExists = bgaMap.TryGetValue(ComponsateShortZero(trackInfo.TrackID), out originalBGALocation);
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
                    CompiledTracks.Add(trackInfo.TrackName + trackInfo.TrackID);
                    AppendKeyValue(trackInfo.TrackID, trackInfo.TrackBPM);
                    AppendBPMTable(trackInfo.TrackID, trackInfo.TrackBPM, compiler.SymbolicBPMTable());
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
            foreach (string title in CompiledTracks)
            {
                Console.WriteLine("[" + index + "]: " + title);
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
            Console.WriteLine("Specify the path seperator this script is running on");
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

            string musiclocation = a000Location + @"music" + sep;
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
            string[] musicFolders = Directory.GetDirectories(musiclocation);

            //Create output directory
            DirectoryInfo output = new DirectoryInfo(outputLocation);
            // XmlInformation test = new XmlInformation(a000Location+ "music" + sep + "music010706" + sep + "");
            //string shortID = ComponsateZero(test.TrackID).Substring(2);
            //Console.WriteLine(shortID);
            //Console.ReadLine();
            //string oldName = imageLocation + "UI_Jacket_00" + shortID + ".png";
            //string newName = @"D:\bg.png";
            //File.Copy(oldName, newName);

            NumberTotalTrackCompiled = 0;
            CompiledTracks = new List<string>();
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
                            bgaExists = bgaMap.TryGetValue(ComponsateShortZero(trackInfo.TrackID), out originalBGALocation);
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
                    CompiledTracks.Add(trackInfo.TrackName + trackInfo.TrackID);
                    AppendKeyValue(trackInfo.TrackID, trackInfo.TrackBPM);
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
            foreach (string title in CompiledTracks)
            {
                Console.WriteLine("[" + index + "]: " + title);
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
            Console.WriteLine("Specify the path seperator this script is running on");
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

            string musiclocation = a000Location + @"music" + sep;
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
            string[] musicFolders = Directory.GetDirectories(musiclocation);

            //Create output directory
            DirectoryInfo output = new DirectoryInfo(outputLocation);
            // XmlInformation test = new XmlInformation(a000Location+ "music" + sep + "music010706" + sep + "");
            //string shortID = ComponsateZero(test.TrackID).Substring(2);
            //Console.WriteLine(shortID);
            //Console.ReadLine();
            //string oldName = imageLocation + "UI_Jacket_00" + shortID + ".png";
            //string newName = @"D:\bg.png";
            //File.Copy(oldName, newName);

            NumberTotalTrackCompiled = 0;
            CompiledTracks = new List<string>();
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
                            bgaExists = bgaMap.TryGetValue(ComponsateShortZero(trackInfo.TrackID), out originalBGALocation);
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
                    CompiledTracks.Add(trackInfo.TrackName + trackInfo.TrackID);
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
            foreach (string title in CompiledTracks)
            {
                Console.WriteLine("[" + index + "]: " + title);
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
        /// Componsate 0 for short music IDs
        /// </summary>
        /// <param name="intake">Music ID</param>
        /// <returns>0..+#Music ID and |Music ID|==4</returns>
        public static string ComponsateShortZero(string intake)
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
        /// <returns>Maiconverter.fancy</returns>
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
            result += "Rev 1.03 by Neskol\n";
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
            sw.WriteLine("Total music compiled: " + NumberTotalTrackCompiled);
            int index = 1;
            sw.WriteLine("Index\tTitle\tGenre\tVersion\tPatch Number");
            foreach (string title in CompiledTracks)
            {
                string[]? compiledDetailArray = new string[0];
                CompiledTrackDetailSet.TryGetValue(title, out compiledDetailArray);
                if (compiledDetailArray == null)
                {
                    compiledDetailArray = new string[0];
                }
                sw.WriteLine("[" + index + "]\t" + compiledDetailArray[0] + "\t" + compiledDetailArray[1] + "\t" + compiledDetailArray[2] + "\t" + compiledDetailArray[3] ?? throw new NullReferenceException());
                index++;
            }
            index = 1;

            sw.WriteLine("Total chart compiled: " + CompiledChart.Count);
            foreach (string title in CompiledChart)
            {
                sw.WriteLine("[" + index + "]\t" + title);
                index++;
            }
            sw.Close();
            BPMCollection.Save(outputLocation + "bpm.xml");
            BPMChangeTable.Save(outputLocation + "bpm_table.xml");
        }

        /// <summary>
        /// Append Nodes to BPMCollection
        /// </summary>
        /// <param name="key">ID</param>
        /// <param name="value">BPM</param>
        public static void AppendKeyValue(string key, string value)
        {
            XmlElement node = BPMCollection.CreateElement("Node");
            XmlElement id = BPMCollection.CreateElement("ID");
            id.InnerText = key;
            XmlElement bpm = BPMCollection.CreateElement("BPM");
            node.AppendChild(id);
            node.AppendChild(bpm);
            XmlNode root = BPMCollection.ChildNodes[1] ?? throw new NullReferenceException();
            root.AppendChild(node);
        }

        /// <summary>
        /// Append Nodes to BPM Table
        /// </summary>
        /// <param name="bpmTable">ID</param>
        public static void AppendBPMTable(string key, string value, BPMChanges bpmTable)
        {
            XmlElement node = BPMChangeTable.CreateElement("Node");
            XmlElement id = BPMChangeTable.CreateElement("ID");
            id.InnerText = key;
            XmlElement bpm = BPMChangeTable.CreateElement("BPM");
            node.AppendChild(id);
            node.AppendChild(bpm);
            foreach (BPMChange note in bpmTable.ChangeNotes)
            {
                XmlElement changeNote = BPMChangeTable.CreateElement("Note");
                XmlElement bar = BPMChangeTable.CreateElement("Bar");
                bar.InnerText = note.Bar.ToString();
                XmlElement tick = BPMChangeTable.CreateElement("Tick");
                tick.InnerText = note.Tick.ToString();
                XmlElement noteBPM = BPMChangeTable.CreateElement("BPM");
                noteBPM.InnerText = note.BPM.ToString();
                changeNote.AppendChild(bar);
                changeNote.AppendChild(tick);
                changeNote.AppendChild(noteBPM);
                bpm.AppendChild(changeNote);
            }
            XmlNode root = BPMChangeTable.ChildNodes[1] ?? throw new NullReferenceException();
            root.AppendChild(node);
        }
    }


}