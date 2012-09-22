using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecognitionServer.Entities;

namespace RecognitionServer.Plugins
{
    class Media : RecognitionPlugin
    {
        new public static void onEnable(RecognitionServer.Main m) {
            m.AddCommand("Media", "random", "pick a song", 0, "Picks a song at random to play");
            m.AddCommand("Media", "song", "play song", 0, "Play a specific song");
            m.AddCommand("Media", "playlist", "play playlist", 0, "Play a specific playlist");
            m.AddCommand("Media", "shuffle", "shuffle all songs", 0, "Shuffles and plays all songs in library");
            m.AddCommand("Media", "shuffleplaylist", "shuffle playlist", 0, "Shuffles and plays a playlist");
        }

        new public static void onRecognise(RecognitionServer.Main m, Command comm)
        {

        }

    }
}
