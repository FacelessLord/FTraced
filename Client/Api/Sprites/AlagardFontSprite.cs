using GlLib.Client.API;
using GlLib.Client.Graphic;
using GlLib.Utils;

namespace GlLib.Client.Api.Sprites
{
    public class AlagardFontSprite : FontSprite
    {
        public AlagardFontSprite() : base(Vertexer.LoadTexture("fonts/alagard.png"),
            0, 0, 192, 96, 16, 8)
        {
            SetupKern();
        }

        public void SetupKern()
        {
            for (char i = 'a'; i <= 'z'; i++)
            {
                SetKern(i, 1, 3);
            }

            SetKern('I', 3, 3);
            SetKern('J', 3, 3);
            SetKern('L', 1, 3);
            SetKern('M', -1, -1);
            SetKern('W', -1, -1);

            SetKern('i', 3, 3);
            SetKern('j', 3, 3);
            SetKern('l', 3, 3);
            SetKern('m', -1, -1);
            SetKern('w', -1, -1);

            SetKern('1', 3, 3);
            SetKern('!', 3, 3);
            SetKern('*', 3, 2);
            SetKern('(', 3, 2);
            SetKern(')', 3, 2);
            SetKern('[', 3, 2);
            SetKern(']', 3, 2);
            SetKern('{', 3, 2);
            SetKern('}', 3, 2);
            SetKern('`', 4, 3);
            SetKern(',', 4, 3);
            SetKern('.', 4, 3);
            SetKern('?', 4, 3);
            SetKern('|', 4, 4);
            SetKern(';', 4, 3);
            SetKern(':', 4, 3);
        }
    }
}