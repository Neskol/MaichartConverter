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
    public readonly string[] State = { "Note", "Tap", "Break", "Touch", "EXTap", "Slide", "Hold", "EXHold", "TouchHold", "BPM", "Quaver", "Information" };

    /// <summary>
    /// Enums of parser state
    /// </summary>
    /// <value></value>
    public readonly string[] Status = { "Ready", "Submit" };

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
        throw new NotImplementedException("Simai does not have this component");
    }

    public Chart ChartOfToken(string[] tokens)
    {
        throw new NotImplementedException();
        Chart result = new Simai2();

        List<Note> notes = new List<Note>();
        BPMChanges bpmChanges = new BPMChanges();
        MeasureChanges measureChanges = new MeasureChanges();
        int bar = 0;
        int tick = 0;
        double currentBPM = 0.0;
        int tickStep = MaximumDefinition;
        for (int i = 0; i < tokens.Length; i++)
        {
            bool isBPM = tokens[i].Contains("(");
            bool isMeasure = tokens[i].Contains("{");
            bool ended = tokens[i].Equals("E");

            if (isBPM)
            {
                string bpm = tokens[i];
                bpm.Replace("(", "");
                bpm.Replace(")", "");
                bpmChanges.Add(new BPMChange(bar, tick, Double.Parse(bpm)));
                currentBPM = Double.Parse(bpm);
            }
            else if (isMeasure)
            {
                string quaverCandidate = tokens[i];
                quaverCandidate.Replace("{", "");
                quaverCandidate.Replace("}", "");
                tickStep = MaximumDefinition / Int32.Parse(quaverCandidate);
            }
            else
            {
                List<string> eachPairCandidates = EachGroupOfToken(tokens[i]);
                foreach (string eachNote in eachPairCandidates)
                {
                    notes.Add(NoteOfToken(eachNote));
                }
            }

            tick += tickStep;
            while (tick >= MaximumDefinition)
            {
                tick -= MaximumDefinition;
                bar++;
            }
        }

        return result;
    }

    public Hold HoldOfToken(string token, int bar, int tick)
    {
        throw new NotImplementedException();
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
        Note result = new Rest();
        bool isSlide = token.Contains("-") ||
        token.Contains("v") ||
        token.Contains("w") ||
        token.Contains("<") ||
        token.Contains(">") ||
        token.Contains("p") ||
        token.Contains("q") ||
        token.Contains("s") ||
        token.Contains("z") ||
        token.Contains("V");
        bool isHold = !isSlide && token.Contains("[");
        if (isSlide)
        {
            result = SlideOfToken(token);
        }
        else if (isHold)
        {
            result = HoldOfToken(token);
        }
        else
        {
            result = TapOfToken(token);
        }
        return result;
    }

    public Note NoteOfToken(string token, int bar, int tick, double bgm)
    {
        Note result = new Rest();
        bool isSlide = token.Contains("-") ||
        token.Contains("v") ||
        token.Contains("w") ||
        token.Contains("<") ||
        token.Contains(">") ||
        token.Contains("p") ||
        token.Contains("q") ||
        token.Contains("s") ||
        token.Contains("z") ||
        token.Contains("V");
        bool isHold = !isSlide && token.Contains("[");
        if (isSlide)
        {
            result = SlideOfToken(token, bar, tick);
        }
        else if (isHold)
        {
            result = HoldOfToken(token, bar, tick);
        }
        else
        {
            result = TapOfToken(token, bar, tick);
        }
        return result;
    }

    public Slide SlideOfToken(string token, int bar, int tick)
    {
        throw new NotImplementedException();
    }

    public Slide SlideOfToken(string token)
    {
        throw new NotImplementedException();
    }

    public Tap TapOfToken(string token, int bar, int tick)
    {
        bool isBreak = token.Contains("b");
        bool isEXTap = token.Contains("x");
        bool isTouch = token.Contains("B") ||
        token.Contains("C") ||
        token.Contains("E") ||
        token.Contains("F");
        Tap result = new Tap();
        if (isTouch)
        {
            bool hasSpecialEffect = token.Contains("f");
            if (hasSpecialEffect)
            {
                result = new Tap("TTP", bar, tick, token.Substring(0, 1) + Int32.Parse(token.Substring(1, 1) + 1),1,"M1");

            }
            else result = new Tap("TTP", bar, tick, token.Substring(0, 1) + Int32.Parse(token.Substring(1, 1) + 1),0,"M1");
        }
        else if (isEXTap)
        {
            result = new Tap("XTP", bar, tick, token.Substring(0, 1));
        }
        else if (isBreak)
        {
            result = new Tap("BRK", bar, tick, token.Substring(0, 1));
        }
        else result = new Tap("TAP", bar, tick, token.Substring(0, 1));
        return result;
    }

    public Tap TapOfToken(string token)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Deal with old, outfashioned and illogical Simai Each Groups.
    /// </summary>
    /// <param name="token">Tokens that potentially contains each Groups</param>
    /// <returns>List of strings that is composed with single note.</returns>
    public static List<string> EachGroupOfToken(string token)
    {
        List<string> result = new List<string>();
        if (token.Contains("/"))
        {
            string[] candidate = token.Split("/");
            foreach (string tokenCandidate in candidate)
            {
                result.AddRange(EachGroupOfToken(tokenCandidate));
            }
        }
        else if (Int32.TryParse(token,out int eachPair))
        {
            char[] eachPairs = token.ToCharArray();
            foreach (char x in eachPairs)
            {
                result.Add(x.ToString());
            }
        }
        return result;
    }

    /// <summary>
    /// Deal with annoying vigours Slide grammar of Simai
    /// </summary>
    /// <param name="token">Token that potentially contains slide note</param>
    /// <returns>A list of string extracts each note</returns>
    public static List<string> ExtractSlideComponents(string token)
    {
        List<string> result = new List<string>();
        return result;
    }
}