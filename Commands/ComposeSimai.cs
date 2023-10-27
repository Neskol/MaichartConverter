using ManyConsole;
using MaiLib;

namespace MaichartConverter
{
    /// <summary>
    /// Compile Simai Command
    /// </summary>
    public class CompileSimai : ConsoleCommand
    {
        /// <summary>
        /// Return when command successfully executed
        /// </summary>
        private const int Success = 0;
        /// <summary>
        /// Return when command failed to execute
        /// </summary>
        private const int Failed = 2;

        /// <summary>
        /// Source file path
        /// </summary>
        public string? FileLocation { get; set; }
        /// <summary>
        /// Difficulty
        /// </summary>
        public string? Difficulty { get; set; }
        /// <summary>
        /// Destination of output
        /// </summary>
        public string? Destination { get; set; }
        /// <summary>
        /// Target Format of the file
        /// </summary>
        public string? TargetFormat { get; set; }
        /// <summary>
        /// Rotation option for charts
        /// </summary>
        /// <value>Clockwise90/180, Counterclockwise90/180, UpsideDown, LeftToRight</value>
        public string? Rotate { get; set; }
        /// <summary>
        /// OverallTick Shift for the chart: if the shift tick exceeds the 0 Bar 0 Tick, any note before 0 bar 0 tick will be discarded.
        /// </summary>
        /// <value>Tick, 384 tick = 1 bar</value>
        public int? ShiftTick { get; set; }

        /// <summary>
        /// Construct Command
        /// </summary>
        public CompileSimai()
        {
            IsCommand("CompileSimai", "Compile assigned simai chart to assigned format");
            HasLongDescription("This function enables user to compile simai chart specified to the format they want. By default is ma2 for simai.");
            HasRequiredOption("p|path=", "The path to file", path => FileLocation = path);
            HasOption("d|difficulty=", "The number representing the difficulty of chart -- 1-6 for Easy to Re:Master, 7 for Original/Utage", diff => Difficulty = diff);
            HasOption("f|format=", "The target format - simai or ma2", format => TargetFormat = format);
            HasOption("r|rotate=", "Rotating method to rotate a chart: Clockwise90/180, Counterclockwise90/180, UpsideDown, LeftToRight", rotate => Rotate = rotate);
            HasOption("s|shift=", "Overall shift to the chart in unit of tick", tick => ShiftTick = int.Parse(tick));
            HasOption("o|output=", "Export compiled chart to location specified", dest => Destination = dest);
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        /// <param name="remainingArguments">Rest of the arguments</param>
        /// <returns>Code of execution indicates if the commands is successfully executed</returns>
        /// <exception cref="FileNotFoundException">Raised when the file is not found</exception>
        public override int Run(string[] remainingArguments)
        {
            try
            {
                SimaiTokenizer tokenizer = new SimaiTokenizer();
                tokenizer.UpdateFromPath(FileLocation ?? throw new FileNotFoundException());
                SimaiParser parser = new SimaiParser();
                string[] tokensCandidates;
                if (Difficulty != null)
                {
                    tokensCandidates = tokenizer.ChartCandidates[Difficulty];
                }
                else
                {
                    tokensCandidates = tokenizer.ChartCandidates.Values.First();
                }
                Chart candidate = parser.ChartOfToken(tokensCandidates);
                if (Rotate != null)
                {
                    bool rotationIsValid = Enum.TryParse(Rotate, out NoteEnum.FlipMethod rotateMethod);
                    if (!rotationIsValid) throw new Exception("Given rotation method is not valid. Given: " + Rotate);
                    candidate.RotateNotes(rotateMethod);
                }
                if (ShiftTick != null && ShiftTick != 0)
                {
                    candidate.ShiftByOffset((int)ShiftTick);
                }
                SimaiCompiler compiler = new SimaiCompiler();
                string result = "";
                switch (TargetFormat)
                {
                    case "simai":
                        Simai resultChart = new Simai(candidate);
                        result = resultChart.Compose();
                        if (Destination != null && !Destination.Equals(""))
                        {
                            StreamWriter sw = new StreamWriter(Destination + Program.GlobalSep + "maidata.txt", false);
                            {
                                sw.WriteLine(result);
                            }
                            sw.Close();
                            if (File.Exists(Destination + Program.GlobalSep + "maidata.txt"))
                            {
                                Console.WriteLine("Successfully compiled at: " + Destination + Program.GlobalSep + "result.ma2");
                            }
                            else
                            {
                                throw new FileNotFoundException("THE FILE IS NOT SUCCESSFULLY COMPILED.");
                            }
                        }
                        else Console.WriteLine(result);
                        break;
                    case null:
                    case "":
                    case "ma2":
                        if (result.Equals(""))
                        {
                            Ma2 defaultChart = new Ma2(candidate);
                            result = defaultChart.Compose();
                        }
                        if (Destination != null && !Destination.Equals(""))
                        {
                            StreamWriter sw = new StreamWriter(Destination + Program.GlobalSep + "result.ma2", false);
                            {
                                sw.WriteLine(result);
                            }
                            sw.Close();
                            if (File.Exists(Destination + Program.GlobalSep + "result.ma2"))
                            {
                                Console.WriteLine("Successfully compiled at: " + Destination + Program.GlobalSep + "result.ma2");
                            }
                            else
                            {
                                throw new FileNotFoundException("THE FILE IS NOT SUCCESSFULLY COMPILED.");
                            }
                        }
                        else Console.WriteLine(result);
                        break;
                }

                return Success;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
                return Failed;
            }
        }
    }

}
