using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MusicConverterTest
{
    /// <summary>
    /// Basic
    /// </summary>
    public abstract class Note : INote,IComparable
    {
        private string noteType;
        private string key;
        private string endKey;
        private int bar;
        private int startTime;
        private int waitTime;
        private int lastTime;
        private bool delayed;
        private double bpm;
        private Note prev;
        private Note next;

        public Note()
        {
            noteType = "";
            key = "";
            endKey = "";
            bar = 0;
            startTime = 0;
            lastTime = 0;
            bpm = 0;
        }

        /// <summary>
        /// Access NoteType
        /// </summary>
        public string NoteType
        {
            get
            {
                return this.noteType;
            }
            set
            {
                this.noteType = value;
            }
        }

        /// <summary>
        /// Access Key
        /// </summary>
        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = value;
            }
        }

        /// <summary>
        /// Access Bar
        /// </summary>
        public int Bar
        {
            get
            {
                return this.bar;
            }
            set
            {
                this.bar = value;
            }
        }

        /// <summary>
        /// Access StartTime
        /// </summary>
        public int StartTime
        {
            get
            {
                return this.startTime;
            }
            set
            {
                this.startTime = value;
            }
        }

        /// <summary>
        /// Acceess wait time
        /// </summary>
        public int WaitTime
        {
            get
            {
                return this.waitTime;
            }
            set
            {
                this.waitTime = value;
            }
        }

        /// <summary>
        /// Access EndTime
        /// </summary>
        public int LastTime
        {
            get
            {
                return this.lastTime;
            }
            set
            {
                this.lastTime = value;
            }
        }

        /// <summary>
        /// Access EndKey
        /// </summary>
        public string EndKey
        {
            get
            {
                return this.endKey;
            }
            set
            {
                this.endKey = value;
            }
        }

        /// <summary>
        /// Access Delayed
        /// </summary>
        public bool Delayed
        {
            get { return this.delayed; }
            set { this.delayed = value; }
        }

        /// <summary>
        /// Access BPM
        /// </summary>
        public double BPM
        {
            get { return this.bpm; }
            set { this.bpm = value; }
        }

        /// <summary>
        /// Access this.prev;
        /// </summary>
        public Note Prev
        {
            get { return this.prev; }
            set { this.prev = value; }
        }

        /// <summary>
        /// Access this.next
        /// </summary>
        public Note Next
        {
            get { return this.next; }
            set { this.next = value; }
        }
        public abstract string NoteSpecificGenre();

        /// <summary>
        /// Return this.noteGenre
        /// </summary>
        /// <returns>string of note genre</returns>
        public abstract string NoteGenre();

        /// <summary>
        /// Return if this is a true note
        /// </summary>
        /// <returns>True if is TAP,HOLD or SLIDE, false elsewise</returns>
        public abstract bool IsNote();
        public abstract bool CheckValidity();

        public abstract string Compose(int format);

        public int CompareTo(Object? obj)
        {
            int result = 0;
            Note another = obj as Note;
            //else if (this.NoteSpecificGenre().Equals("SLIDE")&&(this.NoteSpecificGenre().Equals("TAP")|| this.NoteSpecificGenre().Equals("HOLD")) && this.startTime == another.StartTime && this.bar == another.Bar)
            //{
            //    result = -1;
            //}
            //else if (this.NoteSpecificGenre().Equals("SLIDE_START") && (another.NoteSpecificGenre().Equals("TAP") || another.NoteSpecificGenre().Equals("HOLD")) && this.startTime == another.StartTime && this.bar == another.Bar)
            //{
            //    Console.WriteLine("STAR AND TAP");
            //    result = 1;
            //    Console.WriteLine(this.NoteSpecificGenre() + ".compareTo(" + another.NoteSpecificGenre() + ") is" + result);
            //    //Console.ReadKey();
            //}
            //if (this.Bar==another.Bar&&this.StartTime==another.StartTime)
            //{
            //    if (this.NoteGenre().Equals("BPM"))
            //    {
            //        result = -1;
            //    }
            //    else if (this.NoteGenre().Equals("MEASURE"))
            //    {
            //        result = 1;
            //    }
            //    else if ((this.NoteSpecificGenre().Equals("TAP")|| this.NoteSpecificGenre().Equals("HOLD"))&&another.NoteSpecificGenre().Equals("SLIDE_START"))
            //    {
            //        result= -1;
            //    }
            //}
            //else
            //{
            //    if (this.bar != another.Bar)
            //    {
            //        result = this.bar.CompareTo(another.Bar);
            //        //Console.WriteLine("this.compareTo(another) is" + result);
            //        //Console.ReadKey();
            //    }
            //    else result = this.startTime.CompareTo(another.StartTime);
            //}
            if (this.Bar!=another.Bar)
            {
                result = this.Bar.CompareTo(another.Bar);
            }
            else if (this.Bar==another.Bar&&(this.StartTime!=another.StartTime))
            {
                result = this.StartTime.CompareTo(another.StartTime);
            }
            else
            {
                if (this.NoteSpecificGenre().Equals("BPM"))
                {
                    result =-1;
                }
                //else if (this.NoteSpecificGenre().Equals("SLIDE")&&another.NoteSpecificGenre().Equals("SLIDE_START")&&this.Key.Equals(another.Key))
                //{
                //    result = 1;
                //}
                else result= 0;
            }
            return result;
        }
    }
}
