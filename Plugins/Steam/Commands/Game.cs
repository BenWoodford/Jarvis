using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JarvisSDK;
using System.Speech.Recognition;
using SteamWeb.SteamWeb;
namespace Steam.Commands
{
    class Game : IJarvisCommand
    {
        Steam plugin;
        Dictionary<string, int> gameCache = new Dictionary<string, int>();
        public Game(Steam plugin)
        {
            this.plugin = plugin;
        }
        public Grammar BuildGrammar()
        {
            plugin.GetServer().GetLogger().Info("Parsing Games List");
            Dictionary<string, SteamWeb.SteamWeb.Types.Game> gameList = plugin.GetSteamClient().GetGames();

            GrammarBuilder builder = new GrammarBuilder();
            builder.Append("play game");

            Choices gameChoices = new Choices();

            foreach (KeyValuePair<string, SteamWeb.SteamWeb.Types.Game> game in gameList)
            {
                gameChoices.Add(game.Value.Name);
                gameCache.Add(game.Value.Name, Int32.Parse(game.Key));
            }

            builder.Append(gameChoices);

            return new Grammar(builder);
        }

        public string RunCommand(SpeechRecognizedEventArgs e)
        {
            string gameName = e.Result.Text.Replace("play game ", "");
            if (gameCache.ContainsKey(gameName))
            {
                plugin.GetSteamClient().RunGame(gameCache[gameName]);
                return "";
            }
            else return "I'm afraid I don't know that game.";
        }
    }
}
