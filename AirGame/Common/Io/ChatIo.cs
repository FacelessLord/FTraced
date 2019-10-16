using System.Collections.Generic;
using System.Linq;
using GlLib.Client.Api;

namespace GlLib.Common.Io
{
    public class ChatIo : IStringIo
    {
        public const int MaxLines = 20;
        private readonly List<string> _input = new List<string>();

        public void Output(string _s)
        {
            var lines = _s.Split('\n');
            _input.AddRange(lines.Where(_l => _l != ""));
        }

        public IEnumerable<string> InputStream()
        {
            return _input.AsEnumerable().Reverse().Take(MaxLines);
        }

        public bool TryClearStream()
        {
            _input.Clear();
            return true;
        }
    }
}