using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Globalization;
using System.Reflection;
using RecognitionServer.Entities;
using RecognitionServer.Plugins;

namespace RecognitionServer
{
    class Main
    {
        public SpeechRecognitionEngine rec;
        public Choices choices;
        public GrammarBuilder grammarbuilder;
        public Grammar grammar;
        public Boolean stay;
        public Logger logger;
        public List<Command> commands;
        public Speaker speech;

        public Dictionary<String, RecognitionPlugin> plugins;

        public Main()
        {
            this.rec = Common.SetupSpeech(this.rec);
            this.rec.SpeechRecognized += this.Recognised;
            this.logger = new Logger();
            this.commands = new List<Command>();
            this.plugins = new Dictionary<String, RecognitionPlugin>();

            this.choices = new Choices();
            this.speech = new Speaker();
        }

        void Recognised(object sender, SpeechRecognizedEventArgs e)
        {
            this.GetLogger().Info("Recognised text: " + e.Result.Text);
            foreach(Command c in this.commands)
            {
                if (e.Result.Text.StartsWith(c.Text))
                {
                    if (this.plugins.ContainsKey(c.Class))
                    {
                        this.plugins[c.Class].onRecognise(c, e.Result.Text);
                        break;
                    }
                    else
                        this.GetLogger().Severe("Invalid Class given for command with identifier '" + c.Identifier + "', class name '" + c.Class + "' given.");
                }
            }
        }

        public void Init()
        {
            Media m = new Media();
            m.onEnable(this);
            this.plugins.Add("Media", m);


/*            this.grammarbuilder = new GrammarBuilder(this.choices);
            this.grammar = new Grammar(this.grammarbuilder);

            this.rec.LoadGrammar(this.grammar);*/

            this.commands.Sort(delegate(Command c1, Command c2) { return c1.Priority.CompareTo(c2.Priority); });

            this.stay = true;

            this.grammarbuilder = new GrammarBuilder(this.choices);
            this.grammar = new Grammar(this.grammarbuilder);
            this.rec.LoadGrammar(this.grammar); 

            while (stay)
               this.rec.Recognize();
        }

        public Logger GetLogger()
        {
            return this.logger;
        }

        public void AddCommand(String plugin, String identifier, GrammarBuilder builder, String text, Int32 priority = 0, String description = "")
        {
            Command comm = new Command();

            comm.Class = plugin;
            comm.Priority = priority;
            comm.Description = description;
            comm.Identifier = identifier;
            comm.Text = text;

            this.choices.Add(builder);

            this.commands.Add(comm);
        }
    }
}
