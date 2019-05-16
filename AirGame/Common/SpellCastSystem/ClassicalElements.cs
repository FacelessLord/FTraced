namespace GlLib.Common.SpellCastSystem
{
    internal struct ClassicalElement
    {
        public ElementType type;

        public ClassicalElement(uint _startTime, ElementType _type)
        {
            type = _type;
            StartTime = _startTime;
        }

        public uint StartTime { get; }

        public override string ToString()
        {
            switch (type)
            {
                case ElementType.Water:
                    return "Water";
                case ElementType.Earth:
                    return "Earth";
                case ElementType.Fire:
                    return "Fire";
                case ElementType.Air:
                    return "Air";
            }
            return base.ToString();
        }
    }
}