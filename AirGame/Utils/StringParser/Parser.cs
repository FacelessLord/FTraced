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
        private Dictionary<string, Action<string>> args = new Dictionary<string, Action<string>>();

        public void AddParse(string _word, Action<string> _delegate)
        {
            args.Add(_word, _delegate);
        }

        public void Parse(string _arg)
        {
            var parsed = _arg.Split(' ');
            if (args.ContainsKey(parsed[0]))
            {
                if (parsed.Skip(1).Any())
                    args[parsed[0]](parsed
                        .Skip(1)
                        .Aggregate((_a, _b) => _a + " " + _b));
                else
                {
                    args[parsed[0]]("");
                }
            }
        }
    }
}
