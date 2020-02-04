using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u2accel
{
    class Range
    {
        private float a;
        private float b;

        private int aFrame;
        private int bFrame;

        private float modifier;
        private bool isKms;

        private int tickLength;

        private float lastSpeed;

        public float LowerBorder
        {
            get
            {
                return a;
            }
        }

        public float UpperBorder
        {
            get
            {
                return b;
            }
        }

        DateTime tickStart = DateTime.MinValue;
        DateTime tickEnd = DateTime.MinValue;

        public float Time
        {
            get
            {
                if (tickStart == DateTime.MinValue || tickEnd == DateTime.MinValue)
                    return 0.0f;
                return (float)(tickEnd - tickStart).TotalSeconds;//(bFrame - aFrame) / (1000.0f / tickLength);
            }
        }

        public int EnterFrame
        {
            get
            {
                return aFrame;
            }
            private set
            {
                if(aFrame == 0)
                    aFrame = value;
            }
        }

        public int LeaveFrame
        {
            get
            {
                return bFrame;
            }
            private set
            {
                if(bFrame == 0)
                    bFrame = value;
            }
        }


        public override string ToString()
        {
            if(isKms)
                return a.ToString("F0") + " km/h - " + b.ToString("F0") + " km/h in " + Time.ToString("F2") + " s";

            return (a/modifier).ToString("F0") + " mph - " + (b/modifier).ToString("F0") + " mph in " + Time.ToString("F2")  + " s";
        }

        public static void SaveRanges(Range[] ranges, string path)
        {
            StreamWriter sr = new StreamWriter(path);
            BinaryWriter writer = new BinaryWriter(sr.BaseStream);
            writer.Write(ranges.Length);
            for(int i=0; i<ranges.Length; i++)
            {
                writer.Write(ranges[i].a);
                writer.Write(ranges[i].b);
            }
            sr.Close();
        }

        public static Range[] LoadRanges(string path, int tickLength, bool isKmh)
        {
            if (!File.Exists(path))
                return new Range[0];
            StreamReader sr = new StreamReader(path);
            BinaryReader reader = new BinaryReader(sr.BaseStream);
            int rangesCount = reader.ReadInt32();
            Range[] result = new Range[rangesCount];
            for(int i=0; i<rangesCount; i++)
            {
                result[i] = new Range(reader.ReadSingle(), reader.ReadSingle(), tickLength, isKmh);
            }
            sr.Close();
            return result;
        }

        public Range(float lower, float upper, int tickLength, bool kmh)
        {
            modifier = kmh ? 1.0f : 1.61f;
            isKms = kmh;
            a = lower;
            b = upper;
            this.tickLength = tickLength;
            Reset();
        }

        public void Think(float speed, int tickNumber)
        {
            if((lastSpeed <= a) && (speed >= a) && (tickStart == DateTime.MinValue))
            {
                aFrame = tickNumber;
                tickStart = DateTime.Now;
            }
            if (lastSpeed <= b && speed >= b && (tickEnd == DateTime.MinValue))
            {
                LeaveFrame = tickNumber;
                tickEnd = DateTime.Now;
            }
            lastSpeed = speed;
        }

        public void Reset()
        {
            aFrame = bFrame = 0;
            lastSpeed = 0.0f;
            tickStart = DateTime.MinValue;
            tickEnd = DateTime.MinValue;
        }
    }
}
