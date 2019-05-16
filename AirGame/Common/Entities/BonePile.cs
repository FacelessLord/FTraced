using GlLib.Client.Graphic.Renderers;

namespace GlLib.Common.Entities
{
    public class BonePile : EntityLiving
    {
        public BonePile()
        {
            MaxHealth = 1;
            Health = 1;
            SetCustomRenderer(new BonePileRenderer());
        }
    }
}