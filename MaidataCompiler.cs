using System;
using System.Collections.Generic;
using System.Text;
using static MusicConverterTest.GoodBrother1;

namespace MusicConverterTest
{
    internal class MaidataCompiler : ICompiler
    {
        private GoodBrother1[] charts;
        private Dictionary<string, string> information;
        public MaidataCompiler(GoodBrother1[] intakeChart)
        {
            this.charts = intakeChart;
        }

        public MaidataCompiler()
        {

        }

        public bool CheckValidity()
        {
            bool result = true;
            foreach (GoodBrother1 x in charts)
            {
                result = result && x.CheckValidity();
            }
            return result;
        }

        public string Compose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return compose of specified chart.
        /// </summary>
        /// <param name="chart">Chart to compose</param>
        /// <returns>Maidata of specified chart WITHOUT headers</returns>
        public string Compose(GoodBrother1 chart)
        {
            ////Set all storage variables
            //int nextBpmChangeIndex = 0;
            //int nextBpmChangeBar= chart.BPMChanges.Bar[nextBpmChangeIndex];
            //int nextBpmChangeQuaver = chart.BPMChanges.Tick[nextBpmChangeIndex];
            //int nextMeasureChangeIndex = 0;
            //int nextMeasureChangeBar= chart.MeasureChanges.Bar[nextMeasureChangeIndex];
            //int nextMeasureChangeQuaver = chart.MeasureChanges.Tick[nextMeasureChangeIndex];
            //string lastNote = "";
            //string result = "";
            //bool prettyPrint=false;
            //int bar = 0;
            //int measure = 0;
            //int quaver = chart.MeasureChanges.Quavers[0];
            //int beat = chart.MeasureChanges.Beats[0];
            //double bpm = chart.BPMChanges.Bpm[0];
            //bool separate = true;
            //for (int i = 0; i < chart.Notes.Count; i++)
            //{
            //    //Print measure changes: (BPM)

            //    //Print measure changes: {Quaver}

            //    //Reserve time: Only in first note
            //    if (i == 0)
            //    {
            //        int prefix = chart.MeasureChanges.Beats[0];
            //        for (int x = 0; x < prefix-1; x++)
            //        {
            //            result += ",";
            //            result += "\n";
            //        }
            //    }
            //    //Process Both Note
            //    if (chart.Notes[i].Bar == bar && chart.Notes[i].StartTime == measure)
            //    {
            //        if ((!IsSlideStart(lastNote) && !IsSlide(lastNote))||
            //            (IsSlide(lastNote) && !IsSlide(chart.Notes[i].NoteType)))
            //        {
            //            result += "/";
            //            separate = false;
            //        }
            //        else if ((!IsSlideStart(lastNote) && IsSlide(chart.Notes[i].NoteType))||
            //            (IsSlide(lastNote) && IsSlide(chart.Notes[i].NoteType)))
            //        {
            //            result += "*";
            //            separate = false;
            //        }

            //    }
            //    else if ((chart.Notes[i].Bar != bar || chart.Notes[i].StartTime != quaver) && separate)
            //    {
            //        result += ",";
            //    }
            //    //Add separator

            //    //Compose note
            //    result +=chart.Notes[i].Compose(0);
            //    prettyPrint = chart.Notes[i].Bar > bar;
            //    bar = chart.Notes[i].Bar;
            //    measure = chart.Notes[i].StartTime;
            //    lastNote = chart.Notes[i].NoteType;
            //    separate = true;
            //    //Auto return
            //    if (prettyPrint)
            //    {
            //        result += "\n";
            //        prettyPrint=false;
            //    }
            //}
            //result += ",E";
            //return result;
            string result = "";
            Console.WriteLine(chart.Compose());
            foreach (BPMChange x in chart.BPMChanges.ChangeNotes)
            {
                Console.WriteLine("BPM Change verified in "+x.Bar+" "+x.StartTime+" of BPM"+x.BPM);
            }
            foreach(List<Note> bar in chart.Chart)
            {
                Note lastNote = new MeasureChange();
                //result += bar[1].Bar;
                foreach (Note x in bar)
                {
                    switch(lastNote.NoteSpecificGenre())
                    {
                        case "MEASURE":
                            break;
                        case "BPM":
                            break;
                        case "TAP":
                            if (x.IsNote() && (!IsSlide(x.NoteType))&& x.StartTime == lastNote.StartTime&&!x.NoteGenre().Equals("BPM"))
                            {
                                result += "/";
                            }
                            else result += ",";
                            break;
                        case "HOLD":
                            if (x.IsNote() && !IsSlide(x.NoteType) && x.StartTime == lastNote.StartTime && !x.NoteGenre().Equals("BPM"))
                            {
                                result += "/";
                            }
                            else result += ",";
                            break;
                        case "SLIDE_START":
                            //if (x.IsNote() && x.NoteSpecificGenre().Equals("SLIDE"))
                            //{

                            //}
                            break;
                        case "SLIDE":
                            if (x.IsNote() && (!IsSlide(x.NoteType)) && x.StartTime == lastNote.StartTime && !x.NoteGenre().Equals("BPM"))
                            {
                                result += "/";
                            }
                            else if (x.IsNote() && IsSlide(x.NoteType) && x.StartTime == lastNote.StartTime && !x.NoteGenre().Equals("BPM"))
                            {
                                result += "*";
                            }
                            else result += ",";
                            break;
                        default:
                            result += ",";
                            break;
                    }
                    result += x.Compose(0);
                    lastNote = x;
                    //if (x.NoteGenre().Equals("BPM"))
                    //{
                    //    result+="("+ x.Bar + "_" + x.StartTime + ")";
                    //}
                }
                result += ",\n";
            }
            result += "E";
            return result;
        }

        public void TakeInformation(Dictionary<string, string> information)
        {
            this.information = information;
        }

        /// <summary>
        /// Return if this note is Slide.
        /// </summary>
        /// <param name="noteType">String of Note Type</param>
        /// <returns>True if it is, false elsewise</returns>
        static bool IsSlide(string noteType)
        {
            bool result = true;
            result = result && (noteType.Equals("SI_") ||
                noteType.Equals("SV_") ||
                noteType.Equals("SF_") ||
                noteType.Equals("SCL") ||
                noteType.Equals("SCR") ||
                noteType.Equals("SLL") ||
                noteType.Equals("SLR") ||
                noteType.Equals("SUL") ||
                noteType.Equals("SUR") ||
                noteType.Equals("SXL") ||
                noteType.Equals("SXR"));
            return result;
        }

        /// <summary>
        /// Return if this note is Slide Start.
        /// </summary>
        /// <param name="noteType">String of Note Type</param>
        /// <returns>True if it is, false elsewise</returns>
        static bool IsSlideStart(string noteType)
        {
            bool result = true;
            result = result && (noteType.Equals("STR") ||
                noteType.Equals("XST") ||
                noteType.Equals("BST"));
            return result;
        }

    }
}
