using System.Text;

namespace GlLib.Utils
{
    public static class MiscUtils
    {
        public static string Compact(params object[] _objects)
        {
            var delim = "|";
            var ret = new StringBuilder();
            foreach (var obj in _objects) ret.Append(obj + delim);
            if (_objects.Length > 0)
                ret.Remove(ret.Length - 1, 1);

            return ret + "";
        }

        public static string Compact(string _delimiter, params object[] _objects)
        {
            var ret = new StringBuilder();
            foreach (var obj in _objects) ret.Append(obj + _delimiter);

            ret.Remove(ret.Length - 1, 1);

            return ret + "";
        }

        public static string[] Uncompact(string _compString)
        {
            return _compString.Split("|");
        }

        public static string[] Uncompact(string _delimiter, string _compString)
        {
            return _compString.Split(_delimiter);
        }
    }
}