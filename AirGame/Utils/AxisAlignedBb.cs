using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GlLib.Utils
{
    public class AxisAlignedBb
    {
        public double endX;
        public double endY;
        public double startX;
        public double startY;

        public AxisAlignedBb(double _startX, double _startY, double _endX, double _endY)
        {
            (startX, startY, endX, endY) =
                (_startX, _startY, _endX, _endY);
            CheckCoordinates();
        }

        public AxisAlignedBb(PlanarVector _start, PlanarVector _end)
        {
            (startX, startY, endX, endY) = (_start.x, _start.y, _end.x, _end.y);
            CheckCoordinates();
        }

        public int StartXi => (int)startX;
        public int StartYi => (int)startY;
        public int EndXi => (int)endX;
        public int EndYi => (int)endY;

        public double Width => endX - startX;
        public double Height => endY - startY;

        public int WidthI => EndXi - StartXi;
        public int HeightI => EndYi - StartYi;

        public bool IsVectorInside(PlanarVector _vector)
        {
            return _vector.x <= endX && _vector.x >= startX && _vector.y <= endY && _vector.y >= startY;
        }

        public bool IsVectorInside(double _x, double _y)
        {
            return _x <= endX && _x >= startX && _y <= endY && _y >= startY;
        }

        public bool IntersectsWith(AxisAlignedBb _box)
        {
            var cx1 = (startX + endX) / 2;
            var cy1 = (startY + endY) / 2;
            var cx2 = (_box.startX + _box.endX) / 2;
            var cy2 = (_box.startY + _box.endY) / 2;

            var halfWidth = Width / 2 + _box.Width / 2;
            var halfHeight = Height / 2 + _box.Height / 2;

            //if(Math.Abs(cx1 - cx2) <= halfWidth && Math.Abs(cy1 - cy2) <= halfHeight)
            //    SidedConsole.WriteLine(this + " | " + _box);
            //TODO it's magic check please
            return Math.Abs(cx1 - cx2) <= halfWidth * 1.5 && Math.Abs(cy1 - cy2) <= halfHeight * 1.5;
        }

        public static AxisAlignedBb operator +(AxisAlignedBb _a, PlanarVector _v)
        {
            return new AxisAlignedBb(_a.startX + _v.x, _a.startY + _v.y, _a.endX + _v.x, _a.endY + _v.y);
        }

        public static AxisAlignedBb operator +(AxisAlignedBb _a, RestrictedVector3D _v)
        {
            return new AxisAlignedBb(_a.startX + _v.x, _a.startY + _v.y, _a.endX + _v.x, _a.endY + _v.y);
        }

        public AxisAlignedBb Translate(PlanarVector _v)
        {
            startX += _v.x;
            startY += _v.y;
            endX += _v.x;
            endY += _v.y;
            return this;
        }

        public AxisAlignedBb Scaled(PlanarVector _v, double _s)
        {
            if (_v.x > 0)
            {
                startX += _v.x / _s;
                endX += _v.x * _s;
            }
            else
            {
                startX += _v.x * _s;
                endX += _v.x / _s;
            }

            if (_v.y > 0)
            {
                startY += _v.y / _s;
                endY += _v.y * _s;
            }
            else
            {
                startY += _v.y * _s;
                endY += _v.y / _s;
            }

            return this;
        }

        public override bool Equals(object _obj)
        {
            var item = _obj as AxisAlignedBb;
            if (item is null)
                return false;
            return Equals(item);
        }

        protected bool Equals(AxisAlignedBb _item)
        {
            return Math.Abs(_item.startX - startX) < 1e-3
                   && Math.Abs(_item.startY - startY) < 1e-3
                   && Math.Abs(_item.endX - endX) < 1e-3
                   && Math.Abs(_item.endY - endY) < 1e-3;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = endX.GetHashCode();
                hashCode = (hashCode * 397) ^ endY.GetHashCode();
                hashCode = (hashCode * 397) ^ startX.GetHashCode();
                hashCode = (hashCode * 397) ^ startY.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"Box ({startX.ToString(CultureInfo.InvariantCulture)}," +
                   $"{startY.ToString(CultureInfo.InvariantCulture)}," +
                   $"{endX.ToString(CultureInfo.InvariantCulture)}," +
                   $"{endY.ToString(CultureInfo.InvariantCulture)})";
        }

        private void CheckCoordinates()
        {
            if (startX > endX) (startX, endX) = (endX, startY);

            if (startY > endY) (startY, endY) = (endY, startY);
        }
    }
}
