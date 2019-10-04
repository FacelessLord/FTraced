using GlLib.Common.Entities;
using GlLib.Common.Entities.Casts.FromPlayer;
using GlLib.Common.Io;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlLib.Common.SpellCastSystem
{
    internal class SpellSystem
    {
        internal const uint MaxCastTime = 1000;
        internal const byte ElementsCountBound = 6;

        // ReSharper disable once InconsistentNaming
        private readonly List<ClassicalElement> elements;

        // ReSharper disable once InconsistentNaming
        private long updatedTime;

        public SpellSystem(Entity _spellCaster)
        {
            SpellCaster = _spellCaster;
            elements = new List<ClassicalElement>();
            IsStarted = false;
        }

        internal uint InternalTime
            => IsStarted
                ? (uint) (Proxy.GetServer().InternalMilliseconds - updatedTime)
                : 0;

        protected bool IsStarted { get; private set; }

        public Entity SpellCaster { get; }

        public void OnUpdate(ElementType _element = ElementType.Empty)
        {
            if (_element != ElementType.Empty)
            {
                if (IsStarted)
                {
                    if (elements.Count < ElementsCountBound)
                    {
                        SidedConsole.WriteLine("Adding of " + (int) _element);

                        elements.Add(new ClassicalElement(InternalTime, _element));
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
                    updatedTime = (uint) Proxy.GetServer().InternalMilliseconds;
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

        public void InterruptCast()
        {
            Refresh();
        }

        private void Refresh()
        {
            IsStarted = false;
            updatedTime = 0;
            elements.Clear();
        }


        private void MakeResult()
        {
            if (!elements.Any()) return;

            var averageTime = elements.Average(e => e.StartTime);
            var averageValue = elements.Average(e => (int) e.type);

            SidedConsole.WriteLine("Result: " + averageValue + " " + averageTime + " " + elements.Count);

            switch (Math.Floor(averageValue))
            {
                case (int) ElementType.Air:
                    SpellCaster.worldObj.SpawnEntity(
                        new AirShield(
                            SpellCaster.worldObj,
                            SpellCaster.Position,
                            SpellCaster.velocity,
                            6000000 + 100000 * (uint) averageTime,
                            0));
                    return;
                case (int) ElementType.Water:
                    return;
                case (int) ElementType.Fire:
                    SpellCaster.worldObj.SpawnEntity(
                        new FireBall(
                            SpellCaster.worldObj,
                            SpellCaster.Position,
                            SpellCaster.direction,
                            SpellCaster.velocity,
                            10000000,
                            (int) Math.Round(averageTime * 5)));
                    return;
                case (int) ElementType.Earth:
                    return;
            }
        }
    }
}