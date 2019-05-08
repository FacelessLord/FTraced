using OpenTK.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GlLib.Client.Input
{
    public class BindKeyPattern
    {
        private uint TimeStart { get; set; }
        private Hashtable Keys { get; set; }
        public Delegate Result { get; }

        public BindKeyPattern()
        {
            TimeStart = 0;
        }

        public void Update(uint _time,  params Key[] _keys)
        {
            
        }

        public bool StartsWith(List<Key> _keys)
        {
            return false;
        }

        
    }
}
