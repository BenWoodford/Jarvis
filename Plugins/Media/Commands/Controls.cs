using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JarvisSDK;
using System.Speech.Recognition;

namespace Media.Commands
{
    class Controls : IJarvisCommand
    {
        Media plugin;
        public Controls(Media plugin)
        {
            this.plugin = plugin;
        }
        public Grammar BuildGrammar()
        {
            Choices choiceBuilder = new Choices();

            // Next
            GrammarBuilder nextBuilder = new GrammarBuilder();
            nextBuilder.Append(new Choices("next song", "play the next song", "skip this song", "play next song"));
            choiceBuilder.Add(nextBuilder);

            // Previous
            GrammarBuilder prevBuilder = new GrammarBuilder();
            prevBuilder.Append(new Choices("last song", "previous song", "play the last song", "play the previous song"));
            choiceBuilder.Add(prevBuilder);

            // Pause
            GrammarBuilder pauseBuilder = new GrammarBuilder();
            pauseBuilder.Append(new Choices("pause song", "pause this song", "pause song playback"));
            choiceBuilder.Add(pauseBuilder);

            // Stop
            GrammarBuilder stopBuilder = new GrammarBuilder();
            stopBuilder.Append(new Choices("stop song", "stop song playback", "stop the music"));
            choiceBuilder.Add(stopBuilder);

            // Resume
            GrammarBuilder resumeBuilder = new GrammarBuilder();
            resumeBuilder.Append(new Choices("resume playback", "resume song", "resume playing"));
            choiceBuilder.Add(resumeBuilder);

            return new Grammar(new GrammarBuilder(choiceBuilder));
        }

        public string RunCommand(SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text)
            {
                case "next song":
                case "play the next song":
                case "skip this song":
                case "play next song":
                    SongHelper.NextSong();
                    break;

                case "last song":
                case "previous song":
                case "play the last song":
                case "play the previous song":
                    SongHelper.PreviousSong();
                    break;

                case "pause song":
                case "pause this song":
                case "pause song playback":
                    SongHelper.Pause();
                    break;

                case "stop song":
                case "stop song playback":
                case "stop the music":
                    SongHelper.Stop();
                    break;

                case "resume playback":
                case "resume song":
                case "resume playing":
                    SongHelper.Resume();
                    break;
            }
            return "";
        }
    }
}
