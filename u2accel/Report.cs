using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u2accel
{
    class Report
    {
        string reportString = "";
        int counter = 1;

        public string ReportString
        {
            get
            {
                return reportString;
            }
        }

        public Report()
        {
        }

        public void AddSection(Range[] ranges)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("==================== Take " + counter + " ====================\r\n");
            for(int i=0; i<ranges.Length; i++)
            {
                builder.Append(ranges[i].ToString() + "\r\n");
            }
            counter++;

            reportString += builder.ToString();
        }
    }
}
