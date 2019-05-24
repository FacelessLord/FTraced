using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GlLib.Utils
{
    public struct PlanarVector
    {
        public bool Equals(PlanarVector _other)
        {
            return Math.Abs(x - _other.x) < 1e-3 &&
                   Math.Abs(y - _other.y) < 1e-3;
        }

        public override bool Equals(object _obj)
        {
            return _obj is PlanarVector other && Equals(other);
        }

        public float x;
        public float y;


        public PlanarVector(float _x = 0, float _y = 0)
        {
            (x, y) = (_x, _y);
        }

        public float Angle => MathF.Atan2(y, x);

        public float Length => MathF.Sqrt(x * x + y * y);

        public PlanarVector Normalized
        {
            get
            {
                if (Math.Abs(Length) < 1e-4) return new PlanarVector(0);

                var newX =(float) Math.Round(x * 1 / Length);
                var newY =(float) Math.Round(y * 1 / Length);
                return new PlanarVector(newX, newY);
            }
        }

        public static PlanarVector GetRandom(float _range)
        {
            var r = new Random();

            return new PlanarVector(
                (2 * (float) r.NextDouble() - 1) * _range,
                (2 * (float) r.NextDouble() - 1) * _range);
        }

        public static PlanarVector operator *(PlanarVector _a, float _k)
        {
            return new PlanarVector(_a.x * _k, _a.y * _k);
        }

        public static PlanarVector operator /(PlanarVector _a, float _k)
        {
            return new PlanarVector(_a.x / _k, _a.y / _k);
        }

        public static PlanarVector operator +(PlanarVector _a, PlanarVector _b)
        {
            return new PlanarVector(_a.x + _b.x, _a.y + _b.y);
        }

        public static PlanarVector operator -(PlanarVector _a, PlanarVector _b)
        {
            _b *= -1;
            return new PlanarVector(_a.x + _b.x, _a.y + _b.y);
        }

        public static PlanarVector operator -(RestrictedVector3D _a, PlanarVector _b)
        {
            _b *= -1;
            return new PlanarVector(_a.x + _b.x, _a.y + _b.y);
        }

        public static PlanarVector operator -(PlanarVector _a)
        {
            return new PlanarVector(-_a.x, -_a.y);
        }

        public static bool operator ==(PlanarVector _a, PlanarVector _b)
        {
            return _a.Equals(_b);
        }

        public static bool operator !=(PlanarVector _a, PlanarVector _b)
        {
            return !_a.Equals(_b);
        }

        public override string ToString()
        {
            return $"({x.ToString(CultureInfo.InvariantCulture)}," +
                   $"{y.ToString(CultureInfo.InvariantCulture)})";
        }

        public void Normalize()
        {
            if (Math.Abs(Length) < 1e-4)
            {
                x = 0;
                y = 0;
                return;
            }

            x = (float) Math.Round(x * 1 / Length);
            y = (float) Math.Round(y * 1 / Length);
        }

        public static PlanarVector FromString(string _s)
        {
            if (_s == "")
                return new PlanarVector();
            var coords = _s.Substring(1, _s.Length - 2).Split(",");
            return new PlanarVector(
                float.Parse(coords[0], NumberStyles.Any, CultureInfo.InvariantCulture),
                float.Parse(coords[1], NumberStyles.Any, CultureInfo.InvariantCulture));
        }

        public AxisAlignedBb Expand(double _width, double _height)
        {
            return new AxisAlignedBb(x, y, x + _width, y + _height);
        }


        public override int GetHashCode()
        {
            unchecked
            {
                return (x.GetHashCode() * 397) ^ y.GetHashCode();
            }
        }

        public AxisAlignedBb ExpandBothTo(float _width, float _height)
        {
            return new AxisAlignedBb(x - _width, y - _height, x + _width, y + _height);
        }

        public PlanarVector Divide(float _i)
        {
            x /= _i;
            y /= _i;
            return this;
        }

        public PlanarVector Divide(float _i, float _j)
        {
            x /= _i;
            y /= _j;
            return this;
        }
    }
}
