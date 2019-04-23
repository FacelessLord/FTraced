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

        private void SetObject(string _key, object _obj)
        {
            if (_table.ContainsKey(_key)) _table.Remove(_key);
            _table.Add(_key, _obj);
        }

        public void SetInt(string _key, int _num)
        {
            SetObject(_key, _num);
        }

        public void SetDouble(string _key, double _num)
        {
            SetObject(_key, _num);
        }

        public void SetFloat(string _key, float _num)
        {
            SetObject(_key, _num);
        }

        public void SetLong(string _key, long _num)
        {
            SetObject(_key, _num);
        }

        public void SetString(string _key, string _num)
        {
            SetObject(_key, _num);
        }

        public void SetBool(string _key, bool _num)
        {
            SetObject(_key, _num);
        }

        private object GetObject(string _key)
        {
            if (_table.ContainsKey(_key))
                return _table[_key];
            return null;
        }

        public int GetInt(string _key)
        {
            if (_table.ContainsKey(_key))
                return (int) _table[_key];
            return 0;
        }

        public long GetLong(string _key)
        {
            if (_table.ContainsKey(_key))
                return (long) _table[_key];
            return 0;
        }

        public float GetFloat(string _key)
        {
            if (_table.ContainsKey(_key))
                return (float) _table[_key];
            return 0;
        }

        public double GetDouble(string _key)
        {
            if (_table.ContainsKey(_key))
                return (double) _table[_key];
            return 0;
        }

        public bool GetBool(string _key)
        {
            if (_table.ContainsKey(_key))
                return (bool) _table[_key];
            return false;
        }

        public string GetString(string _key)
        {
            if (_table.ContainsKey(_key))
                return (string) _table[_key];
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

        public static NbtTag FromString(string _rawTag)
        {
            var tag = new NbtTag();

            var entries = MiscUtils.Uncompact(_rawTag);
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

        public void AppendTag(NbtTag _tag, string _prefix)
        {
            foreach (var key in _tag) SetObject(_prefix + key, _tag._table[key]);
        }

        public NbtTag RetrieveTag(string _prefix)
        {
            var tag = new NbtTag();
            var keysToRemove = new List<string>();
            foreach (var key in this)
                if (key.StartsWith(_prefix))
                {
                    keysToRemove.Add(key);
                    tag.SetObject(key.Substring(_prefix.Length), _table[key]);
                }

            foreach (var key in keysToRemove) _table.Remove(key);

            return tag;
        }

        public bool CanRetrieveTag(string _prefix)
        {
            foreach (var key in this)
                if (key.StartsWith(_prefix))
                    return true;

            return false;
        }
    }
}