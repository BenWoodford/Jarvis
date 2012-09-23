using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Globalization;
using System.Reflection;
using RecognitionServer.Entities;

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

        public static Boolean InvokePlugin(Main m, String className, String methodName, Command comm = null)
        {
            Type t = Type.GetType("RecognitionServer.Plugins." + className + ",RecognitionServer");

            if (t == null)
            {
                m.GetLogger().Severe("Invalid class name! (" + className + "." + methodName + " provided with \"" + comm.Text + "\")");
                return false;
            }

            object[] args = new object[] { m };

            if(comm != null)
                args[1] = comm;

            Boolean b = false;

            t.GetMethod(methodName).Invoke(t, args);

//            try
//            {
//            }
/*            catch (Exception e)
            {
                m.GetLogger().Severe(e.Message);
                return false;
            }*/

            return b;
        }
    }
}
