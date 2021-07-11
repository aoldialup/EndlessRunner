namespace WalmartEngine
{
    class Vector2
    {
        public float x { get; }
        public float y { get; }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 zero
        {
            get
            {
                return new Vector2(0f, 0f);
            }
        }

        public static Vector2 one
        {
            get
            {
                return new Vector2(0f, 1f);
            }
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
    => new Vector2(a.x + b.x, a.y + b.y);

        public static Vector2 operator -(Vector2 a, Vector2 b)
    => new Vector2(a.x - b.x, a.y - b.y);

        public static Vector2 operator *(Vector2 v, float scalar)
    => new Vector2(v.x * scalar, v.y * scalar);

        public static Vector2 operator /(Vector2 v, float scalar)
    => new Vector2(v.x / scalar, v.y / scalar);

        public static Vector2 operator *(Vector2 v, Vector2 u)
    => new Vector2(v.x * u.x, v.y * u.y);

        public override string ToString()
        {
            return $"X: {x} Y: {y}";
        }
    }
}