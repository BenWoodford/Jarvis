using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JarvisSDK;
using System.Speech.Recognition;

namespace Media.Commands
{
    class SongInfo : IJarvisCommand
    {
        public Grammar BuildGrammar()
        {
            Choices choiceBuilder = new Choices();

            // What Song
            GrammarBuilder whatBuilder = new GrammarBuilder();
            whatBuilder.Append(new Choices("what song is this", "what is this song", "what song is playing"));
            choiceBuilder.Add(whatBuilder);

            // Who
            GrammarBuilder whoBuilder = new GrammarBuilder();
            whoBuilder.Append(new Choices("who is this song by", "who sings this"));
            choiceBuilder.Add(whoBuilder);

            return new Grammar(new GrammarBuilder(choiceBuilder));
            
        }

        public string RunCommand(SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text)
            {
                case "what song is this":
                case "what is this song":
                case "what song is playing":
                    return SongHelper.currentSong.Name + " by " + SongHelper.currentSong.Artist.Name;

                case "who is this song by":
                case "who sings this":
                    return SongHelper.currentSong.Artist.Name;
            }

            return "";
        }
    }
}
