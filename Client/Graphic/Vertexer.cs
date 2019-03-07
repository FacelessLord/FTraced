using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic
{
    public class Vertexer
    {
        public static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public static void EnableTextures()
        {
            GL.Enable(EnableCap.Texture2D);
        }

        public static void DisableTextures()
        {
            GL.Disable(EnableCap.Texture2D);
        }

        public static void StartDrawingQuads()
        {
            GL.Begin(PrimitiveType.Quads);
        }

        public static void StartDrawing(PrimitiveType type)
        {
            GL.Begin(type);
        }

        public static void VertexAt(double x, double y)
        {
            GL.Vertex2(x, y);
        }

        public static void VertexAt(double x, double y, double z)
        {
            GL.Vertex3(x, y, z);
        }

        public static void VertexWithUvAt(double x, double y, double u, double v)
        {
            GL.TexCoord2(u, v);
            GL.Vertex2(x, y);
        }

        public static void VertexWithUvAt(double x, double y, double z, double u, double v)
        {
            GL.TexCoord2(u, v);
            GL.Vertex3(x, y, z);
        }

        public static void Draw()
        {
            GL.End();
        }

        public static Texture LoadTexture(string path)
        {
            if (textures.ContainsKey(path))
                return textures[path];
            var texture = new Texture("textures/" + path);
            textures.Add(path, texture);
            return texture;
        }

        public static void DrawTexturedModalRect(Texture texture, double x, double y, double u, double v, double width,
            double height)
        {
            GL.PushMatrix();
            texture.Bind();
            var textureLeft = x;
            var textureUp = y;
            var textureRight = x + width;
            var textureDown = y + height;

            var uvLeft = u / texture.width;
            var uvUp = v / texture.height;
            var uvRight = (u + width) / texture.width;
            var uvDown = (v + height) / texture.height;

            StartDrawingQuads();

            VertexWithUvAt(textureLeft, textureUp, uvLeft, uvUp);
            VertexWithUvAt(textureRight, textureUp, uvRight, uvUp);
            VertexWithUvAt(textureRight, textureDown, uvRight, uvDown);
            VertexWithUvAt(textureLeft, textureDown, uvLeft, uvDown);

            Draw();
            GL.PopMatrix();
        }

        public static void BindTexture(Texture text)
        {
            text.Bind();
        }
    }
}