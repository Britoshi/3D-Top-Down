using System;

namespace Game
{
    public readonly struct Vec2 : IComparable<Vec2>, IEquatable<Vec2>
    {
        private readonly int _x;
        private readonly int _y;

        public readonly int x
        {
            set => new Vec2(value, _y);
            get => _x;
        }
        public readonly int y
        {
            set => new Vec2(_x, value);
            get => _y;
        }

        public Vec2(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public Vec2(Vec2? clone = null)
        {
            if (clone == null)
            {
                _x = 0;
                _y = 0;
            }
            else
            {
                _x = clone.Value.x;
                _y = clone.Value.y;
            }
        }

        public readonly int CompareTo(Vec2 other)
        {
            var cX = x.CompareTo(other.x);
            if (cX != 0) return cX;
            return y.CompareTo(other.y);
        }

        public readonly bool Equals(Vec2 other) => x == other.x && y == other.y;

        public static Vec2 operator -(Vec2 self) => new Vec2(-self.x, -self.y);
        public static Vec2 operator +(Vec2 self, Vec2 other) => new Vec2(self.x + other.x, self.y + other.y);
        public static Vec2 operator -(Vec2 self, Vec2 other) => new Vec2(self.x - other.x, self.y - other.y);
        public static Vec2 operator *(Vec2 self, Vec2 other) => new Vec2(self.x * other.x, self.y * other.y);
        public static Vec2 operator *(Vec2 self, int value) => new Vec2(self.x * value, self.y * value);
        public static Vec2 operator *(int value, Vec2 self) => new Vec2(self.x * value, self.y * value);
        public static Vec2 operator /(Vec2 self, Vec2 other) => new Vec2(self.x / other.x, self.y / other.y);
        public static Vec2 operator /(Vec2 self, int value) => new Vec2(self.x / value, self.y / value);

        public override string ToString() => "Vec2(" + x + ", " + y + ")";
    }
}