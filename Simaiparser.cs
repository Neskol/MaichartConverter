namespace MaichartConverter;

public class SimaiParser : IParser
{
    public readonly string[] State = { "Note","Tap","Break","Touch","EXTap","Slide","Hold","EXHold","TouchHold","BPM","Quaver","Information" };
    public readonly string[] Status = { "Ready","Submit"};
    /// <summary>
    /// Constructor of simaiparser
    /// </summary>
    public SimaiParser()
    {
    }

    public BPMChanges BPMChangesOfToken(string token)
    {
        throw new NotImplementedException();
    }

    public Chart ChartOfToken(string[] token)
    {
        throw new NotImplementedException();
        int bar = 0;
        int tick = 0;
        int multiplier = 1;
        double bpm = 0.0;
        string status = "";
        string storage = "";
        Chart result;

        foreach (string symbol in token)
        {
            switch (symbol)
            {
                case "&":
                    status = "Information";
                    break;
                case "=":
                    break;
                default:
                    storage += symbol;
                    break;
                
            }
        }

        return result;
    }

    public Hold HoldOfToken(string token)
    {
        throw new NotImplementedException();
    }

    public MeasureChanges MeasureChangesOfToken(string token)
    {
        throw new NotImplementedException();
    }

    public Note NoteOfToken(string token)
    {
        throw new NotImplementedException();
    }

    public Slide SlideOfToken(string token)
    {
        throw new NotImplementedException();
    }

    public Tap TapOfToken(string token)
    {
        throw new NotImplementedException();
    }
}