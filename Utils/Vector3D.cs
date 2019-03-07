using System;

namespace GlLib.Utils
{
    public class RestrictedVector3D
    {
        public double x;
        public double y;
        public int z;

        public RestrictedVector3D()
        {
        }

        public RestrictedVector3D(double x, double y, int z)
        {
            (this.x, this.y, this.z) = (x, y, z);
        }

        public int Ix => (int) Math.Floor(x);
        public int Iy => (int) Math.Floor(y);

        public static RestrictedVector3D operator +(RestrictedVector3D a, RestrictedVector3D b)
        {
            if (a.z != b.z)
                throw new ArgumentException("Tried to sum vectors with different heights");
            return new RestrictedVector3D(a.x + b.x, a.y + b.y, a.z);
        }

        public static RestrictedVector3D operator +(RestrictedVector3D a, PlanarVector b)
        {
            return new RestrictedVector3D(a.x + b.x, a.y + b.y, a.z);
        }

        public static RestrictedVector3D operator -(RestrictedVector3D a)
        {
            return new RestrictedVector3D(-a.x, -a.y, a.z);
        }

        public static RestrictedVector3D operator -(RestrictedVector3D a, PlanarVector b)
        {
            return a + -b;
        }

        public static RestrictedVector3D operator -(RestrictedVector3D a, RestrictedVector3D b)
        {
            if (a.z != b.z)
                throw new ArgumentException("Tried to subtract vectors with different heights");
            return a + -b;
        }

        public static RestrictedVector3D operator *(RestrictedVector3D a, double k)
        {
            return new RestrictedVector3D(a.x * k, a.y * k, a.z);
        }

        public static RestrictedVector3D operator *(double k, RestrictedVector3D a)
        {
            return new RestrictedVector3D(a.x * k, a.y * k, a.z);
        }

        public static RestrictedVector3D FromAngleAndHeight(double angle, int height)
        {
            return new RestrictedVector3D(Math.Cos(angle), Math.Sin(angle), height);
        }

        public RestrictedVector3D Rotate(double angle)
        {
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            return new RestrictedVector3D(x * cos - y * sin, x * sin + y * cos, z);
        }

        public override string ToString()
        {
            return $"({x},{y},{z})";
        }

        public static RestrictedVector3D FromString(string s)
        {
            if (s == "")
                return new RestrictedVector3D();
            var coords = s.Substring(1, s.Length - 2).Split(",");
            return new RestrictedVector3D(double.Parse(coords[0]), double.Parse(coords[1]), int.Parse(coords[2]));
        }

        public PlanarVector ToPlanar()
        {
            return new PlanarVector(x, y);
        }
    }

    public class PlanarVector
    {
        public double x;
        public double y;

        public PlanarVector()
        {
        }

        public PlanarVector(double x, double y)
        {
            (this.x, this.y) = (x, y);
        }

        public double Length => Math.Sqrt(x * x + y * y);

        public static PlanarVector operator *(PlanarVector a, double k)
        {
            return new PlanarVector(a.x * k, a.y * k);
        }

        public static PlanarVector operator /(PlanarVector a, double k)
        {
            return new PlanarVector(a.x / k, a.y / k);
        }

        public static PlanarVector operator +(PlanarVector a, PlanarVector b)
        {
            return new PlanarVector(a.x + b.x, a.y + b.y);
        }

        public static PlanarVector operator -(PlanarVector a)
        {
            return new PlanarVector(-a.x, -a.y);
        }

        public override string ToString()
        {
            return $"({x},{y})";
        }

        public static PlanarVector FromString(string s)
        {
            if (s == "")
                return new PlanarVector();
            var coords = s.Substring(1, s.Length - 2).Split(",");
            return new PlanarVector(double.Parse(coords[0]), double.Parse(coords[1]));
        }

        public AxisAlignedBb Expand(double width, double height)
        {
            return new AxisAlignedBb(x, y, x + width, y + height);
        }

        public AxisAlignedBb ExpandBothTo(double width, double height)
        {
            return new AxisAlignedBb(x - width / 2, y / height, x + width / 2, y + height / 2);
        }
    }

    public class AxisAlignedBb
    {
        public double endX;
        public double endY;
        public double startX;
        public double startY;

        public AxisAlignedBb(double startX, double startY, double endX, double endY)
        {
            (this.startX, this.startY, this.endX, this.endY) = (startX, startY, endX, endY);
            CheckCoordinates();
        }

        public AxisAlignedBb(PlanarVector start, PlanarVector end)
        {
            (startX, startY, endX, endY) = (start.x, start.y, end.x, end.y);
            CheckCoordinates();
        }

        public int StartXi => (int) startX;
        public int StartYi => (int) startY;
        public int EndXi => (int) endX;
        public int EndYi => (int) endY;

        public double Width => endX - startX;
        public double Height => endY - startY;

        public bool IntersectsWith(AxisAlignedBb box)
        {
            var cx1 = (startX + endX) / 2;
            var cy1 = (startY + endY) / 2;
            var cx2 = (box.startX + box.endX) / 2;
            var cy2 = (box.startY + box.endY) / 2;

            var halfWidth = endX - cx1 + box.endX - cx2;
            var halfHeight = endY - cy1 + box.endY - cy2;

            return Math.Abs(cx1 - cx2) < halfWidth && Math.Abs(cy1 - cy2) < halfHeight;
        }

        public static AxisAlignedBb operator +(AxisAlignedBb a, PlanarVector v)
        {
            return new AxisAlignedBb(a.startX + v.x, a.startY + v.y, a.endX + v.x, a.endY + v.y);
        }

        public void CheckCoordinates()
        {
            if (startX > endX) (startX, endX) = (endX, startY);

            if (startY > endY) (startY, endY) = (endY, startY);
        }
    }
}