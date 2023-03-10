using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDataConverter
{
    interface IAnalyzer
    {
        string Feed(string line);
    }
    internal class ResultAnalyzer : IAnalyzer
    {
        private char _splitter;
        public ResultAnalyzer(char splitter)
        {
            _splitter = splitter;
        }
        public string Feed(string line)
        {
            string ReturnValue = "";
            if (line.Contains(_splitter))
            {
                string[] values = line.Split(_splitter, StringSplitOptions.TrimEntries);
                for(int x = 0; x <values.Length; x++)
                {
                    if(x == 9)
                    {
                        string val = values[x] + ":";
                        x++;
                        val += values[x] + ":";
                        x++;
                        val += values[x];
                        ReturnValue += $"\'{val}\'";
                    }
                    else
                        ReturnValue += $"\'{values[x]}\'";
                    if (x < values.Length - 1)
                        ReturnValue += ",";
                }
            }
            return ReturnValue;
        }
    }
}
