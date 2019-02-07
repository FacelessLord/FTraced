using System;
using DiggerLib;
using OpenTK;

namespace GlLib.Utils
{
    public class RestrictedVector3D
    {
        public double _x = 0;
        public double _y = 0;
        public int _z = 0;

        public int Ix => (int) _x;
        public int Iy => (int) _y;

        public RestrictedVector3D()
        {
        }

        public RestrictedVector3D(double x, double y, int z)
        {
            (_x, _y, _z) = (x, y, z);
        }

        public static RestrictedVector3D operator +(RestrictedVector3D a, RestrictedVector3D b)
        {
            if (a._z != b._z)
                throw new ArgumentException("Tried to sum vectors with different heights");
            return new RestrictedVector3D(a._x + b._x, a._y + b._y, a._z);
        }

        public static RestrictedVector3D operator +(RestrictedVector3D a, PlanarVector b)
        {
            return new RestrictedVector3D(a._x + b._x, a._y + b._y, a._z);
        }

        public static RestrictedVector3D operator -(RestrictedVector3D a)
        {
            return new RestrictedVector3D(-a._x, -a._y, a._z);
        }

        public static RestrictedVector3D operator -(RestrictedVector3D a, PlanarVector b)
        {
            return a + -b;
        }

        public static RestrictedVector3D operator -(RestrictedVector3D a, RestrictedVector3D b)
        {
            if (a._z != b._z)
                throw new ArgumentException("Tried to subtract vectors with different heights");
            return a + -b;
        }

        public static RestrictedVector3D operator *(RestrictedVector3D a, double k)
        {
            return new RestrictedVector3D(a._x * k, a._y * k, a._z);
        }

        public static RestrictedVector3D operator *(double k, RestrictedVector3D a)
        {
            return new RestrictedVector3D(a._x * k, a._y * k, a._z);
        }

        public static RestrictedVector3D FromAngleAndHeight(double angle, int height)
        {
            return new RestrictedVector3D(Math.Cos(angle), Math.Sin(angle), height);
        }

        public RestrictedVector3D Rotate(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return new RestrictedVector3D(_x * cos - _y * sin, _x * sin + _y * cos, _z);
        }

        public PlanarVector ToPlanar()
        {
            return new PlanarVector(_x, _y);
        }
    }

    public class PlanarVector
    {
        public double _x = 0;
        public double _y = 0;

        public PlanarVector()
        {
        }

        public PlanarVector(double x, double y)
        {
            (_x, _y) = (x, y);
        }

        public static PlanarVector operator *(PlanarVector a, double k)
        {
            return new PlanarVector(a._x * k, a._y * k);
        }

        public static PlanarVector operator +(PlanarVector a, PlanarVector b)
        {
            return new PlanarVector(a._x + b._x, a._y + b._y);
        }

        public static PlanarVector operator -(PlanarVector a)
        {
            return new PlanarVector(-a._x, -a._y);
        }

        public AxisAlignedBb Expand(double width, double height)
        {
            return new AxisAlignedBb(_x, _y, _x + width, _y + height);
        }

        public AxisAlignedBb ExpandBothTo(double width, double height)
        {
            return new AxisAlignedBb(_x - width / 2, _y / height, _x + width / 2, _y + height / 2);
        }
    }

    public class AxisAlignedBb
    {
        public double _startX;
        public double _startY;
        public double _endX;
        public double _endY;

        public int StartXi => (int) _startX;
        public int StartYi => (int) _startY;
        public int EndXi => (int) _endX;
        public int EndYi => (int) _endY;

        public double Width => _endX - _startX;
        public double Height => _endY - _startY;

        public AxisAlignedBb(double startX, double startY, double endX, double endY)
        {
            (_startX, _startY, _endX, _endY) = (startX, startY, endX, endY);
            CheckCoordinates();
        }

        public AxisAlignedBb(PlanarVector start, PlanarVector end)
        {
            (_startX, _startY, _endX, _endY) = (start._x, start._y, end._x, end._y);
            CheckCoordinates();
        }

        public bool IntersectsWith(AxisAlignedBb box)
        {
            double cx1 = (_startX + _endX) / 2;
            double cy1 = (_startY + _endY) / 2;
            double cx2 = (box._startX + box._endX) / 2;
            double cy2 = (box._startY + box._endY) / 2;

            double halfWidth = _endX - cx1 + box._endX - cx2;
            double halfHeight = _endY - cy1 + box._endY - cy2;

            return Math.Abs(cx1 - cx2) < halfWidth || Math.Abs(cy1 - cy2) < halfHeight;
        }

        public static AxisAlignedBb operator +(AxisAlignedBb a, PlanarVector v)
        {
            return new AxisAlignedBb(a._startX + v._x, a._startY + v._y, a._endX + v._x, a._endY + v._y);
        }

        public void CheckCoordinates()
        {
            if (_startX > _endX)
            {
                (_startX, _endX) = (_endX, _startY);
            }

            if (_startY > _endY)
            {
                (_startY, _endY) = (_endY, _startY);
            }
        }
    }
}