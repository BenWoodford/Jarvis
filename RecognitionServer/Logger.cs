using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RecognitionServer
{
    class Logger
    {
        Boolean _ToFile = false;

        public Boolean ToFile
        {
            get { return this._ToFile; }
            set { this._ToFile = value; }
        }

        public void Print(String s, String level = "Server")
        {
            if (this._ToFile)
            {
                this.WriteToFile(s, level + ".log");
            }
            else
            {
                Console.WriteLine("[" + level + "] " + s);
            }
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine("{0} {1}: {2}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString(), logMessage);
        }

        public void WriteToFile(String s, String file)
        {
            using (StreamWriter w = File.AppendText("logs\\" + file))
            {
                Log(s, w);
            }
        }

        public void Severe(String s)
        {
            this.Print(s, "SEVERE");
        }
    }
}
