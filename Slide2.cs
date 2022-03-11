using System.Collections.Generic;

namespace MaichartConverter
{
    public class Slide2 : Note
    {
        Tap? startNote;
        List<Slide> slideNoteLine;
        public Slide2()
        {
            this.slideNoteLine=new List<Slide>();
        }

        public Slide2(string noteType, int bar, int startTime, string key, int waitTime, int lastTime, string endKey)
        {
            this.NoteType = noteType;
            this.Key = key;
            this.Bar = bar;
            this.Tick = startTime;
            this.WaitTime = waitTime;
            this.LastTime = lastTime;
            this.EndKey = endKey;
            this.Delayed = this.WaitTime != 96;
            this.slideNoteLine=new List<Slide>();
        }

        public override string NoteSpecificType => "SLIDE";

        public override string NoteGenre => "SLIDE";

        public override bool IsNote => true;

        public override bool CheckValidity()
        {
            throw new NotImplementedException();
        }

        public override string Compose(int format)
        {
            throw new NotImplementedException();
        }
    }
}