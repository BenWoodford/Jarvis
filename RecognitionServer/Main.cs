using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Globalization;
using System.Reflection;
using RecognitionServer.Entities;

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

        public Main()
        {
            Common.SetupSpeech(ref this.rec);
            this.rec.SpeechRecognized += this.Recognised;
            this.logger = new Logger();
            this.commands = new List<Command>();
        }

        void Recognised(object sender, SpeechRecognizedEventArgs e)
        {

        }

        public void Init()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (Type t in asm.GetTypes())
            {
                if (t.Namespace == "RecognitionServer.Plugins")
                {
                    Common.InvokePlugin(this, t.Name, "onEnable");
                }
            }

/*            this.grammarbuilder = new GrammarBuilder(this.choices);
            this.grammar = new Grammar(this.grammarbuilder);

            this.rec.LoadGrammar(this.grammar);*/

            this.stay = true;

            while (stay)
                this.rec.Recognize();
        }

        public Logger GetLogger()
        {
            return this.logger;
        }

        public void AddCommand(String plugin, String identifier, String text, Int32 priority = 0, String description = "")
        {
            Command comm = new Command();

            comm.Class = plugin;
            comm.Text = text;
            comm.Priority = priority;
            comm.Description = description;
            comm.Identifier = identifier;

            this.commands.Add(comm);
        }
    }
}
