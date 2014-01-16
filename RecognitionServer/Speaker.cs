using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;

namespace RecognitionServer
{
    class Speaker
    {
        public SpeechSynthesizer synth;

        public Speaker()
        {
            synth = new SpeechSynthesizer();
            synth.Volume = 100;
        }

        public void Say(String s, Boolean async = true)
        {
            if (async)
                this.synth.SpeakAsync(s);
            else
                this.synth.Speak(s);
        }
    }
}
