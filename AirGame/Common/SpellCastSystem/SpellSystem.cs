using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlLib.Common.Entities;

namespace GlLib.Common.SpellCastSystem
{
    internal class SpellSystem
    {
        public const uint MaxCastTime = 2 * 60 * 1000;
        public const byte ElementsCountBound = 6; 
        // ReSharper disable once InconsistentNaming
        private static uint? time;
        protected  bool IsStarted { get; private set; }

        // ReSharper disable once InconsistentNaming
        private readonly List<ClassicalElement> elements;
        public Player SpellCaster { get; }

        public SpellSystem(Player _spellCaster)
        {
            SpellCaster = _spellCaster;
            elements = new List<ClassicalElement>();
            IsStarted = false;

        }
        public  void OnUpdate(ElementType _element = ElementType.Empty)
        {
            if (_element != ElementType.Empty)
            {
                if (IsStarted)
                {
                    if (elements.Count < ElementsCountBound)
                    {
                        uint totalMilliseconds = (uint) (DateTime.Now.TimeOfDay.TotalMilliseconds - time);
                        elements.Add(new ClassicalElement(totalMilliseconds, _element));
                    }
                    else if (elements.Count == ElementsCountBound)
                    {
                        MakeResult();
                        IsStarted = false;
                        return;
                    }
                }
                else
                {
                    time = (uint) DateTime.Now.TimeOfDay.TotalMilliseconds;
                    IsStarted = true;
                    elements.Add(new ClassicalElement(1, _element));
                }
            }

            if (MaxCastTime > time - DateTime.Now.TimeOfDay.TotalMilliseconds)
            {
                MakeResult();
                IsStarted = false;
            }

        }

        private  void MakeResult()
        {
            Calculate();
            //TODO think how system can say to server to make result 
            throw new NotImplementedException();
        }

        private  void Calculate()
        {
            double averageTime = elements.Average(e => e.StartTime);
            double averageValue = elements.Average(e => (int) e.type);

            //TODO cast spell using this time and average element, simple but it should work

        }
    }
}
