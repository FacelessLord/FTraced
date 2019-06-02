using System;
using System.Collections.Generic;
using System.Linq;
using GlLib.Client.Api;

namespace GlLib.Utils.StringParser
{
    internal class Parser
    {
        public Parser()
        {
            _args = new Dictionary<string, Action<string[], IStringIo>>();
            _actionDescription = new Dictionary<string, string>();
        }

        private readonly Dictionary<string, Action<string[], IStringIo>> _args;
        private readonly Dictionary<string, string> _actionDescription;


        public string GetCommandList()
        {
            return _args.Keys.Aggregate((_a, _b) => _a + "\n" + $"{_b,12}: {_actionDescription[_b],-10:NO}");
        }

        public void AddParse(string _word, Action<string[], IStringIo> _delegate, string _description = "")
        {
            _args.Add(_word, _delegate);
            _actionDescription.Add(_word, _description);
        }

        public void Parse(string _arg, IStringIo _io)
        {
            _arg += " ";
            var parsed = _arg.Split(' ');
            if (_args.ContainsKey(parsed[0].ToLower()))
            {
                _args[parsed[0].ToLower()](parsed
                    .Skip(1)
                    .ToArray(), _io);
            }
            else _io.Output($"\t\"{parsed[0]}\" is not command.\n\tType \"help\" to get full list of commands");
        }
    }
}
