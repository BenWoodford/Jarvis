using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Globalization;
using System.Reflection;
using JarvisSDK;
using System.IO;

namespace RecognitionServer
{
    class Main : IRecognitionServer
    {
        public SpeechRecognitionEngine rec;
        public SpeechRecognitionEngine listenrec;

        public Logger logger = new Logger();
        public Choices choices = new Choices();
        public Dictionary<string, IJarvisCommand> commands = new Dictionary<string, IJarvisCommand>();
        public Speaker speech = new Speaker();
        public Boolean listening = false;
        public List<IJarvisPlugin> plugins = new List<IJarvisPlugin>();

        public Main()
        {
            this.rec = Common.SetupSpeech(this.rec);
            this.listenrec = Common.SetupSpeech(this.listenrec);
            this.listenrec.SpeechRecognized += this.InitialRecognise;
            this.rec.InitialSilenceTimeout = TimeSpan.FromSeconds(5);
            this.rec.SpeechRecognized += this.Recognised;
        }

        public void Recognised(object sender, SpeechRecognizedEventArgs e)
        {
            this.listening = false;
            this.GetLogger().Info("Recognised text: " + e.Result.Text);
            if (commands.ContainsKey(e.Result.Grammar.Name))
            {
                String s = commands[e.Result.Grammar.Name].RunCommand(e);
                if (s != null && s.Length > 0)
                {
                    speech.Say(s);
                }
            }
        }

        // Branch the program, let's make it play random noises.
        public void Init()
        {
            this.GetLogger().Info("Initial Silence Timeout: " + this.rec.InitialSilenceTimeout);
            this.GetLogger().Info("Babble Timeout: " + this.rec.BabbleTimeout);
            this.GetLogger().Info("End Silence Timeout: " + this.rec.EndSilenceTimeout);
            this.GetLogger().Info("End Ambiguous Silence Timeout: " + this.rec.EndSilenceTimeoutAmbiguous);
            this.GetLogger().Info("Voice Volume: " + this.speech.synth.Volume);

            if (Directory.Exists("Plugins"))
            {
                foreach (String fileName in Directory.GetFiles("Plugins", "*.dll"))
                {
                    Assembly asm = Assembly.LoadFrom(fileName);
                    try {
                        Type[] types = asm.GetTypes();
                    }
                    catch (ReflectionTypeLoadException e)
                    {
                        GetLogger().Severe(e.Message);
                    }

                    foreach (Type t in asm.GetTypes())
                    {
                        if(t.IsInterface)
                            continue;
                        if (t.GetInterface("IJarvisPlugin") != null)
                            plugins.Add((IJarvisPlugin)Activator.CreateInstance(t));
                    }
                }
            }
            else
            {
                Directory.CreateDirectory("Plugins");
            }

            foreach(IJarvisPlugin plugin in plugins) {
                RegisterPlugin(plugin);
            }

            this.listenrec.LoadGrammar(new Grammar(new GrammarBuilder(new Choices("Jarvis"))));

            while(true)
                this.listenrec.Recognize();
        }

        public void RegisterPlugin(IJarvisPlugin plugin)
        {
            if (plugin.OnEnable(this))
            {
                foreach (IJarvisCommand command in plugin.GetCommands())
                {
                    Grammar gram = command.BuildGrammar();
                    if (gram != null)
                    {
                        gram.Name = command.GetType().Namespace + "." + command.GetType().Name;
                        this.rec.LoadGrammar(gram);
                        commands.Add(gram.Name, command);
                    }
                }
            }
        }

        public void InitialRecognise(object sender, SpeechRecognizedEventArgs e)
        {
            this.listening = true;
            this.GetLogger().Info("Listening.");
            this.rec.Recognize();

            if (this.listening)
                this.GetLogger().Info("Never mind.");
        }

        public Logger GetLogger()
        {
            return this.logger;
        }

        public void AddCommand(String plugin, String identifier, GrammarBuilder builder, String text, Int32 priority = 0, String description = "")
        {
            
        }

        ILogger IRecognitionServer.GetLogger()
        {
            return logger;
        }

        public SpeechRecognitionEngine GetRecognitionEngine()
        {
            return rec;
        }
    }
}
