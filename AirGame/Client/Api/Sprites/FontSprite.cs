using System.Collections.Generic;
using GlLib.Client.API;
using GlLib.Client.Graphic;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Api.Sprites
{
    public class FontSprite
    {
        public Dictionary<char, (int, int)> horizKerning = new Dictionary<char, (int, int)>();
        public Layout layout;
        public Dictionary<char, int> registry = new Dictionary<char, int>();
        public Texture texture;
        public Dictionary<char, int> vertKerning = new Dictionary<char, int>();

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

        public void SetVerticalKern(char _character, int _kern)
        {
            vertKerning.Add(_character, _kern);
        }

        public void SetHorizontalKern(char _character, (int left, int right) _kern)
        {
            horizKerning.Add(_character, _kern);
        }

        public void SetHorizontalKern(char _character, int _left, int _right)
        {
            if (horizKerning.ContainsKey(_character))
                horizKerning[_character] = (_left, _right);
            else
                horizKerning.Add(_character, (_left, _right));
        }

        public (int left, int right) GetHorizontalKern(char _character)
        {
            if (horizKerning.ContainsKey(_character)) return horizKerning[_character];

            return (1, 2);
        }

        public int GetVerticalKern(char _character)
        {
            if (vertKerning.ContainsKey(_character)) return vertKerning[_character];

            return 0;
        }

        public void Setup()
        {
            var l = new List<char>();

            for (var i = 'A'; i <= 'Z'; i++) l.Add(i);

            for (var i = 'a'; i <= 'z'; i++) l.Add(i);

            l.AddRange(new List<char>
            {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+',
                '\\', '/', '<', '>', ',', '.', '?', '|', ';', ':', '[', ']', '{', '}', '`', '~', '\'', '\"', '-', ' ',
                '\t', '\n'
            });

            for (var i = 0; i < l.Count; i++)
                registry.Add(l[i], i);
        }


        public void Render(char _character)
        {
            var (startU, startV, endU, endV) = layout.GetFrameUvProportions(registry[_character]);
            var dh = 0.35 / layout.height;

            Vertexer.DrawSquare(0, 0, 1, 1, startU, startV + dh, endU, endV+dh);
        }

        public double GetTextWidth(string _text, int _size)
        {
            double d = 0;
            for (var i = 0; i < _text.Length; i++)
            {
                var character = _text[i];
                var hKern = GetHorizontalKern(character);
                var leftKern = hKern.left / (float) _size;
                var rightKern = hKern.right / (float) _size;
                d += 1.0 - rightKern - leftKern;
            }

            return d * _size;
        }

        public virtual void DrawText(string _text, int _size, float _r = 0, float _g = 0, float _b = 0, float _a = 1.0f)
        {
            GL.PushMatrix();
            Vertexer.BindTexture(texture);
            Vertexer.Colorize(_r, _g, _b, _a);
            GL.Scale(_size, _size + 2, 1);
            double d = 0;
            for (var i = 0; i < _text.Length; i++)
            {
                var character = _text[i];
                var hKern = GetHorizontalKern(character);
                var vKern = GetVerticalKern(character);
                var leftKern = hKern.left / (float) _size;
                var rightKern = hKern.right / (float) _size;
                var vertKern = vKern / (float) _size;

                if (character != '\n')
                {
                    GL.Translate(-leftKern, vertKern / 2, 0);
                    if (character != ' ') Render(character);

                    d += 1.00 - rightKern - leftKern;
                    GL.Translate(1.0 - rightKern, -vertKern / 2, 0);
                }
                else
                {
                    GL.Translate(-d, 1.2, 0);
                    d = 0;
                }
            }

            Vertexer.ClearColor();
            GL.PopMatrix();
        }
    }
}