using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JarvisSDK;
using System.Speech.Recognition;

namespace Media.Commands
{
    class Play : IJarvisCommand
    {
        Media plugin;
        public Play(Media plugin)
        {
            this.plugin = plugin;
        }

        public Grammar BuildGrammar()
        {
            Choices choiceBuilder = new Choices();
            
            // Songs
            if (SongHelper.SongCount() > 0) // it freaks out if there's nothing in the one-of bit.
            {
                GrammarBuilder songBuilder = new GrammarBuilder();
                songBuilder.Append("play song");
                songBuilder.Append(SongHelper.GenerateSongChoices());
                choiceBuilder.Add(songBuilder);
            }

            GrammarBuilder shuffleBuilder = new GrammarBuilder();
            shuffleBuilder.Append("shuffle all songs");
            choiceBuilder.Add(shuffleBuilder);


            // Playlists
            if (SongHelper.PlaylistCount() > 0)
            {
                GrammarBuilder playListBuilder = new GrammarBuilder();
                playListBuilder.Append("play playlist");
                playListBuilder.Append(SongHelper.GeneratePlaylistChoices());
                choiceBuilder.Add(playListBuilder);

                GrammarBuilder shufflePlayListBuilder = new GrammarBuilder();
                shufflePlayListBuilder.Append("shuffle playlist");
                shufflePlayListBuilder.Append(SongHelper.GeneratePlaylistChoices());
                choiceBuilder.Add(shufflePlayListBuilder);
            }

            Grammar gram = new Grammar(new GrammarBuilder(choiceBuilder));

            return gram;
        }


        public string RunCommand(SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.StartsWith("play song"))
            {
                SongHelper.Shuffle(false);
                if (!SongHelper.PlaySong(e.Result.Text.Replace("play song ", "")))
                    return "I don't know that song.";
            }
            else if (e.Result.Text.StartsWith("play playlist"))
            {
                SongHelper.Shuffle(false);
                if (SongHelper.PlayPlaylist(e.Result.Text.Replace("play playlist ", "")))
                    return "I don't have that playlist in my library.";
            }
            else if (e.Result.Text.StartsWith("shuffle playlist"))
            {
                SongHelper.Shuffle(true);
                if (!SongHelper.PlayPlaylist(e.Result.Text.Replace("shuffle playlist ", "")))
                    return "I don't have that playlist in my library";
            }
            else if (e.Result.Text.StartsWith("shuffle all songs"))
            {
                SongHelper.Shuffle(true);
                if (!SongHelper.PlayRandom())
                    return "I don't have any songs to shuffle...";
            }

            return "";
        }
    }
}
