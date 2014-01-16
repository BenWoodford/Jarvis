using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JarvisSDK;
using System.Speech.Recognition;
using Microsoft.Xna.Framework.Media;

namespace Media
{
    public class Media : IJarvisPlugin
    {
        IRecognitionServer server;
        List<IJarvisCommand> commands = new List<IJarvisCommand>();
        public bool OnEnable(IRecognitionServer server)
        {
            this.server = server;
            server.GetLogger().Info("Enabling Media.dll Plugin");
            server.GetLogger().Info("Updating Song Library");

            commands.Add(new Commands.Play(this));
            commands.Add(new Commands.Controls(this));
            commands.Add(new Commands.RandomSong(this));

            return true;
        }

        public void OnDisable()
        {
            Console.WriteLine("Disabling Media.dll Plugin");
        }

        public List<IJarvisCommand> GetCommands()
        {
            return commands;
        }

        public IRecognitionServer GetServer()
        {
            return server;
        }
    }
}
