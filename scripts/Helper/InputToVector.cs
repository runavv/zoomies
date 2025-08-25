using Godot;

public static class InputToVector
{
    public static Vector2 GetDirection()
    {
        Vector2 direction = Vector2.Zero;
        
        if (Input.IsActionPressed("Up"))
         direction.Y -= 1;
        if (Input.IsActionPressed("Down"))
            direction.Y += 1;
        if (Input.IsActionPressed("Left"))
            direction.X -= 1;
        if (Input.IsActionPressed("Right"))
            direction.X += 1;
        return direction.LengthSquared() > 0 ? direction.Normalized() : Vector2.Zero;
    }
}
