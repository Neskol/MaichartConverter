using ManyConsole;
using MaiLib;
using CSCore.CoreAudioAPI;

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

    public static double TimeHandler(List<BPMChange> changeTable, string input)
    {
        string buffer = "";
        foreach (char token in input)
        {

        }
        throw new NotImplementedException();
    }

    public static double BarTickHandler(string input)
    {
        int barCount;
        int tickCount;
        int resolution = 384;
        string buffer = "";
        string state = "NORMAL";

        void SubmitBuffer()
        {
            switch (state)
            {
                case "RESOLUTION":
                    resolution = int.Parse(buffer);
                    break;
                case "BAR":
                    barCount = int.Parse(buffer);
                    break;
                case "TICK":
                    tickCount = int.Parse(buffer);
                    break;
            }
        }
        foreach (char token in input) switch (token)
        {
            case 'R':
                SubmitBuffer();
                state = "RESOLUTION";
                buffer = "";
                break;
            case 'B':
                SubmitBuffer();
                state = "BAR";
                buffer = "";
                break;
            case 'T':
                SubmitBuffer();
                state = "TICK";
                buffer = "";
                break;
            default:
                if (char.IsDigit(token)) buffer += token;
                else throw new InvalidDataException($"Unexpected token when parsing in state if {state}: {token}");
                break;
        }

        return 0;
    }

    public override int Run(string[] remainingArguments)
    {
        throw new NotImplementedException();
    }
}
