using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Graphic
{
    public class Vertexer
    {
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

        public static void VertexAt(double x,double y)
        {
            GL.Vertex2(x,y);
        }
        public static void VertexAt(double x,double y,double z)
        {
            GL.Vertex3(x,y,z);
        }
        
        public static void VertexWithUvAt(double x,double y,double u,double v)
        {
            GL.TexCoord2(u,v);
            GL.Vertex2(x,y);
        }
        public static void VertexWithUvAt(double x,double y,double z,double u,double v)
        {
            GL.TexCoord2(u,v);
            GL.Vertex3(x,y,z);
        }

        public static void Draw()
        {
            GL.End();
        }

        public static Texture LoadTexture(string path)
        {
            if (textures.ContainsKey(path))
                return textures[path];
            var texture = new Texture("textures/"+path);
            textures.Add(path,texture);
            return texture;
        }

        public static void DrawTexturedModalRect(Texture texture,double x,double y, double u, double v, double width, double height)
        {
            texture.Bind();
            double textureLeft = x;
            double textureUp = y;
            double textureRight = x + width;
            double textureDown = y + height;

            double uvLeft = u / texture.width;
            double uvUp = v / texture.height;
            double uvRight= (u+width) / texture.width;
            double uvDown = (v+height)/ texture.height;
            
            StartDrawingQuads();
            
            VertexWithUvAt(textureLeft,textureUp,uvLeft,uvUp);
            VertexWithUvAt(textureRight,textureUp,uvRight,uvUp);
            VertexWithUvAt(textureRight,textureDown,uvRight,uvDown);
            VertexWithUvAt(textureLeft,textureDown,uvLeft,uvDown);
            
            Draw();
        }

        public static void DrawTexturedModalRect(double x, double y, double u, double v, double width, double height)
        {
            double textureLeft = x;
            double textureUp = y;
            double uOverV = width / height;
            double kU = uOverV > 1 ? uOverV : 1;
            double kV = uOverV < 1 ? uOverV : 1;
            double textureRight = x + width;
            double textureDown = y + height;

            double uvLeft = u / _texture.width;
            double uvUp = v / _texture.height;
            double uvRight = (u + width) / _texture.width;
            double uvDown = (v + height) / _texture.height;

            StartDrawingQuads();

            VertexWithUvAt(textureLeft, textureUp, uvLeft, uvUp);
            VertexWithUvAt(textureRight, textureUp, uvRight, uvUp);
            VertexWithUvAt(textureRight, textureDown, uvRight, uvDown);
            VertexWithUvAt(textureLeft, textureDown, uvLeft, uvDown);

            Draw();
        }

        public static Dictionary<string,Texture> textures = new Dictionary<string,Texture>();

        public static Texture _texture = new Texture("textures/null.png");
        
        public static void BindTexture(Texture text)
        {
            EnableTextures();
            _texture = text;
            text.Bind();
        }
    }
}