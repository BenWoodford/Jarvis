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
    class Media : RecognitionPlugin
    {
        MediaLibrary library;
        SongCollection songs;
        PlaylistCollection playlists;
        String currentSong = "";

        RecognitionServer.Main main;

        public override void onEnable(RecognitionServer.Main m) {
            this.main = m;

            this.GetSongs();

            GrammarBuilder tmpBuilder = null;

            tmpBuilder = new GrammarBuilder();
            tmpBuilder.Append("pick a song");
            m.AddCommand("Media", "random", tmpBuilder, "pick a song", 0, "Picks a song at random to play");

            tmpBuilder = new GrammarBuilder();
            tmpBuilder.Append("play song");
            tmpBuilder.Append(this.GenerateSongChoices());
            m.AddCommand("Media", "song", tmpBuilder, "play song", 0, "Play a specific song");

            tmpBuilder = new GrammarBuilder();
            tmpBuilder.Append("play playlist");
            tmpBuilder.Append(this.GeneratePlaylistChoices());
            m.AddCommand("Media", "playlist", tmpBuilder, "play playlist", 0, "Play a specific playlist");

            tmpBuilder = new GrammarBuilder();
            tmpBuilder.Append("shuffle all songs");
            m.AddCommand("Media", "shuffle", tmpBuilder, "shuffle all songs", 0, "Shuffles and plays all songs in library");

            tmpBuilder = new GrammarBuilder();
            tmpBuilder.Append("shuffle playlist");
            tmpBuilder.Append(this.GeneratePlaylistChoices());
            m.AddCommand("Media", "shuffleplaylist", tmpBuilder, "shuffle playlist", 0, "Shuffles and plays a playlist");

            tmpBuilder = new GrammarBuilder();
            tmpBuilder.Append(new Choices("next song", "play the next song", "skip this song", "play next song"));
            m.AddCommand("Media", "nextsong", tmpBuilder, "next song", 0, "Plays the next song");
            m.AddCommand("Media", "nextsong", tmpBuilder, "play the next song", 0, "Plays the next song");
            m.AddCommand("Media", "nextsong", tmpBuilder, "skip this song", 0, "Plays the next song");
            m.AddCommand("Media", "nextsong", tmpBuilder, "next song", 0, "Plays the next song");

            tmpBuilder = new GrammarBuilder();
            tmpBuilder.Append(new Choices("pause song", "pause this song", "pause this", "pause playback", "pause song playback"));
            m.AddCommand("Media", "pause", tmpBuilder, "pause this song", 0, "Pauses the currently playing song");
            m.AddCommand("Media", "pause", tmpBuilder, "pause this", 0, "Pauses the currently playing song");
            m.AddCommand("Media", "pause", tmpBuilder, "pause song", 0, "Pauses the currently playing song");
            m.AddCommand("Media", "pause", tmpBuilder, "pause playback", 0, "Pauses the currently playing song");
            m.AddCommand("Media", "pause", tmpBuilder, "pause song playback", 0, "Pauses the currently playing song");

            tmpBuilder = new GrammarBuilder();
            tmpBuilder.Append(new Choices("stop this song", "stop playing", "stop song", "stop this", "stop playback", "stop song playback"));
            m.AddCommand("Media", "stop", tmpBuilder, "stop this song", 0, "Stops the currently playing song");
            m.AddCommand("Media", "stop", tmpBuilder, "stop playing", 0, "Stops the currently playing song");
            m.AddCommand("Media", "stop", tmpBuilder, "stop song", 0, "Stops the currently playing song");
            m.AddCommand("Media", "stop", tmpBuilder, "stop this", 0, "Stops the currently playing song");
            m.AddCommand("Media", "stop", tmpBuilder, "stop playback", 0, "Stops the currently playing song");
            m.AddCommand("Media", "stop", tmpBuilder, "stop song playback", 0, "Stops the currently playing song");

            tmpBuilder = new GrammarBuilder();
            tmpBuilder.Append(new Choices("resume song", "resume playing", "resume playback", "resume song playback"));
            m.AddCommand("Media", "resume", tmpBuilder, "resume song", 0, "Resumes the currently paused song");
            m.AddCommand("Media", "resume", tmpBuilder, "resume playing", 0, "Resumes the currently paused song");
            m.AddCommand("Media", "resume", tmpBuilder, "resume playback", 0, "Resumes the currently paused song");
            m.AddCommand("Media", "resume", tmpBuilder, "resume song playback", 0, "Resumes the currently paused song");

            tmpBuilder = new GrammarBuilder();
            tmpBuilder.Append(new Choices("what is this song", "what song is this", "what is playing", "what song is playing"));
            m.AddCommand("Media", "what", tmpBuilder, "what is this song", 0, "Tells you what the current song is");
            m.AddCommand("Media", "what", tmpBuilder, "what song is this", 0, "Tells you what the current song is");
            m.AddCommand("Media", "what", tmpBuilder, "what is playing", 0, "Tells you what the current song is");
            m.AddCommand("Media", "what", tmpBuilder, "what song is playing", 0, "Tells you what the current song is");
        }

        public override void onRecognise(Command comm, String text = "")
        {
            switch (comm.Identifier)
            {
                case "random":
                    this.Shuffle(false);
                    this.PlayRandom();
                    break;

                case "song":
                    this.Shuffle(false);
                    this.PlaySong(text.Replace("play song ", ""));
                    break;

                case "playlist":
                    this.Shuffle(false);
                    this.PlayPlaylist(text.Replace("play playlist ", ""));
                    break;

                case "shuffle":
                    this.Shuffle(true);
                    this.PlayRandom();
                    break;

                case "shuffleplaylist":
                    this.Shuffle(true);
                    this.PlayPlaylist(text.Replace("shuffle playlist ", ""));
                    break;

                case "nextsong":
                    this.NextSong();
                    break;

                case "stop":
                    this.Stop();
                    break;

                case "pause":
                    this.Pause();
                    break;

                case "resume":
                    this.Resume();
                    break;

                case "what":
                    this.CurrentlyPlaying();
                    break;
            }
        }

        public void Shuffle(Boolean toggle)
        {
            FrameworkDispatcher.Update();
            MediaPlayer.IsShuffled = toggle;
        }

        public Choices GenerateSongChoices()
        {
            if (this.songs == null)
                this.GetSongs();

            Choices choices = new Choices();

            foreach (Song s in this.songs)
            {
                choices.Add(s.Name);
            }

            return choices;
        }

        public Choices GeneratePlaylistChoices()
        {
            if (this.songs == null)
                this.GetSongs();

            Choices choices = new Choices();

            foreach (Playlist p in this.playlists)
            {
                choices.Add(p.Name);
            }

            return choices;
        }

        public SongCollection GetSongs()
        {
            using (this.library = new MediaLibrary())
            {
                this.songs = library.Songs;
                this.playlists = library.Playlists;
                return songs;
            }
        }

        public void PlayRandom()
        {
            if (this.songs == null)
                this.GetSongs();

            Random rand = new Random();

            this.Play(this.songs, (Int32)rand.Next(songs.Count - 1));
        }

        public void PlaySong(String name)
        {
            if (this.songs == null)
                this.GetSongs();

            foreach (Song s in this.songs)
            {
                if (s.Name.ToString().Equals(name))
                {
                    this.Play(s);
                    break;
                }
            }
        }

        public void PlayPlaylist(String name)
        {
            if (this.songs == null)
                this.GetSongs();

            foreach (Playlist p in this.playlists)
            {
                if (p.Name.ToString().Equals(name))
                {
                    this.Playlist(p);
                    break;
                }
            }
        }

        public void Playlist(Playlist p)
        {
            FrameworkDispatcher.Update();
            MediaPlayer.Play(p.Songs);
        }

        public void Play(Song s)
        {
            FrameworkDispatcher.Update();
            MediaPlayer.Play(s);

            this.currentSong = s.Name;
        }

        public void Play(SongCollection s)
        {
            FrameworkDispatcher.Update();
            MediaPlayer.Play(s);

            this.currentSong = s[0].Name;
        }

        public void Play(SongCollection s, Int32 i)
        {
            FrameworkDispatcher.Update();
            MediaPlayer.Play(s, i);

            this.currentSong = s[i].Name;
        }

        public void NextSong()
        {
            FrameworkDispatcher.Update();
            MediaPlayer.MoveNext();
        }

        public void Stop()
        {
            FrameworkDispatcher.Update();
            MediaPlayer.Stop();
        }

        public void Pause()
        {
            FrameworkDispatcher.Update();
            MediaPlayer.Pause();
        }

        public void Resume()
        {
            FrameworkDispatcher.Update();
            MediaPlayer.Resume();
        }

        public void CurrentlyPlaying()
        {
            MediaPlayer.Volume = (float)0.1;
            this.main.speech.Say("The current song is " + this.currentSong, false);
            MediaPlayer.Volume = (float)1;
        }

    }
}
