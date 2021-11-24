using System;
using System.Collections.Generic;
using System.IO;

namespace MusicConverterTest
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Specify location:");
            GoodBrother1 test = new GoodBrother1(@"D:\MaiAnalysis\music\music000799\000799_04.ma2");
            MaidataCompiler compiler = new MaidataCompiler();
            Console.WriteLine(compiler.Compose(test));

        }
    }
}