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
        Chart result = new Simai2();

        foreach (string block in token)
        {
            
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