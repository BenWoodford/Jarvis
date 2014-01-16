using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace JarvisSDK
{
    public interface IJarvisCommand
    {
        Grammar BuildGrammar();
        String RunCommand(SpeechRecognizedEventArgs e);
    }
}
