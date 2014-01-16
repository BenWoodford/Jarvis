using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisSDK
{
    public interface ILogger
    {
        void Print(String s, String level = "Server");
        void Log(string logMessage, StreamWriter w);
        void WriteToFile(String s, String file);
        void Severe(String s);
        void Info(String s);
        void Warn(String s);
    }
}
