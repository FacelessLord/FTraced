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
            //TODO
            return base.ToString();
        }
    }
}