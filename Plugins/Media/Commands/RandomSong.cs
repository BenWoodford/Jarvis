using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JarvisSDK;
using System.Speech.Recognition;

namespace Media.Commands
{
    class RandomSong : IJarvisCommand
    {
        Media plugin;
        public RandomSong(Media plugin)
        {
            this.plugin = plugin;
        }
        public Grammar BuildGrammar()
        {
            GrammarBuilder builder = new GrammarBuilder();
            builder.Append(new Choices("pick a song", "play a random song", "play random song"));
            return new Grammar(builder);
        }


        public string RunCommand(SpeechRecognizedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}