using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JarvisSDK;
using System.Speech.Recognition;

namespace Steam.Commands
{
    class Space : IJarvisCommand
    {
        Steam plugin;
        public Space(Steam plugin)
        {
            this.plugin = plugin;
        }
        public Grammar BuildGrammar()
        {
            Choices choiceBuilder = new Choices("open web browser", "open my game library", "open game library", "open my friends", "open messaging");
            return new Grammar(new GrammarBuilder(choiceBuilder));
        }

        public string RunCommand(SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text.Replace("open ", ""))
            {
                case "my game library":
                case "game library":
                    plugin.GetSteamClient().ChangeSpace(Spaces.LIBRARY);
                    break;

                case "web browser":
                    plugin.GetSteamClient().ChangeSpace(Spaces.WEBBROWSER);
                    break;

                case "my friends":
                case "messaging":
                    plugin.GetSteamClient().ChangeSpace(Spaces.FRIENDS);
                    break;

                default:
                    return "I don't know that space";
                    break;
            }

            return "";
        }
    }
}
