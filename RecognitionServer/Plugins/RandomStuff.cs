using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecognitionServer.Entities;
using System.Speech.Recognition;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;

namespace RecognitionServer.Plugins
{
    class RandomStuff : RecognitionPlugin
    {
        RecognitionServer.Main main;

        public override void onEnable(RecognitionServer.Main m)
        {
            this.main = m;

            GrammarBuilder tmpBuilder = new GrammarBuilder();
            tmpBuilder.Append(new Choices("you cray cray"));
            m.AddCommand("Random", "crazy", tmpBuilder, "you cray cray", 0, "Crazy.");
        }

        public override void onRecognise(Command comm, String text = "")
        {
            switch (comm.Identifier)
            {
                case "crazy":
                    this.main.speech.Say("Yes, yes I am.");
                    break;
                case "shutdown":
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
