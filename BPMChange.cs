using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicConverterTest
{
    /// <summary>
    /// BPMChange note for Simai
    /// </summary>
    public class BPMChange:Note
    {

        /// <summary>
        /// Construct Empty
        /// </summary>
        public BPMChange()
        {
            this.NoteType = "BPM";
            this.Key = "";
            this.Bar = 0;
            this.StartTime = 0;
            this.BPM = 0;
        }

        /// <summary>
        /// Construct BPMChange with given bar, tick, BPM
        /// </summary>
        /// <param name="bar">Bar</param>
        /// <param name="startTime">startTime</param>
        /// <param name="BPM">BPM</param>
        public BPMChange(int bar, int startTime, double BPM)
        {
            this.Bar = bar;
            this.StartTime = startTime;
            this.BPM = BPM;
        }

        public BPMChange(BPMChange takein)
        {
            this.Bar = takein.Bar;
            this.StartTime = takein.StartTime;
            this.BPM = takein.BPM;
        }

        //public int Bar
        //{
        //    get { return this.Bar; }
        //    set { this.Bar = value; }
        //}

        public int Tick 
        { 
            get 
            { return this.StartTime; }
            set 
            { this.StartTime = value; }
        }

        //public int StartTime
        //{
        //    get
        //    { return this.StartTime; }
        //    set
        //    {
        //        this.StartTime = value;
        //    }
        //}

        public override bool CheckValidity()
        {
            throw new NotImplementedException();
        }

        public override string Compose(int format)
        {
            string result = "";
            if (format==0)
            {
                result += "(" + this.BPM + ")";
                //result += "(" + this.BPM + "_" + this.Bar + "_" + this.StartTime + ")";
            }
            else result += "(" + this.BPM + "_" + this.Bar + "_" + this.StartTime + ")";
                return result;
        }

        public override string NoteGenre()
        {
            return "BPM";
        }

        public override bool IsNote()
        {
            return true;
        }

        public override string NoteSpecificGenre()
        {
            return "BPM";
        }
    }
}
