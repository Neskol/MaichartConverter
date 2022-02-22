using System;
using System.IO;

namespace MaichartConverter
{
    /// <summary>
    /// Main program of converter
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines Windows path seperator
        /// </summary>
        public static string windowsPathSep = "\\";

        /// <summary>
        /// Defines MacOS path seperator
        /// </summary>
        public static string macPathSep = "/";

        /// <summary>
        /// Defines which seperator is using in programs
        /// </summary>
        public static readonly string sep = macPathSep;

        /// <summary>
        /// Defines possible sorting scheme
        /// </summary>
        /// <value>Sorting scheme</value>
        public static readonly string[] categorize = { "Genre", "Level", "Cabinet","Composer","BPM","SD//DX Chart","No Separate Folder" };

        /// <summary>
        /// Program will sort output according to this
        /// </summary>
        public static string category = categorize[0];

        /// <summary>
        /// Main method to process charts
        /// </summary>
        /// <param name="args">Parameters to take in</param>     
        public static void Main(string[] args)
        {
            // TestSpecificChart("000834","4");
            CompileChartDatabase();
        }

        public static void TestSpecificChart(string musicID, string difficulty)
        {
            Chart good = new Ma2(@"/Users/neskol/MaiAnalysis/A000/music/music" + musicID + "/" + musicID + "_0" + difficulty + ".ma2");
            MaidataCompiler compiler = new MaidataCompiler();
            Console.WriteLine(good.Compose());
            Console.WriteLine(compiler.Compose(good));
            Console.WriteLine(good.Information);
        }

        /// <summary>
        /// Compile all maidata using value provided
        /// </summary>
        public static void CompileChartDatabase()
        {
            string sep = Program.sep;
            Console.WriteLine("Specify the path seperator this script is running on");
            sep = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            if (sep.Equals(""))
            {
                sep = Program.sep;
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
            Console.WriteLine("Specify the sorting method number the script will be used: ");
            for (int i = 0; i < Program.categorize.Length; i++)
            {
                Console.WriteLine("[" + i + "]" + " " + categorize[i]);
            }
            category = Console.ReadLine() ?? throw new NullReferenceException("Null For Console.ReadLine");
            try
            {
                if (0 <= Int32.Parse(category) && Int32.Parse(category) < categorize.Length)
                {
                    categorizeIndex = Int32.Parse(category);
                    category = categorize[Int32.Parse(category)];      
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " The program will use Genre as default method. Press any key to continue.");
                category = categorize[0];
                categorizeIndex = 0;
                Console.ReadKey();
            }

            Dictionary<string, string> bgaMap = new Dictionary<string, string>();
            if (exportBGA)
            {
                string[] bgaFiles = Directory.GetFiles(bgaLocation, "*.mp4");
                foreach (string bgaFile in bgaFiles)
                {
                    string musicID = bgaFile.Substring(bgaLocation.Length).Substring(0, 6).Substring(2, 4);
                    // Console.WriteLine(musicID);
                    // if (!bgaFile.Substring(bgaLocation.Length).Substring(0,3).Equals("mmv"))
                    // {
                    bgaMap.Add(musicID, bgaFile);
                    // }
                }
            }
            // Console.ReadLine();
            string[] musicFolders = Directory.GetDirectories(musiclocation);


            //GoodBrother1 test = new GoodBrother1(@"D:\MaiAnalysis\music\music000834\000834_04.ma2");
            //MaidataCompiler compiler = new MaidataCompiler(musiclocation + @"music000834\", @"D:\MaiAnalysis\Output\");
            //Console.WriteLine(compiler.Compose(test));

            //Create output directory
            DirectoryInfo output = new DirectoryInfo(outputLocation);
            // XmlInformation test = new XmlInformation(a000Location+ "music" + sep + "music010706" + sep + "");
            //string shortID = ComponsateZero(test.TrackID).Substring(2);
            //Console.WriteLine(shortID);
            //Console.ReadLine();
            //string oldName = imageLocation + "UI_Jacket_00" + shortID + ".png";
            //string newName = @"D:\bg.png";
            //File.Copy(oldName, newName);

            //Iterate music folders
            foreach (string track in musicFolders)
            {
                //try
                {
                    if (File.Exists(track + sep + "Music.xml"))
                    {
                        TrackInformation trackInfo = new XmlInformation(track + sep + "");
                        Console.WriteLine("There is Music.xml in " + track);
                        string shortID = ComponsateZero(trackInfo.TrackID).Substring(2);
                        Console.WriteLine("Name: " + trackInfo.TrackName);
                        Console.WriteLine("ID:" + trackInfo.TrackID);
                        Console.WriteLine("Genre: " + trackInfo.TrackGenre);
                        string[] categorizeScheme = { trackInfo.TrackGenre, trackInfo.TrackSymbolicLevel, trackInfo.TrackVersion,trackInfo.TrackComposer,trackInfo.TrackBPM,trackInfo.StandardDeluxePrefix,"" };
                        string defaultCategorizedPath = outputLocation + categorizeScheme[categorizeIndex];

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
                        if (!Directory.Exists(defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChart))
                        {
                            Directory.CreateDirectory(defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChart);
                            Console.WriteLine("Created song folder: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChart);
                        }
                        else
                        {
                            Console.WriteLine("Already exist song folder: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChart);
                        }
                        MaidataCompiler compiler = new MaidataCompiler(track + sep + "", defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChart);
                        Console.WriteLine("Finished compiling maidata " + trackInfo.TrackName + " to: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChart + sep + "maidata.txt");

                        if (exportAudio)
                        {
                            string originalMusicLocation = audioLocation;
                            originalMusicLocation += "music00" + shortID + ".mp3";
                            string newMusicLocation = defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChart + sep + "track.mp3";
                            if (!File.Exists(newMusicLocation))
                            {
                                File.Copy(originalMusicLocation, newMusicLocation);
                                Console.WriteLine("Exported music to: " + newMusicLocation);
                            }
                            else
                            {
                                Console.WriteLine("Audio already found in: " + newMusicLocation);
                            }
                        }

                        if (exportImage)
                        {
                            string originalImageLocation = imageLocation;
                            originalImageLocation += "UI_Jacket_00" + shortID + ".png";
                            string newImageLocation = defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChart + sep + "bg.png";
                            if (!File.Exists(newImageLocation))
                            {
                                File.Copy(originalImageLocation, newImageLocation);
                                Console.WriteLine("Image exported to: " + newImageLocation);
                            }
                            else
                            {
                                Console.WriteLine("Image already found in: " + newImageLocation);
                            }
                        }
                        // Console.WriteLine("Exported to: " + outputLocation + trackInfo.TrackGenre + sep + trackNameSubstitute + trackInfo.DXChart);

                        string? originalBGALocation = "";
                        bool bgaExists = bgaMap.TryGetValue(ComponsateZero(trackInfo.TrackID), out originalBGALocation);
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
                        if (exportBGA)
                        {
                            string? newBGALocation = defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChart + sep + "mv.mp4";
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
                        }
                        Console.WriteLine("Exported to: " + defaultCategorizedPath + sep + trackNameSubstitute + trackInfo.DXChart);
                        Console.WriteLine();
                    }

                }
                //catch (Exception ex)
                //{
                //    Console.WriteLine("There might not be Music.xml in " + track + ", Original message: " + ex.Message);
                //}
            }
        }
        /// <summary>
        /// Componsate 0 for music IDs
        /// </summary>
        /// <param name="intake">Music ID</param>
        /// <returns>0..+#Music ID and |Music ID|==6</returns>
        public static string ComponsateZero(string intake)
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
    }
}