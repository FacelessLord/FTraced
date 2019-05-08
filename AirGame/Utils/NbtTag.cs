using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GlLib.Utils
{
    public class NbtTag : IEnumerable<string>
    {
        private readonly Hashtable _table;
        private int _suppressExistenceErrorsNumber;

        public NbtTag()
        {
            _suppressExistenceErrorsNumber = 0;
            _table = new Hashtable();
        }

        public int Count => _table.Count;

        public IEnumerable<string> Keys()
        {
            return _table.Keys.Cast<string>();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var key in _table.Keys) yield return key + "";
        }

        public void Set<T>(string _key, T _obj)
        {
            if (!typeof(T).IsPrimitive && !(_obj is string))
                _suppressExistenceErrorsNumber++;
            if (_table.ContainsKey(_key)) _table.Remove(_key);
            _table.Add(_key, _obj);
        }

        public T Get<T>(string _key)
        {
            try
            {
                if (_table.ContainsKey(_key))
                    return (T) _table[_key];
            }
            catch (InvalidCastException e)
            {
                _suppressExistenceErrorsNumber++;
                SidedConsole.WriteLine("Cast error: " + e.Message);
            }

            return default(T);
        }

        public int GetInt(string _key)
        {
            return Get<int>(_key);
        }


#if false
        public long GetLong(string _key)
        {
            return Get<long>(_key);
        }

        public float GetFloat(string _key)
        {
            return Get<float>(_key);
        }

        public double GetDouble(string _key)
        {
            return Get<double>(_key);
        }

        public bool GetBool(string _key)
        {
            return Get<bool>(_key);
        }

        public string GetString(string _key)
        {
            return Get<string>(_key);
        }
#endif

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
                if (entries[2 * i + 1] == "") continue;
                
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
                    default:
                        tag._suppressExistenceErrorsNumber++;
                        continue;
                }
            }

            return tag;
        }

        public void AppendTag(NbtTag _tag, string _prefix)
        {
            foreach (var key in _tag) Set(_prefix + key, _tag._table[key]);
        }

        public NbtTag RetrieveTag(string _prefix)
        {
            var tag = new NbtTag();
            var keysToRemove = new List<string>();
            foreach (var key in this)
                if (key.StartsWith(_prefix))
                {
                    keysToRemove.Add(key);
                    tag.Set(key.Substring(_prefix.Length), _table[key]);
                }

            foreach (var key in keysToRemove) _table.Remove(key);

            return tag;
        }

        public int GetErrorNumber => _suppressExistenceErrorsNumber;

        public bool CanRetrieveTag(string _prefix)
        {
            foreach (var key in this)
                if (key.StartsWith(_prefix))
                    return true;

            return false;
        }
    }
}