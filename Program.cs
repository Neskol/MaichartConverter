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
            string a000Location = @"C:\MUG\maimai\SDEZ1.10\Package\Sinmai_Data\StreamingAssets\A000\";
            string musiclocation = a000Location + @"music\";
            Console.WriteLine("Specify Audio location:");
            string audioLocation = Console.ReadLine();
            Console.WriteLine("Specify Image location");
            string imageLocation = Console.ReadLine();
            string[] musicFolders = Directory.GetDirectories(musiclocation);
            //GoodBrother1 test = new GoodBrother1(@"D:\MaiAnalysis\music\music000834\000834_04.ma2");
            MaidataCompiler compiler = new MaidataCompiler(musiclocation+@"music000834\", @"D:\MaiAnalysis\Output\");
            //Console.WriteLine(compiler.Compose(test));

        }
    }
}