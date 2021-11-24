using System;
using System.Collections.Generic;
using System.IO;

namespace MusicConverterTest
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Specify A000 location:");
            //string a000Location = Console.ReadLine();
            string a000Location = @"C:\MUG\maimai\SDEZ1.17\Package\Sinmai_Data\StreamingAssets\A000\";
            string musiclocation = a000Location + @"music\";
            Console.WriteLine("Specify Audio location:");
            //string audioLocation = Console.ReadLine();
            string audioLocation = @"D:\MaiAnalysis\Audio\";
            Console.WriteLine("Specify Image location");
            //string imageLocation = Console.ReadLine();
            string imageLocation = @"D:\MaiAnalysis\Image\Texture2D\";
            Console.WriteLine("Specify Output location;");
            string outputLocation = @"D:\MaiAnalysis\Output\";
            //string outputLocation = Console.ReadLine();
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
                    if (File.Exists(originalImageLocation + "UI_Jacket_00" + shortID + ".png"))
                    {
                        originalImageLocation += "UI_Jacket_00" + shortID + ".png";
                    }
                    else
                    {
                        originalImageLocation += "UI_Jacket_000000.png";
                    }
                    string newImageLocation = outputLocation + trackInfo.TrackGenre + "\\" + trackInfo.TrackName + trackInfo.DXChart + "\\bg.png";
                    File.Copy(originalImageLocation, newImageLocation);
                    Console.WriteLine("Exported to: " + outputLocation + trackInfo.TrackGenre + "\\" + trackInfo.TrackName + trackInfo.DXChart);
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
            while (result.Length<6)
            {
                result = "0" + result;
            }
            return result;
        }
    }
}