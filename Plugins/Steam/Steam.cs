using JarvisSDK;
using SteamWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Steam
{
    public class Steam : IJarvisPlugin
    {
        IRecognitionServer server;
        List<IJarvisCommand> commands = new List<IJarvisCommand>();
        SteamWebClient steamClient;
        public bool OnEnable(IRecognitionServer server)
        {
            this.server = server;
            steamClient = new SteamWebClient("Voice Recognition System", "irjgEfRGKftgj"); // Will setup a configuration system at some point.
            server.GetLogger().Info("Enabling Steam.dll Plugin");

            server.GetLogger().Info("Pinging for Authorisation with Steam Client");

            if (!steamClient.AuthoriseClient())
            {
                server.GetLogger().Warn("Please authorise this system to control your Steam Client.");
                server.GetLogger().Info("Another attempt will be made in 10 seconds. If this fails, Steam.dll will not be loaded.");

                Thread.Sleep(10000);

                if (!steamClient.AuthoriseClient())
                {
                    server.GetLogger().Severe("Failed to authorise with Steam Client. Reload plugin to re-attempt.");
                    return false;
                }
            }

            commands.Add(new Commands.Game(this));
            commands.Add(new Commands.Space(this));

            return true;
        }

        public void OnDisable()
        {
            Console.WriteLine("Disabling Steam.dll Plugin");
        }

        public List<IJarvisCommand> GetCommands()
        {
            return commands;
        }

        public IRecognitionServer GetServer()
        {
            return server;
        }

        public SteamWebClient GetSteamClient()
        {
            return steamClient;
        }
    }
}