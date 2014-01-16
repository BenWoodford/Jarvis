using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace JarvisSDK
{
    public interface IJarvisPlugin
    {
        bool OnEnable(IRecognitionServer server);
        void OnDisable();
        List<IJarvisCommand> GetCommands();
    }
}
