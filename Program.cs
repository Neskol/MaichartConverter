namespace MusicConverterTest
{
    class Program
    {
        public static void Main(string[] args)
        {
            //GoodBrother1 good = new GoodBrother1(@"C:\MUG\maimai\SDEZ1.17\Package\Sinmai_Data\StreamingAssets\A000\music\music011089\011089_03.ma2");
            //MaidataCompiler compiler = new MaidataCompiler();
            //Console.WriteLine(good.Compose());
            //Console.WriteLine(compiler.Compose(good));

            Console.WriteLine("Specify A000 location: *Be sure to add \\ in the end");
            string a000Location = Console.ReadLine();
            if (a000Location.Equals(""))
            {
                a000Location = @"C:\MUG\maimai\SDEZ1.17\Package\Sinmai_Data\StreamingAssets\A000\";
            }
            string musiclocation = a000Location + @"music\";
            Console.WriteLine("Specify Audio location: *Be sure to add \\ in the end");
            string audioLocation = Console.ReadLine();
            if (audioLocation.Equals(""))
            {
                audioLocation = @"D:\MaiAnalysis\Audio\";
            }
            Console.WriteLine("Specify Image location: *Be sure to add \\ in the end");
            string imageLocation = Console.ReadLine();
            if (imageLocation.Equals(""))
            {
                imageLocation = @"D:\MaiAnalysis\Image\Texture2D\";
            }
            Console.WriteLine("Specify Output location: *Be sure to add \\ in the end");
            string outputLocation = @"D:\MaiAnalysis\Output\";
            if (outputLocation.Equals(""))
            {
                outputLocation = Console.ReadLine();
            }
            string[] musicFolders = Directory.GetDirectories(musiclocation);
            //GoodBrother1 test = new GoodBrother1(@"D:\MaiAnalysis\music\music000834\000834_04.ma2");
            //MaidataCompiler compiler = new MaidataCompiler(musiclocation+@"music000834\", @"D:\MaiAnalysis\Output\");
            //Console.WriteLine(compiler.Compose(test));

            //Create output drectory
            DirectoryInfo output = new DirectoryInfo(outputLocation);
            //XmlUtility test = new XmlUtility(@"D:\MaiAnalysis\music\music000835\");
            //string shortID = ComponsateZero(test.TrackID).Substring(2);
            //Console.WriteLine(shortID);
            //Console.ReadLine();
            //string oldName = imageLocation + "UI_Jacket_00" + shortID + ".png";
            //string newName = @"D:\bg.png";
            //File.Copy(oldName, newName);


            //Iterate music folders
            foreach (string track in musicFolders)
            {
                try
                {
                    XmlUtility trackInfo = new XmlUtility(track + "\\");
                    Console.WriteLine("There is Music.xml in " + track);
                    string shortID = ComponsateZero(trackInfo.TrackID).Substring(2);
                    Console.WriteLine(shortID);
                    if (!Directory.Exists(outputLocation + trackInfo.TrackGenre))
                    {
                        Directory.CreateDirectory(outputLocation + trackInfo.TrackGenre);
                    }
                    if (!Directory.Exists(outputLocation + trackInfo.TrackGenre + "\\" + trackInfo.TrackName + trackInfo.DXChart))
                    {
                        Directory.CreateDirectory(outputLocation + trackInfo.TrackGenre + "\\" + trackInfo.TrackName + trackInfo.DXChart);
                    }
                    MaidataCompiler compiler = new MaidataCompiler(track + "\\", outputLocation + trackInfo.TrackGenre + "\\" + trackInfo.TrackName + trackInfo.DXChart);

                    string originalMusicLocation = audioLocation;
                    originalMusicLocation += "music00" + shortID + ".mp3";
                    string newMusicLocation = outputLocation + trackInfo.TrackGenre + "\\" + trackInfo.TrackName + trackInfo.DXChart + "\\track.mp3";
                    File.Copy(originalMusicLocation, newMusicLocation);

                    string originalImageLocation = imageLocation;
                    originalImageLocation += "UI_Jacket_00" + shortID + ".png";
                    string newImageLocation = outputLocation + trackInfo.TrackGenre + "\\" + trackInfo.TrackName + trackInfo.DXChart + "\\bg.png";
                    File.Copy(originalImageLocation, newImageLocation);
                    Console.WriteLine("Exported to: " + outputLocation + trackInfo.TrackGenre + "\\" + trackInfo.TrackName + trackInfo.DXChart);
                    Console.WriteLine("Find bg in 835:" + File.Exists(@"D:\MaiAnalysis\Output\maimai\Believe The Railbow\bg.png"));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("There might not be Music.xml in " + track);
                }
            }
        }
        /// <summary>
        /// Componsate 0 for music IDs
        /// </summary>
        /// <param name="intake">Music ID</param>
        /// <returns>0..+#Music ID and |Music ID|==6</returns>
        public static string ComponsateZero(string intake)
        {
            string result = intake;
            while (result.Length < 6)
            {
                result = "0" + result;
            }
            return result;
        }
    }
}