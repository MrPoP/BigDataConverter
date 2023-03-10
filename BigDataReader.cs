using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BigDataConverter
{
    internal class BigDataReader
    {
        int MaxLines = 1024;
        string FilePath;
        public BigDataReader(string File)
        {
            FilePath = File;
        }
        public IEnumerator<string> GetLines()
        {
            if (!String.IsNullOrEmpty(FilePath))
            {
                long filesize = new FileInfo(FilePath).Length;
                using (Stream stream = File.OpenRead(FilePath))
                {
                    for (long x = 0; x < MaxLines; x++)
                    {
                        long LefttotalLegth = filesize - (x * MaxLines);
                        long leghtcopy = Math.Min(MaxLines, LefttotalLegth);
                        byte[] chunkbyte = new byte[leghtcopy];
                        stream.Read(chunkbyte, 0, chunkbyte.Length);
                        string Readed = Encoding.UTF8.GetString(chunkbyte, 0, chunkbyte.Length);
                        foreach (string str in Readed.Split('\n'))
                        {
                            yield return str;
                        }
                    }
                }
            }
        }
    }
}
