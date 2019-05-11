using OpenTK.Input;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GlLib.Client.Input
{
    public class BindSystem
    {
        private uint TimeTick { get; set; }
        private List<BindKeyPattern> Patterns { get; set; }
        public Hashtable PressedKeys { get; set; }

        public List<BindKeyPattern> ExpectedPatterns { get; set; }

        public void OnKeyUpdate(params Key[] _keys)
        {
            
            foreach (var pattern in Patterns)
            {
                if (pattern.StartsWith(_keys.ToList()))
                {
                    ExpectedPatterns.Add(pattern);
                }
            }

            foreach (var pattern in ExpectedPatterns)
            {
                pattern.Update(TimeTick, _keys);
            }


            PressedKeys[TimeTick] = _keys;
            Update();
            
            
        }

        private void Update()
        {
            foreach (var expPattern in ExpectedPatterns)
            {
                
            }

            TimeTick++;
        }


    }
}
