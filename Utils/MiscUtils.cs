using System.Collections.Generic;
using System.Text;

namespace GlLib.Utils
{
    public static class MiscUtils
    {
        public static string Compact(params object[] objects)
        {
            string delim = "|";
            StringBuilder ret = new StringBuilder();
            foreach (var obj in objects)
            {
                ret.Append(obj+delim);
            }
            if(objects.Length>0)
                ret.Remove(ret.Length - 1, 1);

            return ret + "";
        }
        
        public static string Compact(string delimiter,params object[] objects)
        {
            StringBuilder ret = new StringBuilder();
            foreach (var obj in objects)
            {
                ret.Append(obj+delimiter);
            }

            ret.Remove(ret.Length - 1, 1);

            return ret + "";
        }

        public static string[] Uncompact(string compString)
        {
            return compString.Split("|");
        }
        public static string[] Uncompact(string delimiter,string compString)
        {
            return compString.Split(delimiter);
        }
    }
}