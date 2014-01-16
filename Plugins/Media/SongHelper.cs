using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System.Speech.Recognition;

namespace Media
{
    class SongHelper
    {
        public static SongCollection songs;
        public static MediaLibrary library;
        public static PlaylistCollection playlists;
        public static Song currentSong;

        public static void Shuffle(bool toggle)
        {
            FrameworkDispatcher.Update();
            MediaPlayer.IsShuffled = toggle;
        }

        public static bool PlayRandom()
        {
            if (SongHelper.songs == null)
                SongHelper.GetSongs();

            if(songs.Count == 0)
                return false;

            Random rand = new Random();

            SongHelper.Play(SongHelper.songs, (Int32)rand.Next(songs.Count - 1));
            return true;
        }

        public static bool PlaySong(String name)
        {
            if (SongHelper.songs == null)
                SongHelper.GetSongs();

            foreach (Song s in SongHelper.songs)
            {
                if (s.Name.ToString().Equals(name))
                {
                    SongHelper.Play(s);
                    return true;
                }
            }
            return false;
        }

        public static bool PlayPlaylist(String name)
        {
            if (SongHelper.songs == null)
                SongHelper.GetSongs();

            foreach (Playlist p in SongHelper.playlists)
            {
                if (p.Name.ToString().Equals(name))
                {
                    SongHelper.Playlist(p);
                    return true;
                }
            }

            return false;
        }

        public static void Playlist(Playlist p)
        {
            FrameworkDispatcher.Update();
            MediaPlayer.Play(p.Songs);
        }

        public static void Play(Song s)
        {
            FrameworkDispatcher.Update();
            MediaPlayer.Play(s);

            SongHelper.currentSong = s;
        }

        public static void Play(SongCollection s)
        {
            FrameworkDispatcher.Update();
            MediaPlayer.Play(s);

            SongHelper.currentSong = s[0];
        }

        public static void Play(SongCollection s, Int32 i)
        {
            FrameworkDispatcher.Update();
            MediaPlayer.Play(s, i);

            SongHelper.currentSong = s[i + 1];
        }

        public static void NextSong()
        {
            FrameworkDispatcher.Update();
            MediaPlayer.MoveNext();
        }

        public static void PreviousSong()
        {
            FrameworkDispatcher.Update();
            MediaPlayer.MovePrevious();
        }

        public static void Stop()
        {
            FrameworkDispatcher.Update();
            MediaPlayer.Stop();
        }

        public static void Pause()
        {
            FrameworkDispatcher.Update();
            MediaPlayer.Pause();
        }

        public static void Resume()
        {
            FrameworkDispatcher.Update();
            MediaPlayer.Resume();
        }

        public static Song CurrentlyPlaying()
        {
            return currentSong;
        }

        public static Choices GenerateSongChoices()
        {
            if (SongHelper.songs == null)
                SongHelper.GetSongs();

            Choices choices = new Choices();

            foreach (Song s in SongHelper.songs)
            {
                choices.Add(s.Name);
            }

            return choices;
        }

        public static Choices GeneratePlaylistChoices()
        {
            if (SongHelper.songs == null)
                SongHelper.GetSongs();

            Choices choices = new Choices();

            foreach (Playlist p in SongHelper.playlists)
            {
                choices.Add(p.Name);
            }

            return choices;
        }

        public static SongCollection GetSongs()
        {
            using (SongHelper.library = new MediaLibrary())
            {
                SongHelper.songs = library.Songs;
                SongHelper.playlists = library.Playlists;
                return songs;
            }
        }

        public static int SongCount()
        {
            if (SongHelper.songs == null)
                SongHelper.GetSongs();

            return songs.Count;
        }

        public static int PlaylistCount()
        {
            if (SongHelper.playlists == null)
                SongHelper.GetSongs();

            return playlists.Count;
        }
    }
}
