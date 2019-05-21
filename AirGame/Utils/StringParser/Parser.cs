using System;
using System.Collections.Generic;
using System.Linq;

namespace GlLib.Utils.StringParser
{
    internal class Parser
    {
        public Parser()
        {
            args = new Dictionary<string, Action<string[]>>();
        }
        // TODO 
        private readonly Dictionary<string, Action<string[]>> args;


        public string GetCommandList()
        {
            return args.Keys.Aggregate( (a,b) => a + "\n" + b);
        }
        public void AddParse(string _word, Action<string[]> _delegate)
        {
            args.Add(_word, _delegate);
        }

        public void Parse(string _arg)
        {
            _arg += " ";
            var parsed = _arg.Split(' ');
            if (args.ContainsKey(parsed[0]))
            {
                args[parsed[0]](parsed
                    .Skip(1)
                    .ToArray());
            }
            else SidedConsole.WriteLine($"\"{parsed[0]}\" is not command.\n{GetCommandList()}");
        }
    }
}
