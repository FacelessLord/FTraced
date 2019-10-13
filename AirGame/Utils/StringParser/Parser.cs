using System;
using System.Collections.Generic;
using System.Linq;
using GlLib.Client.Api;

namespace GlLib.Utils.StringParser
{
    public class Parser : IParser
    {
        private readonly Dictionary<string, Action<string[], IStringIo>> _actions;
        protected readonly Dictionary<string, string> actionDescription;

        public Parser()
        {
            _actions = new Dictionary<string, Action<string[], IStringIo>>();
            actionDescription = new Dictionary<string, string>();
        }

        public void Parse(string _arg, IStringIo _io)
        {
//            _arg += " ";
            var parsed = _arg.Split(' ');
            if (_actions.ContainsKey(parsed[0].ToLower()))
                _actions[parsed[0].ToLower()](parsed
                    .Skip(1)
                    .ToArray(), _io);
            else _io.Output($"\t\"{parsed[0]}\" is not command.\n\tType \"help\" to get full list of commands");
        }


        public string[] GetCommandList()
        {
            return _actions.Keys.Skip(1).Select(_c => $"{_c} : {actionDescription[_c]}").ToArray();
        }

        public void AddParse(string _word, Action<string[], IStringIo> _delegate, string _description = "")
        {
            _actions.Add(_word, _delegate);
            actionDescription.Add(_word, _description);
        }
    }
}