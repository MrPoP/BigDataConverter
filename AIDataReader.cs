using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDataConverter
{
    internal class AIException : Exception
    {
        public AIException() : base() { }
        public AIException(string? message) : base(message) { }
    }
    interface IReader 
    {
        void Start();
        void Dispose(bool disposing);
    }
    internal class AIDataReader : IReader
    {
        private string File;
        private Thread ReadProcess;
        private List<string> _Content;
        private bool _Disposed = false;
        private bool _ProcessWorking = false;
        private object _Lock = new object();
        public bool ProcessWorking { get { return _ProcessWorking; } }
        public AIDataReader(string file)
        {
            File = file;
            _Content = new List<string>();
            ReadProcess = new Thread(new ThreadStart(Read));
            ReadProcess.IsBackground = true;
        }
        public void Dispose(bool disposing)
        {
            _Disposed = disposing;
        }
        private void Read()
        {
            lock (this._Lock)
            {
                AIException? aIException = null;
                try
                {
                    if (this._Disposed)
                        return;
                    if (!String.IsNullOrEmpty(File))
                    {
                        long filesize = new FileInfo(File).Length;
                        long ReadedSize = 0;
                        int Lines = 0;
                        using (Stream stream = System.IO.File.OpenRead(File))
                        {
                            while (ReadedSize < filesize)
                            {
                                if (this._Disposed || ReadedSize == filesize)
                                    break;
                                string TextToAdd = "";
                                while(stream.CanRead)
                                {
                                    if (this._Disposed || ReadedSize == filesize)
                                        break;
                                    TextToAdd += (char)stream.ReadByte();
                                    ReadedSize++;
                                    if (TextToAdd.Contains("\n"))
                                        break;
                                }
                                _Content.Add(TextToAdd);
                                Lines++;
                            }
                        }
                    }
                }
                catch(Exception es)
                {
                    aIException = new AIException(es.Message);
                }
                finally
                {
                    if (aIException != null)
                        throw aIException;
                }
                _ProcessWorking = false;
            }
        }
        public void Start()
        {
            this._ProcessWorking = true;
            this.ReadProcess.Start();
        }
        private int CurrentIndex = 0;
        public string GetNextLine(out bool NewLine)
        {
            NewLine = false;
            if (_Content.Count > CurrentIndex)
            {
                NewLine = true;
                return this._Content[CurrentIndex++];
            }
            return "";
        }
    }
}
