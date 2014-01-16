using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Globalization;
using System.Reflection;

namespace RecognitionServer
{
    class Common
    {
        public static SpeechRecognitionEngine SetupSpeech(SpeechRecognitionEngine rec)
        {
            CultureInfo culture = new CultureInfo("en-GB");
            rec = new SpeechRecognitionEngine(culture);
            rec.SetInputToDefaultAudioDevice();
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            return rec;
        }
    }
}
