namespace MusicConverterTest
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
        public static void Main(string[] args)
        {
            //GoodBrother1 good = new GoodBrother1(@"C:\MUG\maimai\SDEZ1.17\Package\Sinmai_Data\StreamingAssets\A000\music\music010706\010706_03.ma2");
            //MaidataCompiler compiler = new MaidataCompiler();
            //Console.WriteLine(good.Compose());
            //Console.WriteLine(compiler.Compose(good));

            Console.WriteLine("Specify A000 location: *Be sure to add \\ in the end");
#pragma warning disable CS8600 // 将 null 文本或可能的 null 值转换为不可为 null 类型。
            string a000Location = Console.ReadLine();
#pragma warning restore CS8600 // 将 null 文本或可能的 null 值转换为不可为 null 类型。
#pragma warning disable CS8602 // 解引用可能出现空引用。
            if (a000Location.Equals(""))
#pragma warning restore CS8602 // 解引用可能出现空引用。
            {
                a000Location = @"C:\MUG\maimai\SDEZ1.17\Package\Sinmai_Data\StreamingAssets\A000\";
            }
            string musiclocation = a000Location + @"music\";
            Console.WriteLine("Specify Audio location: *Be sure to add \\ in the end");
#pragma warning disable CS8600 // 将 null 文本或可能的 null 值转换为不可为 null 类型。
            string audioLocation = Console.ReadLine();
#pragma warning restore CS8600 // 将 null 文本或可能的 null 值转换为不可为 null 类型。
#pragma warning disable CS8602 // 解引用可能出现空引用。
            if (audioLocation.Equals(""))
#pragma warning restore CS8602 // 解引用可能出现空引用。
            {
                audioLocation = @"F:\MaiAnalysis\Audio\";
            }
            Console.WriteLine("Specify Image location: *Be sure to add \\ in the end");
#pragma warning disable CS8600 // 将 null 文本或可能的 null 值转换为不可为 null 类型。
            string imageLocation = Console.ReadLine();
#pragma warning restore CS8600 // 将 null 文本或可能的 null 值转换为不可为 null 类型。
#pragma warning disable CS8602 // 解引用可能出现空引用。
            if (imageLocation.Equals(""))
#pragma warning restore CS8602 // 解引用可能出现空引用。
            {
                imageLocation = @"F:\MaiAnalysis\Image\Texture2D\";
            }
            Console.WriteLine("Specify Output location: *Be sure to add \\ in the end");
            string outputLocation = Console.ReadLine();
            if (outputLocation.Equals(""))
            {
#pragma warning disable CS8600 // 将 null 文本或可能的 null 值转换为不可为 null 类型。
                outputLocation = @"F:\MaimaiAnalysis\Output\";
#pragma warning restore CS8600 // 将 null 文本或可能的 null 值转换为不可为 null 类型。
            }
            string[] musicFolders = Directory.GetDirectories(musiclocation);
            //GoodBrother1 test = new GoodBrother1(@"D:\MaiAnalysis\music\music000834\000834_04.ma2");
            //MaidataCompiler compiler = new MaidataCompiler(musiclocation + @"music000834\", @"D:\MaiAnalysis\Output\");
            //Console.WriteLine(compiler.Compose(test));

            //Create output drectory
#pragma warning disable CS8604 // “DirectoryInfo.DirectoryInfo(string path)”中的形参“path”可能传入 null 引用实参。
            DirectoryInfo output = new DirectoryInfo(outputLocation);
#pragma warning restore CS8604 // “DirectoryInfo.DirectoryInfo(string path)”中的形参“path”可能传入 null 引用实参。
            XmlInformation test = new XmlInformation(a000Location+"music\\music010706\\");
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
                    if (File.Exists(track+"\\Music.xml"))
                    {
                        XmlInformation trackInfo = new XmlInformation(track + "\\");
                        Console.WriteLine("There is Music.xml in " + track);
                        string shortID = ComponsateZero(trackInfo.TrackID).Substring(2);
                        Console.WriteLine("Name: " + trackInfo.TrackName);
                        Console.WriteLine("ID:" + trackInfo.TrackID);
                        string trackNameSubtitude = trackInfo.TrackSortName.Replace("\\", "of");
                        trackNameSubtitude = trackInfo.TrackSortName.Replace("/", "of");
                        if (!Directory.Exists(outputLocation + trackInfo.TrackGenre))
                        {
                            Directory.CreateDirectory(outputLocation + trackInfo.TrackGenre);
                        }
                        if (!Directory.Exists(outputLocation + trackInfo.TrackGenre + "\\" + trackNameSubtitude + trackInfo.DXChart))
                        {
                            Directory.CreateDirectory(outputLocation + trackInfo.TrackGenre + "\\" + trackNameSubtitude + trackInfo.DXChart);
                        }
                        MaidataCompiler compiler = new MaidataCompiler(track + "\\", outputLocation + trackInfo.TrackGenre + "\\" + trackNameSubtitude + trackInfo.DXChart);

                        string originalMusicLocation = audioLocation;
                        originalMusicLocation += "music00" + shortID + ".mp3";
                        string newMusicLocation = outputLocation + trackInfo.TrackGenre + "\\" + trackNameSubtitude + trackInfo.DXChart + "\\track.mp3";
                        if (!File.Exists(newMusicLocation))
                        {
                            File.Copy(originalMusicLocation, newMusicLocation);
                        }

                        string originalImageLocation = imageLocation;
                        originalImageLocation += "UI_Jacket_00" + shortID + ".png";
                        string newImageLocation = outputLocation + trackInfo.TrackGenre + "\\" + trackNameSubtitude + trackInfo.DXChart + "\\bg.png";
                        if (!File.Exists(newImageLocation))
                        {
                            File.Copy(originalImageLocation, newImageLocation);
                        }
                        Console.WriteLine("Exported to: " + outputLocation + trackInfo.TrackGenre + "\\" + trackNameSubtitude + trackInfo.DXChart);
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
                return "";
            }
        }
    }
}