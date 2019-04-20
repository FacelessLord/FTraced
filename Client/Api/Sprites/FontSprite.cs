using System.Collections.Generic;
using GlLib.Client.API;
using GlLib.Client.Graphic;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Sprites
{
    public class FontSprite
    {
        public Texture texture;
        public Layout layout;

        public Dictionary<char, (int, int)> kerning = new Dictionary<char, (int, int)>();
        public Dictionary<char, int> registry = new Dictionary<char, int>();

        public FontSprite(Texture _texture, int _startU, int _startV, int _endU, int _endV, int _countX, int _countY)
        {
            texture = _texture;
            layout = new Layout(_texture.width, _texture.height, _startU, _startV, _endU, _endV, _countX, _countY);
            Setup();
        }

        public FontSprite(Texture _texture, Layout _layout)
        {
            texture = _texture;
            layout = _layout;
            Setup();
        }

        public void SetKern(char _character, (int left, int right) _kern)
        {
            kerning.Add(_character, _kern);
        }

        public void SetKern(char _character, int _left, int _right)
        {
            if (kerning.ContainsKey(_character))
            {
                kerning[_character] = (_left, _right);
            }
            else
                kerning.Add(_character, (_left, _right));
        }

        public (int left, int right) GetKern(char _character)
        {
            if (kerning.ContainsKey(_character))
                return kerning[_character];
            return (0, 2);
        }

        public void Setup()
        {
            var l = new List<char>();

            for (char i = 'A'; i <= 'Z'; i++)
            {
                l.Add(i);
            }

            for (char i = 'a'; i <= 'z'; i++)
            {
                l.Add(i);
            }

            l.AddRange(new List<char>
            {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+',
                '\\', '/', '<', '>', ',', '.', '?', '|', ';', ':', '[', ']', '{', '}', '`', '~'
            });

            for (int i = 0; i < l.Count; i++)
                registry.Add(l[i], i);
        }


        public void Render(char _character)
        {
            (float startU, float startV, float endU, float endV) = layout.GetFrameUvProportions(registry[_character]);

            var kern = GetKern(_character);
            float leftKern = kern.left / (float) texture.width;
            float rightKern = kern.right / (float) texture.width;

            GL.Translate(-leftKern, 0, 0);
            Vertexer.StartDrawingQuads();

            Vertexer.VertexWithUvAt(0, 0, startU, startV);
            Vertexer.VertexWithUvAt(1, 0, endU, startV);
            Vertexer.VertexWithUvAt(1, 1, endU, endV);
            Vertexer.VertexWithUvAt(0, 1, startU, endV);

            Vertexer.Draw();
            GL.Translate(-rightKern, 0, 0);
        }

        public virtual void DrawText(string _text, int _size, float _r = 0, float _g = 0, float _b = 0, float _a = 1.0f)
        {
            GL.PushMatrix();
            Vertexer.BindTexture(texture);
            GL.Color4(_r, _g, _b, _a);
            GL.Scale(_size, _size, 1);
            for(int i=0;i<_text.Length;i++)
            {
                Render(_text[i]);
                GL.Translate(1, 0, 0);
            }

            GL.Color4(1.0, 1, 1, 1);
            GL.PopMatrix();
        }
    }
}