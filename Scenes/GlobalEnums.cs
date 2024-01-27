using Godot;
using System;

public enum Color {
    None,
    Type1,
    Type2,
    Type3,
    Type4,
    Type5,
    Max
}
public enum Shape {
    None,
    Circle,
    Triangle,
    Square,
    Max
}

public class GlobalEnums : Node
{
    public static Shape GetRandomShape()
    {
        var random = new Random();
        return (Shape) random.Next(1, (int)Shape.Max);
    }
    
    public static Color GetRandomColor()
    {
        var random = new Random();
        return (Color) random.Next(1, (int)Color.Max);
    }
}
