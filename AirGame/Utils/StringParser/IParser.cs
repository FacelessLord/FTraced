using System;
using GlLib.Client.Api;

namespace GlLib.Utils.StringParser
{
    public interface IParser
    {
        void Parse(string _arg, IStringIo _io);
    }
}