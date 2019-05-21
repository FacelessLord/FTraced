using System.Collections.Generic;
using System.Linq;

namespace GlLib.Client.API
{
    public class ChatIo : IStringIo
    {
        private List<string> _input = new List<string>();
        public const int MaxLines = 24;

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