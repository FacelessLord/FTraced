using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OpenTK.Input;

namespace GlLib.Client.Input
{
     internal abstract class BindPattern
    {
        protected BindPattern(Delegate _result)
        {
            Result = _result;
        }

        private uint TimeStart { get; set; }
        private Hashtable Keys { get; set; }
        public virtual Delegate Result { get; }


        public abstract void Update(uint _time, params Key[] _keys);

        public abstract bool StartsWith(List<Key> _keys);

    }
}
