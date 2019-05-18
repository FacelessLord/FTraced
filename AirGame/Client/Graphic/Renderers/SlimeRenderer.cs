using GlLib.Client.Api.Sprites;
using GlLib.Client.API;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Graphics;

namespace GlLib.Client.Graphic.Renderers
{
    public class SlimeRenderer : EntityRenderer
    {
        public LinearSprite slimeSprite;
        public LinearSprite spawnSprite;

        public override void Setup(Entity _p)
        {
            var layout = new TextureLayout("slime/slime_waiting.png", 7, 1);
            slimeSprite = new LinearSprite(layout, 22, 20);
            spawnSprite = SpawnSprite;
            spawnSprite.MoveSpriteTo(new PlanarVector(-2, 40));
            spawnSprite.SetColor(new Color4(1, 1, 1, 0.5f));
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            slimeSprite.Render();
//            if (spawnSprite.FullFrameCount< 1)
//                spawnSprite.Render();
        }
    }
}