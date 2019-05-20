using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Platform.Windows;

namespace GlLib.Utils.StringParser
{
    internal class Parser
    {
        public Parser()
        {

        }
        // TODO okay
        private Dictionary<string, Action<string>> args;

        public void AddParse(string _word, Action<string> _delegate)
        {
            args.Add(_word, _delegate);
        }

        public void Parse(string _arg)
        {
            var parsed = _arg.Split(' ');
            if (args.ContainsKey(parsed[0]))
            {
                args[parsed[0]](parsed
                    .Skip(1)
                    .Aggregate((_a, _b) => _a + " " + _b));

            }
        }

    }
}
