using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecognitionServer.Entities;

namespace RecognitionServer
{
    class RecognitionPlugin
    {
        public virtual void onRecognise(Command comm, String text = "") { }
        public virtual void onEnable(RecognitionServer.Main m) { }
    }
}