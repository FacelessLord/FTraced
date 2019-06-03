using GlLib.Client.Graphic;

namespace GlLib.Client.Api.Sprites
{
    public class EsogamFontSprite : FontSprite
    {
        public EsogamFontSprite() : base(Vertexer.LoadTexture("fonts/esogam.png"),
            0, 0, 192, 98, 16, 7)
        {
            SetupKern();
        }

        public void SetupKern()
        {
            foreach (char chr in registry.Keys)
                SetHorizontalKern(chr, 1, 1);
//            for (var i = 'a'; i <= 'z'; i++) SetHorizontalKern(i, 1, 3);
//
//            SetHorizontalKern('I', 3, 3);
//            SetHorizontalKern('J', 3, 3);
//            SetHorizontalKern('L', 1, 3);
//            SetHorizontalKern('M', -1, -1);
//            SetHorizontalKern('O', 2, 2);
//            SetHorizontalKern('W', -1, -1);
//
//            SetHorizontalKern(' ', 4, 4);
//            SetHorizontalKern('a', 2, 2);
//            SetHorizontalKern('b', 3, 2);
//            SetHorizontalKern('c', 2, 2);
//            SetHorizontalKern('d', 2, 2);
//            SetHorizontalKern('e', 2, 3);
//            SetHorizontalKern('f', 2, 3);
//            SetHorizontalKern('g', 2, 2);
//            SetHorizontalKern('h', 2, 2);
//            SetHorizontalKern('i', 4, 4);
//            SetHorizontalKern('j', 4, 2);
//            SetHorizontalKern('l', 4, 3);
//            SetHorizontalKern('k', 2, 0);
//            SetHorizontalKern('m', 1, 0);
//            SetHorizontalKern('n', 2, 2);
//            SetHorizontalKern('o', 2, 2);
//            SetHorizontalKern('p', 3, 2);
//            SetHorizontalKern('q', 2, 2);
//            SetHorizontalKern('r', 2, 2);
//            SetHorizontalKern('s', 2, 2);
//            SetHorizontalKern('t', 3, 4);
//            SetHorizontalKern('u', 2, 2);
//            SetHorizontalKern('v', 1, 2);
//            SetHorizontalKern('w', 0, 0);
//            SetHorizontalKern('x', 2, 2);
//            SetHorizontalKern('y', 3, 2);
//
//            SetHorizontalKern('1', 2, 3);
//            SetHorizontalKern('!', 3, 3);
//            SetHorizontalKern('*', 3, 2);
//            SetHorizontalKern('(', 3, 2);
//            SetHorizontalKern(')', 3, 2);
//            SetHorizontalKern('[', 3, 2);
//            SetHorizontalKern(']', 3, 2);
//            SetHorizontalKern('{', 3, 2);
//            SetHorizontalKern('}', 3, 2);
//            SetHorizontalKern('`', 4, 3);
//            SetHorizontalKern(',', 4, 3);
//            SetHorizontalKern('.', 4, 3);
//            SetHorizontalKern('\'', 4, 3);
//            SetHorizontalKern('\"', 4, 3);
//            SetHorizontalKern('?', 4, 3);
//            SetHorizontalKern('|', 4, 4);
//            SetHorizontalKern(';', 4, 3);
//            SetHorizontalKern(':', 4, 3);
        }
    }
}