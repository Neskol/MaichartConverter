using ManyConsole;
using MaiLib;

namespace MaichartConverter;

public class GeneratePracticeChartBySimai : ConsoleCommand
{
    public const int Success = 0;
    public const int Failed = 2;

    public string SourceLocation { get; private set; }
    public string OutputLocation { get; private set; }
    public string? StartStamp { get; private set; }
    public string? EndStamp { get; private set; }
    public int RepeatTimes { get; private set; }

    public GeneratePracticeChartBySimai()
    {
        SourceLocation = "";
        OutputLocation = "";
        RepeatTimes = 1;
        IsCommand("GeneratePracticeChartBySimai",
            "Generates chart snippets for practicing based on existing Simai chart folder");
        HasRequiredOption("p|path=", "Load Simai files from this location", path => SourceLocation = path);
        HasRequiredOption("o|output=", "Write extracted files into this directory. New folder will be created if not provided",
            output => OutputLocation = output);
        HasOption("s|start=", "Defines start of snippet", start => StartStamp = start);
        HasOption("e|end=", "Defines end of snippet", end => EndStamp = end);
        HasOption("r|repeat=", "Times that the snippet will be played", times => RepeatTimes = int.Parse(times));
    }

    public double TimeHandler(string input)
    {
        throw new NotImplementedException();
    }

    public override int Run(string[] remainingArguments)
    {
        throw new NotImplementedException();
    }
}
