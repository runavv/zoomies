/**
* This Interface defines the core capabilities and values that a thing that can move itself can do.
* 
*/

using Godot;

public interface IMovableAgent
{
   float MaxSpeed { get; }
   float Acceleration { get; }
   int AngleIncrementCount { get; }
   float Rotation { get; set; }
   Vector2 Velocity { get; set; }
   Vector2 GlobalPosition { get; }
   bool MoveAndSlide();
}
