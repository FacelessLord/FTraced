using System.Collections.Generic;

namespace GlLib.Common.Api
{
    public class Trait
    {
        public static readonly Trait Health = new Trait(0, 40);
        public static readonly Trait Mana = new Trait(1, 40);
        public static readonly Trait Level = new Trait(2, 1);
        public static readonly Trait MeleeDamage = new Trait(3, 1);
        public static readonly Trait RangedDamage = new Trait(4, 1);
        public static readonly Trait AirLevel = new Trait(5, 1);
        public static readonly Trait FireLevel = new Trait(6, 1);
        public static readonly Trait WaterLevel = new Trait(7, 1);
        public static readonly Trait EarthLevel = new Trait(8, 1);

        public static List<Trait> traits = new List<Trait>();

        public int baseValue;
        public int id;

        public Trait(int _id, int _baseValue)
        {
            id = _id;
            baseValue = _baseValue;
            traits.Add(this);
        }
    }
}