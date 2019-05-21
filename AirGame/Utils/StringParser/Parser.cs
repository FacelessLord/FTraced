using System;
using System.Collections.Generic;
using System.Linq;

namespace GlLib.Utils.StringParser
{
    internal class Parser
    {
        public Parser()
        {
            _args = new Dictionary<string, Action<string[]>>();
            _actionDescription = new Dictionary<string, string>();
        }
        private readonly Dictionary<string, Action<string[]>> _args;
        private readonly Dictionary<string, string> _actionDescription;


        public string GetCommandList()
        {
            return _args.Keys.Aggregate((_a,_b) => _a + "\n" + $"{_b, 12} {_actionDescription[_b], -10:NO}");
        }
        public void AddParse(string _word, Action<string[]> _delegate, string _description="")
        {
            _args.Add(_word, _delegate);
            _actionDescription.Add(_word, _description);
        }

        public void Parse(string _arg)
        {
            _arg += " ";
            var parsed = _arg.Split(' ');
            if (_args.ContainsKey(parsed[0]))
            {
                _args[parsed[0]](parsed
                    .Skip(1)
                    .ToArray());
            }
            else SidedConsole.WriteLine($"\"{parsed[0]}\" is not command.\n{GetCommandList()}");
        }
    }
}
