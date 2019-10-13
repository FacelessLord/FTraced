using System;
using System.Globalization;

namespace GlLib.Utils.Math
{
    public struct RestrictedVector3D
    {
        public bool Equals(RestrictedVector3D _other)
        {
            return z == _other.z &&
                   System.Math.Abs(x - _other.x) < 1e-3 &&
                   System.Math.Abs(y - _other.y) < 1e-3;
        }

        public override bool Equals(object _obj)
        {
            return _obj is RestrictedVector3D other && Equals(other);
        }

        public short z;
        public float x;
        public float y;


        public RestrictedVector3D(float _x = 0, float _y = 0, short _z = 0)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public int Ix => (int) MathF.Floor(x);
        public int Iy => (int) MathF.Floor(y);

        public static RestrictedVector3D operator +(RestrictedVector3D _a, RestrictedVector3D _b)
        {
            // TODO 
            // check this code
            if (_a.z != _b.z)
                throw new ArgumentException();
            return new RestrictedVector3D(_a.x + _b.x, _a.y + _b.y, _a.z);
        }

        public static RestrictedVector3D operator +(RestrictedVector3D _a, PlanarVector _b)
        {
            return new RestrictedVector3D(_a.x + _b.x, _a.y + _b.y, _a.z);
        }

        public static RestrictedVector3D operator -(RestrictedVector3D _a)
        {
            return new RestrictedVector3D(-_a.x, -_a.y, _a.z);
        }

        public AxisAlignedBb ExpandBothTo(float _width, float _height)
        {
            return new AxisAlignedBb(x - _width, y - _height, x + _width, y + _height);
        }

        public static bool operator ==(RestrictedVector3D _a, RestrictedVector3D _b)
        {
            return _b.Equals(_a);
        }

        public static bool operator !=(RestrictedVector3D _a, RestrictedVector3D _b)
        {
            return !_b.Equals(_a);
        }

        public static RestrictedVector3D operator -(RestrictedVector3D _a, PlanarVector _b)
        {
            return _a + -_b;
        }

        public static RestrictedVector3D operator -(RestrictedVector3D _a, RestrictedVector3D _b)
        {
            return _a + -_b;
        }

        public static RestrictedVector3D operator *(RestrictedVector3D _a, float _k)
        {
            return new RestrictedVector3D(_a.x * _k, _a.y * _k, _a.z);
        }

        public static RestrictedVector3D operator *(float _k, RestrictedVector3D _a)
        {
            return new RestrictedVector3D(_a.x * _k, _a.y * _k, _a.z);
        }

        public static RestrictedVector3D operator /(RestrictedVector3D _a, float _k)
        {
            return new RestrictedVector3D(_a.x / _k, _a.y / _k, _a.z);
        }


        public static RestrictedVector3D FromAngleAndHeight(float _angle, short _height)
        {
            return new RestrictedVector3D(MathF.Cos(_angle), MathF.Sin(_angle), _height);
        }

        public RestrictedVector3D Rotate(float _angle)
        {
            return new RestrictedVector3D(x * MathF.Cos(_angle) - y * MathF.Sin(_angle),
                x * MathF.Sin(_angle) + y * MathF.Cos(_angle), z);
        }


        public PlanarVector ToPlanarVector()
        {
            return new PlanarVector(x, y);
        }

        public static implicit operator PlanarVector(RestrictedVector3D v)
        {
            return v.ToPlanarVector();
        }

        public override string ToString()
        {
            return $"({x.ToString(CultureInfo.InvariantCulture)}," +
                   $"{y.ToString(CultureInfo.InvariantCulture)}," +
                   $"{z.ToString(CultureInfo.InvariantCulture)})";
        }

        public static RestrictedVector3D FromString(string _s)
        {
            if (_s == "")
                return new RestrictedVector3D();
            var coords = _s.Substring(1, _s.Length - 2).Split(",");
            return new RestrictedVector3D(
                float.Parse(coords[0], NumberStyles.Any, CultureInfo.InvariantCulture),
                float.Parse(coords[1], NumberStyles.Any, CultureInfo.InvariantCulture),
                short.Parse(coords[2], NumberStyles.Any, CultureInfo.InvariantCulture));
        }


        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z;
                return hashCode;
            }
        }

        public float Angle => MathF.Atan2(y, x);

        public float Length => MathF.Sqrt(x * x + y * y + z * z);
    }
}