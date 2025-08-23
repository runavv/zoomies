using Godot;
using System;

public partial class Cat : CharacterBody2D
{
	[Export] public float Speed = 300f;
	[Export] public float Acceleration = 750f;
	[Export] public int AngleIncrementCount = 8; // number of rotation steps

	private NavigationAgent2D _agent;
	private AnimatedSprite2D _animatedSprite;
	private Vector2 _currentVelocity;
	
	public override void _Ready()
	{
		_agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedCatSprite2D");
		_currentVelocity = Velocity;
	}

	public override void _Process(double delta)
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		
		_currentVelocity = Velocity;
		_agent.TargetPosition = mousePosition;
		_animatedSprite.GlobalRotation = 0;
		_animatedSprite.FlipH = mousePosition.X < GlobalPosition.X;

		if (_agent.IsNavigationFinished())
		{
			_animatedSprite.Play("idle");
			return;
		}

		_animatedSprite.Play("run");
		Vector2 next = _agent.GetNextPathPosition();
		Vector2 direction = (next - GlobalPosition).Normalized();
		Velocity += direction * Acceleration * (float)delta;
		
		if (Velocity.Length() > Speed)
		{
			Velocity = Velocity.Normalized() * Speed;
		}
		
		Rotation = SnapRotation(direction.Angle());
		MoveAndSlide();
	}
	
	private float SnapRotation(float targetAngle)
	{
		float slice = Mathf.Tau / AngleIncrementCount;
		return Mathf.Round(targetAngle / slice) * slice;
	}
}
