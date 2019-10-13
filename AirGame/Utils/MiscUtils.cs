using System.Text;

namespace GlLib.Utils
{
    public static class MiscUtils
    {
        private const string Delim = "|";

        public static double Limit(double _t, double _min, double _max)
        {
            if (_t < _min) return _min;
            if (_t > _max) return _max;

            return _t;
        }

        public static string Compact(params object[] _objects)
        {
            var ret = new StringBuilder();
            foreach (var obj in _objects) ret.Append(obj + Delim);
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
            return _compString.Split(Delim);
        }

        public static string[] Uncompact(string _delimiter, string _compString)
        {
            return _compString.Split(_delimiter);
        }
    }
}