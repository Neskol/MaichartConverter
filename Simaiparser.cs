using System.Collections.Generic;

namespace MaichartConverter;

/// <summary>
/// Parse charts in Simai format
/// </summary>
public class SimaiParser : IParser
{
    /// <summary>
    /// Enums of variables in Simai file
    /// </summary>
    /// <value>Common variables</value>
    public readonly string[] State = { "Note","Tap","Break","Touch","EXTap","Slide","Hold","EXHold","TouchHold","BPM","Quaver","Information" };

    /// <summary>
    /// Enums of parser state
    /// </summary>
    /// <value></value>
    public readonly string[] Status = { "Ready","Submit"};

    /// <summary>
    /// The maximum definition of a chart
    /// </summary>
    public static int MaximumDefinition = 384;

    /// <summary>
    /// Constructor of simaiparser
    /// </summary>
    public SimaiParser()
    {
    }

    /// <summary>
    /// Parse BPM change notes
    /// </summary>
    /// <param name="token">The parsed set of BPM change</param>
    /// <returns>Error: simai does not have this variable</returns>
    public BPMChanges BPMChangesOfToken(string token)
    {
        throw new NotImplementedException();
    }

    public Chart ChartOfToken(string[] tokens)
    {
        throw new NotImplementedException();
        Chart result = new Simai2();

        List<Note> notes = new List<Note>();
        BPMChanges bpmChanges = new BPMChanges();
        MeasureChanges measureChanges = new MeasureChanges();
        int bar=0;
        int tick=0;
        int tickStep=MaximumDefinition;
        for (int i = 0;i<tokens.Length;i++)
        {
            bool isBPM = tokens[i].Contains("(");
            bool isMeasure = tokens[i].Contains("{");
            bool ended = tokens[i].Equals("E");

            if (isBPM)
            {
                string bpm = tokens[i];
                bpm.Replace("(","");
                bpm.Replace(")","");
                bpmChanges.Add(new BPMChange(bar,tick,Double.Parse(bpm)));
            }
            else if (isMeasure)
            {
                string quaverCandidate = tokens[i];
                quaverCandidate.Replace("{","");
                quaverCandidate.Replace("}","");
                tickStep = MaximumDefinition/Int32.Parse(quaverCandidate);
            }
            else
            {
                notes.Add(NoteOfToken(tokens[i]));
            }

            tick+=tickStep;
            while (tick>=MaximumDefinition)
            {
                tick-=MaximumDefinition;
                bar++;
            }
        }

        return result;
    }

    public Hold HoldOfToken(string token, int bar, int tick)
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

    public Slide SlideOfToken(string token, int bar, int tick)
    {
        throw new NotImplementedException();
    }

    public Tap TapOfToken(string token, int bar, int tick)
    {
        throw new NotImplementedException();
    }
}