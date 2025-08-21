using Godot;
using System;

public partial class Cat : CharacterBody2D
{
	[Export] public float Speed = 300f;
	[Export] public float Acceleration = 750f;
	[Export] public int AngleIncrementCount = 8; // number of rotation steps

	private NavigationAgent2D _agent;

	public override void _Ready()
	{
		_agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
	}

	public override void _Process(double delta)
	{
		Vector2 mousePos = GetGlobalMousePosition();
		GD.Print(mousePos);
		_agent.TargetPosition = mousePos;

		if (_agent.IsNavigationFinished())
			return;

		Vector2 next = _agent.GetNextPathPosition();
		Vector2 direction = (next - GlobalPosition).Normalized();
		Velocity += direction * Acceleration * (float)delta;
		if (Velocity.Length() > Speed)
			Velocity = Velocity.Normalized() * Speed;
		
		Rotation = SnapRotation(direction.Angle());

		MoveAndSlide();
	}
	
	private float SnapRotation(float targetAngle)
	{
		float slice = Mathf.Tau / AngleIncrementCount;
		return Mathf.Round(targetAngle / slice) * slice;
	}
}
