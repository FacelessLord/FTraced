using System.Collections.Generic;

namespace GlLib.Client.Api
{
    public interface IStringIo
    {
        void Output(string _s);

        IEnumerable<string> InputStream();

        bool TryClearStream();
    }
}