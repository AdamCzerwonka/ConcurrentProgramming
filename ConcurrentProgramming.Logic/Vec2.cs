namespace ConcurrentProgramming.Logic;

public struct Vec2
{
    public Vec2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X;
    public int Y;

    public static Vec2 operator +(Vec2 a, Vec2 b)
    {
        return new Vec2(a.X + b.X, a.Y + b.Y);
    }
}