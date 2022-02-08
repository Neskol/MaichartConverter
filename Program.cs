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
        /// Main method to process charts
        /// </summary>
        /// <param name="args">Parameters to takein</param>
        public static string windowsPathSep = "\\";
        public static string macPathSep = "/";
        public static void Main(string[] args)
        {
            // GoodBrother1 good = new GoodBrother1(@"/Users/neskol/MUG/maimai/SDEZ1.17/Package/Sinmai_Data/StreamingAssets/A000/music/music000837/000837_03.ma2");
            // MaidataCompiler compiler = new MaidataCompiler();
            // Console.WriteLine(good.Compose());
            // Console.WriteLine(compiler.Compose(good));
            string sep = macPathSep;
            Console.WriteLine("Specify A000 location: *Be sure to add "+sep+" in the end");
            bool exportBGA = true;
            string a000Location = Console.ReadLine()?? throw new NullReferenceException("Null For Console.ReadLine"); 
            if (a000Location.Equals(""))
            {
                a000Location = @"/Users/neskol/MaiAnalysis/A000/";
            }
            string musiclocation = a000Location + @"music"+sep;
            Console.WriteLine("Specify Audio location: *Be sure to add " + sep + " in the end");
            string audioLocation = Console.ReadLine()?? throw new NullReferenceException("Null For Console.ReadLine"); 
            if (audioLocation.Equals(""))
            {
                audioLocation = @"/Users/neskol/MaiAnalysis/Sound/";
            }
            Console.WriteLine("Specify Image location: *Be sure to add " + sep + "in the end");
            string imageLocation = Console.ReadLine()?? throw new NullReferenceException("Null For Console.ReadLine"); 
            if (imageLocation.Equals(""))
            {
                imageLocation = @"/Users/neskol/MaiAnalysis/Image/Texture2D/";
            }
            Console.WriteLine("Specify BGA location: *Be sure to add "+sep+" in the end");
            string bgaLocation = Console.ReadLine()?? throw new NullReferenceException("Null For Console.ReadLine"); 
            if (bgaLocation.Equals(""))
            {
                bgaLocation = @"/Users/neskol/MaiAnalysis/DXBGA/";
            }
            else if (bgaLocation.Equals("0"))
            {
                exportBGA=false;
            }
            Console.WriteLine("Specify Output location: *Be sure to add " + sep + " in the end");
            string outputLocation = Console.ReadLine()?? throw new NullReferenceException("Null For Console.ReadLine"); 
            if (outputLocation.Equals(""))
            {
                outputLocation = @"/Users/neskol/MaiAnalysis/Output/";
            }

            Dictionary<string, string> bgaMap = new Dictionary<string,string>();
            if (exportBGA)
            {
                string[] bgaFiles = Directory.GetFiles(bgaLocation,"*.mp4");               
                foreach (string bgaFile in bgaFiles)
                {
                   string musicID =bgaFile.Substring(bgaLocation.Length).Substring(0,6).Substring(2,4);
                  // Console.WriteLine(musicID);
                    // if (!bgaFile.Substring(bgaLocation.Length).Substring(0,3).Equals("mmv"))
                   // {
                  bgaMap.Add(musicID,bgaFile);
                   // }
                }
            }
            // Console.ReadLine();
            string[] musicFolders = Directory.GetDirectories(musiclocation);
            

            //GoodBrother1 test = new GoodBrother1(@"D:\MaiAnalysis\music\music000834\000834_04.ma2");
            //MaidataCompiler compiler = new MaidataCompiler(musiclocation + @"music000834\", @"D:\MaiAnalysis\Output\");
            //Console.WriteLine(compiler.Compose(test));

            //Create output drectory
            DirectoryInfo output = new DirectoryInfo(outputLocation);
            XmlInformation test = new XmlInformation(a000Location+ "music" + sep + "music010706" + sep + "");
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
                    if (File.Exists(track+ sep + "Music.xml"))
                    {
                        XmlInformation trackInfo = new XmlInformation(track + sep + "");
                        Console.WriteLine("There is Music.xml in " + track);
                        string shortID = ComponsateZero(trackInfo.TrackID).Substring(2);
                        Console.WriteLine("Name: " + trackInfo.TrackName);
                        Console.WriteLine("ID:" + trackInfo.TrackID);
                        Console.WriteLine("Genre: "+trackInfo.TrackGenre);
                        // Console.ReadLine();
                        string trackNameSubtitude = trackInfo.TrackSortName.Replace("" + sep + "", "of");
                        trackNameSubtitude = trackInfo.TrackSortName.Replace("/", "of");
                        if (!Directory.Exists(outputLocation + trackInfo.TrackGenre))
                        {
                            Directory.CreateDirectory(outputLocation + trackInfo.TrackGenre);
                            Console.WriteLine("Created folder: "+outputLocation+trackInfo.TrackGenre);
                        }
                        else{
                            Console.WriteLine("Already exist folder: "+outputLocation+trackInfo.TrackGenre);
                        }
                        if (!Directory.Exists(outputLocation + trackInfo.TrackGenre + sep + trackNameSubtitude + trackInfo.DXChart))
                        {
                            Directory.CreateDirectory(outputLocation + trackInfo.TrackGenre + sep + trackNameSubtitude + trackInfo.DXChart);
                            Console.WriteLine("Created song folder: "+outputLocation+trackInfo.TrackGenre+sep+trackNameSubtitude+trackInfo.DXChart);
                        }
                        else{
                            Console.WriteLine("Already exist song folder: "+outputLocation+trackInfo.TrackGenre+sep+trackNameSubtitude+trackInfo.DXChart);
                        }
                        MaidataCompiler compiler = new MaidataCompiler(track + sep + "", outputLocation + trackInfo.TrackGenre + sep + trackNameSubtitude + trackInfo.DXChart);
                        Console.WriteLine("Finished compiling maidata "+trackInfo.TrackName+" to: "+outputLocation+trackInfo.TrackGenre+sep+trackNameSubtitude+trackInfo.DXChart+sep+"maidata.txt");

                        string originalMusicLocation = audioLocation;
                        originalMusicLocation += "music00" + shortID + ".mp3";
                        string newMusicLocation = outputLocation + trackInfo.TrackGenre + sep + trackNameSubtitude + trackInfo.DXChart + sep + "track.mp3";
                        if (!File.Exists(newMusicLocation))
                        {
                            File.Copy(originalMusicLocation, newMusicLocation);
                            Console.WriteLine("Exported music to: "+newMusicLocation);
                        }
                        else{
                            Console.WriteLine("Audio already found in: "+newMusicLocation);
                        }

                        string originalImageLocation = imageLocation;
                        originalImageLocation += "UI_Jacket_00" + shortID + ".png";
                        string newImageLocation = outputLocation + trackInfo.TrackGenre + sep + trackNameSubtitude + trackInfo.DXChart + sep + "bg.png";
                        if (!File.Exists(newImageLocation))
                        {
                            File.Copy(originalImageLocation, newImageLocation);
                            Console.WriteLine("Image exported to: "+newImageLocation);
                        }
                        else{
                            Console.WriteLine("Image already found in: "+newImageLocation);
                        }
                        // Console.WriteLine("Exported to: " + outputLocation + trackInfo.TrackGenre + sep + trackNameSubtitude + trackInfo.DXChart);

                        string? originalBGALocation = "";
                        bool bgaExists = bgaMap.TryGetValue(ComponsateZero(trackInfo.TrackID),out originalBGALocation);
                        if (!bgaExists)
                        {
                            if (trackInfo.TrackID.Length==5)
                            {
                                bgaExists = bgaMap.TryGetValue(trackInfo.TrackID.Substring(1,4),out originalBGALocation);
                            }
                            else if (trackInfo.TrackID.Length==3)
                            {
                                bgaExists = bgaMap.TryGetValue(ComponsateShortZero(trackInfo.TrackID),out originalBGALocation);
                            }
                            
                        }
                        if (exportBGA)
                        {
                            string? newBGALocation = outputLocation+trackInfo.TrackGenre + sep + trackNameSubtitude + trackInfo.DXChart + sep +"pv.mp4";
                            if (bgaExists&&!File.Exists(newBGALocation))
                            {
                            Console.WriteLine("A BGA file was found in "+originalBGALocation);
                            var originalBGALocationCandidate = originalBGALocation ?? throw new NullReferenceException();
                            File.Copy(originalBGALocationCandidate,newBGALocation);
                            Console.WriteLine("Exported BGA file to: "+newBGALocation);
                            }
                            else if(bgaExists&& File.Exists(newBGALocation))
                            {
                                Console.WriteLine("BGA already found in "+newBGALocation);
                            }
                        }
                        Console.WriteLine("Exported to: " + outputLocation + trackInfo.TrackGenre + sep + trackNameSubtitude + trackInfo.DXChart);
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
                return "Exception raised: "+ex.Message;
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
                return "Exception raised: "+ex.Message;
            }
        }
    }
}