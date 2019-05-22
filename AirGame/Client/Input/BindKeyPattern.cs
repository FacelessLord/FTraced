using System;
using System.Collections;
using System.Collections.Generic;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class BindKeyPattern
    {
        public BindKeyPattern()
        {
            TimeStart = 0;
        }

        private uint TimeStart { get; }
        private Hashtable Keys { get; set; }
        public Delegate Result { get; }

        public void Update(uint _time, params Key[] _keys)
        {
        }

        public bool StartsWith(List<Key> _keys)
        {
            return false;
        }
    }
}