using SFML.System;
using System;

namespace Game.ExtensionMethods
{
    public static class Vector2fExtensions
    {
        public static void Zero(this Vector2f v)
        {
            v.X = 0;
            v.Y = 0;
        }
        public static bool IsZero(this Vector2f v)
        {
            return v.X == 0 && v.Y == 0;
        }

        public static float Magnitude(this Vector2f v)
        {
            return MathExtensions.Sqrt(v.X * v.X + v.Y * v.Y);
        }

        public static float GetAngle(this Vector2f v)
        {
            return MathExtensions.Atan2(v.Y, v.X);
        }

        public static float Magnitude(this Vector2f v, Vector2f v2)
        {
            return MathExtensions.Sqrt(Math.Pow(v.X - v2.X, 2) + Math.Pow(v.Y - v2.Y, 2));
        }

        public static float LengthSq(this Vector2f v)
        {
            return v.X * v.X + v.Y * v.Y;
        }

        public static float MagnitudeSquared(this Vector2f v, Vector2f v2)
        {
            return v.X * v2.X + v.Y * v2.Y;
        }

        public static float MagnitudeSquared(this Vector2f v)
        {
            return v.X * v.X + v.Y * v.Y;
        }

        public static Vector2f Normalize(this Vector2f v)
        {
            var magnitude = v.Magnitude();
            if (magnitude == 0)
            {
                return new Vector2f();
            }

            return new Vector2f(v.X / magnitude, v.Y / magnitude);
        }

        public static void Scale(this Vector2f v, float scale)
        {
            v.X *= scale;
            v.Y *= scale;
        }

        public static float Dot(this Vector2f v, Vector2f vector)
        {
            return v.X * vector.X + v.Y * vector.Y;
        }

        public static Vector2f PerendicularClockwise(this Vector2f v)
        {
            return new Vector2f(v.Y, -v.X);
        }

        public static Vector2f PerendicularCounterClockwise(this Vector2f v)
        {
            return new Vector2f(-v.Y, v.X);
        }

        public static void Truncate(this Vector2f v, float max)
        {
            if (v.Magnitude() < max)
            {
                return;
            }

            var normalized = v.Normalize();
            v.X = normalized.X;
            v.Y = normalized.Y;

            v.Scale(max);
        }


        public static float Distance(this Vector2f v, Vector2f v2)
        {
            return MathExtensions.Sqrt(Math.Pow(v.X - v2.X, 2) + Math.Pow(v.Y - v2.Y, 2));
        }


        public static float DistanceSquared(this Vector2f v, Vector2f v2)
        {
            return (float)(Math.Pow(v.X - v2.X, 2) + Math.Pow(v.Y - v2.Y, 2));
        }

        public static Vector2f GetReverse(this Vector2f v)
        {
            return new Vector2f(-v.X, -v.Y);
        }
    }
}
