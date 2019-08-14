namespace GlLib.Common.Items
{
    public class ToolMaterial
    {
        public int meleeAttack;
        public int rangedAttack;
        public int speed;
        public int enchantability;

        public ToolMaterial(int _meleeAttack,int _rangedAttack, int _speed, int _enchantability)
        {
            meleeAttack = _meleeAttack;
            rangedAttack = _rangedAttack;
            speed = _speed;
            enchantability = _enchantability;
        }
    }
}