using Godot;
using System;

public partial class Player : CharacterBody2D
{
   float _maxSpeed = 250f;
   float _acceleration = 2000f;
   float _friction = 750f;
   float _dashSpeed = 500;

   public override void _Ready()
   {
   }

   public override void _Process(double delta)
   {
      ApplyFriction(delta);
      Vector2 direction = InputToVector.GetDirection();
      
      // Dash
      if (Input.IsActionPressed("Space") && Velocity.Length() <= _maxSpeed)
         Velocity = direction * _dashSpeed;

      // Move
      Accelerate(direction, delta);
      MoveAndSlide();
   }

   private void ApplyFriction(double delta)
   {
      float length = Velocity.Length();
      float friction = _friction * (float)delta;
      if (length < friction)
      {
         Velocity = Vector2.Zero;
      }
      else
      {
         Velocity = Velocity.Normalized() * (length - friction);
      }
   }

   private void Accelerate(Vector2 direction, double delta)
   {
      if (Velocity.Length() > _maxSpeed)
      {
         return;
      }
      float deltaAcceleration = _acceleration * (float)delta; 
      Velocity += direction * Mathf.Min(deltaAcceleration, Mathf.Max(0, _maxSpeed-Velocity.Length()));
      Velocity = Velocity.Length() > _maxSpeed ? Velocity.Normalized() * _maxSpeed : Velocity;
   }
}
