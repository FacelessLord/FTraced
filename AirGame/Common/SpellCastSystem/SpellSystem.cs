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
        internal const uint MaxCastTime = 3 * 1000;
        internal const byte ElementsCountBound = 6; 
        // ReSharper disable once InconsistentNaming
        private uint time;
        internal uint InternalTime
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
            time = 0;
            elements.Clear();
        }
        private void MakeResult()
        {
            double averageTime = elements.Average(e => e.StartTime);
            double averageValue = elements.Average(e => (int) e.type);

            SidedConsole.WriteLine("Result: " + averageValue +" " + averageTime+ " " + elements.Count);

            switch (Math.Floor(averageValue))
            {
                case (int)ElementType.Air:
                    SpellCaster.worldObj.SpawnEntity(
                        new AirShield(
                            SpellCaster.worldObj,
                            SpellCaster.Position, 
                            SpellCaster.velocity,
                            5000000 + 100000 * (uint)averageTime,
                            0));
                    return;
                case (int)ElementType.Water:
                    return;
                case (int)ElementType.Fire:
                    SpellCaster.worldObj.SpawnEntity(
                        new FireBall(
                            SpellCaster.worldObj,
                            SpellCaster.Position,
                            SpellCaster.direction,
                            SpellCaster.velocity,
                            10000000,
                            (int)Math.Round(averageTime * 5)));
                    return;
                case (int)ElementType.Earth:
                    return;
            }


            //TODO think how system can say to server to make result 
            //TODO cast spell using this time and average element, simple but it should work

        }
    }
}
