using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace JarvisSDK
{
    public interface IRecognitionServer
    {
        void Init();
        void Recognised(object sender, SpeechRecognizedEventArgs e);
        void InitialRecognise(object sender, SpeechRecognizedEventArgs e);
        ILogger GetLogger();
        SpeechRecognitionEngine GetRecognitionEngine();
    }
}
