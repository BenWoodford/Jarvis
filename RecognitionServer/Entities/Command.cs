using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecognitionServer.Entities
{
    class Command
    {
        public String Class = "";
        public Int32 Priority = 0; // Higher means checked later.
        public String Description = "";
        public String Text = "";
        public String Identifier = "";
    }
}
