using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlLib.Common.Entities;
using GlLib.Utils;

namespace GlLib.Common.SpellCastSystem
{
    internal class SpellSystem
    {
        public const uint MaxCastTime = 3 * 1000;
        public const byte ElementsCountBound = 6; 
        // ReSharper disable once InconsistentNaming
        private uint? time;
        internal uint? InternalTime
            => IsStarted
                ? (uint) DateTime.Now.TimeOfDay.TotalMilliseconds - time
                : 0;

        protected bool IsStarted
        {
            get; 
            private set;
        }

        // ReSharper disable once InconsistentNaming
        private readonly List<ClassicalElement> elements;
        public Entity SpellCaster { get; }

        public SpellSystem(Entity _spellCaster)
        {
            SpellCaster = _spellCaster;
            elements = new List<ClassicalElement>();
            IsStarted = false;

        }
        public void OnUpdate(ElementType _element = ElementType.Empty)
        {
            if (_element != ElementType.Empty)
            {
                if (IsStarted)
                {
                    if (elements.Count < ElementsCountBound)
                    {
                        uint totalMilliseconds = (uint) (DateTime.Now.TimeOfDay.TotalMilliseconds - time);
                        SidedConsole.WriteLine("Adding of " + (int)_element);
                        elements.Add(new ClassicalElement(totalMilliseconds, _element));
                    }
                    else if (elements.Count == ElementsCountBound)
                    {
                        MakeResult();
                        Refresh();
                        return;
                    }
                }
                else
                {
                    SidedConsole.WriteLine("Spell casting start");
                    time = (uint) DateTime.Now.TimeOfDay.TotalMilliseconds;
                    IsStarted = true;
                    elements.Add(new ClassicalElement(1, _element));
                }
            }

            if (IsStarted && InternalTime >= MaxCastTime)
            {
                MakeResult();
                Refresh();
            }

        }

        private void Refresh()
        {
            IsStarted = false;
            time = null;
            elements.Clear();
        }
        private void MakeResult()
        {
            double averageTime = elements.Average(e => e.StartTime);
            double averageValue = elements.Average(e => (int) e.type);

            SidedConsole.WriteLine("Result: " + averageValue +" " + averageTime+ " " + elements.Count);

            switch ((int) Math.Floor(averageValue))
            {
                case (int) ElementType.Air:
                    SpellCaster.worldObj.SpawnEntity(
                        new FireBall(
                            SpellCaster.worldObj,
                            SpellCaster.Position, 
                            SpellCaster.velocity,
                            (uint) Math.Round(averageTime * 3),
                            (int) Math.Round(averageTime * 5)));
                    return;
                case (int)ElementType.Water:
                    return;
                case (int)ElementType.Fire:
                    return;
                case (int)ElementType.Earth:
                    return;
            }


            //TODO think how system can say to server to make result 
            //TODO cast spell using this time and average element, simple but it should work

        }
    }
}
