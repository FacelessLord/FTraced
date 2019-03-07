using System.Collections;
using System.Collections.Generic;

namespace GlLib.Utils
{
    public class NbtTag : IEnumerable<string>
    {
        private readonly Hashtable _table = new Hashtable();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var key in _table.Keys) yield return key + "";
        }

        private void SetObject(string key, object obj)
        {
            if (_table.ContainsKey(key)) _table.Remove(key);
            _table.Add(key, obj);
        }

        public void SetInt(string key, int num)
        {
            SetObject(key, num);
        }

        public void SetDouble(string key, double num)
        {
            SetObject(key, num);
        }

        public void SetFloat(string key, float num)
        {
            SetObject(key, num);
        }

        public void SetLong(string key, long num)
        {
            SetObject(key, num);
        }

        public void SetString(string key, string num)
        {
            SetObject(key, num);
        }

        public void SetBool(string key, bool num)
        {
            SetObject(key, num);
        }

        private object GetObject(string key)
        {
            if (_table.ContainsKey(key))
                return _table[key];
            return null;
        }

        public int GetInt(string key)
        {
            if (_table.ContainsKey(key))
                return (int) _table[key];
            return 0;
        }

        public long GetLong(string key)
        {
            if (_table.ContainsKey(key))
                return (long) _table[key];
            return 0;
        }

        public float GetFloat(string key)
        {
            if (_table.ContainsKey(key))
                return (float) _table[key];
            return 0;
        }

        public double GetDouble(string key)
        {
            if (_table.ContainsKey(key))
                return (double) _table[key];
            return 0;
        }

        public bool GetBool(string key)
        {
            if (_table.ContainsKey(key))
                return (bool) _table[key];
            return false;
        }

        public string GetString(string key)
        {
            if (_table.ContainsKey(key))
                return (string) _table[key];
            return "";
        }

        public override string ToString()
        {
            var entries = new List<object>();
            foreach (var key in _table.Keys)
            {
                entries.Add(key);
                entries.Add(_table[key].GetType().Name[0] + "" + _table[key]);
            }

            return MiscUtils.Compact(entries.ToArray());
        }

        public static NbtTag FromString(string rawTag)
        {
            var tag = new NbtTag();

            var entries = MiscUtils.Uncompact(rawTag);
            for (var i = 0; i < entries.Length / 2; i++)
            {
                var valueType = entries[2 * i + 1][0];
                var value = entries[2 * i + 1].Substring(1);
                switch (valueType)
                {
                    case 'I':
                        tag._table[entries[2 * i]] = int.Parse(value);
                        break;
                    case 'L':
                        tag._table[entries[2 * i]] = long.Parse(value);
                        break;
                    case 'F':
                        tag._table[entries[2 * i]] = float.Parse(value);
                        break;
                    case 'D':
                        tag._table[entries[2 * i]] = double.Parse(value);
                        break;
                    case 'B':
                        tag._table[entries[2 * i]] = bool.Parse(value);
                        break;
                    case 'S':
                        tag._table[entries[2 * i]] = value;
                        break;
                }
            }

            return tag;
        }

        public void AppendTag(NbtTag tag, string prefix)
        {
            foreach (var key in tag) SetObject(prefix + key, tag._table[key]);
        }

        public NbtTag RetrieveTag(string prefix)
        {
            var tag = new NbtTag();
            var keysToRemove = new List<string>();
            foreach (var key in this)
                if (key.StartsWith(prefix))
                {
                    keysToRemove.Add(key);
                    tag.SetObject(key.Substring(prefix.Length), _table[key]);
                }

            foreach (var key in keysToRemove) _table.Remove(key);

            return tag;
        }

        public bool CanRetrieveTag(string prefix)
        {
            foreach (var key in this)
                if (key.StartsWith(prefix))
                    return true;

            return false;
        }
    }
}