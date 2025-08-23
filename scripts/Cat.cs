// using Godot;
// using System;

// public partial class Cat : CharacterBody2D, IMovableAgent
// {
//    [Export] public float MaxSpeed { get; set; } //300
//    [Export] public float Acceleration { get; set; } //75
//    [Export] public int AngleIncrementCount { get; set; } //= 8; // number of rotation steps

//    private MovementController _movementController;

//    public override void _Ready()
//    {
// 	  _movementController = new(this, GetNode<NavigationAgent2D>("NavigationAgent2D")); 
//    }

//    public override void _Process(double delta)
//    {
// 	  // _agent target Does not need to be modified every frame
// 	  // but for this demo it chases the mouse, as all good cats should.
// 	  Vector2 mousePos = GetGlobalMousePosition();
// 	  _movementController.Target = mousePos;

// 	  _movementController.Process(delta);
//    }

//    private float SnapRotation(float targetAngle)
//    {
// 	  float slice = Mathf.Tau / AngleIncrementCount;
// 	  return Mathf.Round(targetAngle / slice) * slice;
//    }
// }


using Godot;
using System;

public partial class Cat : CharacterBody2D
{
   [Export] public float MaxSpeed = 300f;
   [Export] public float Acceleration = 750f;
   [Export] public int AngleIncrementCount = 8; // number of rotation steps

   private NavigationAgent2D _agent;

   public override void _Ready()
   {
      _agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
   }

   public override void _Process(double delta)
   {
      // For this demo, it chases the mouse, as all good cats should.
      Vector2 mousePos = GetGlobalMousePosition();
      _agent.TargetPosition = mousePos;

      if (_agent.IsNavigationFinished())
         return;

      Vector2 next = _agent.GetNextPathPosition();
      Vector2 direction = next - GlobalPosition;
      if (direction.Length() < 0.1f && direction.Length() > -0.1f)
      {
         return;
      }
      Vector2 accelerationVector = CalculateTargetAccelerationVector(direction.Normalized());
      Velocity += accelerationVector * (float)delta;
      if (Velocity.Length() > MaxSpeed)
         Velocity = Velocity.Normalized() * MaxSpeed;

      Rotation = accelerationVector.Angle();//SnapRotation(accelerationVector.Angle());

      MoveAndSlide();
   }

   private float SnapRotation(float targetAngle)
   {
      float slice = Mathf.Tau / AngleIncrementCount;
      return Mathf.Round(targetAngle / slice) * slice;
   }

   private Vector2 CalculateTargetAccelerationVector(Vector2 nomalizedTarget)
   {
      Vector2 slide = Velocity.Slide(nomalizedTarget.Normalized());
      Vector2 projection = Velocity.Project(nomalizedTarget);
      Vector2 alignedProjection = ParallelAreSameDirection(projection, nomalizedTarget) ? projection : -projection;
      Vector2 targetAcceleration = alignedProjection - slide;

      if (targetAcceleration.Length() >= MaxSpeed)
      {
         targetAcceleration = targetAcceleration.LimitLength(Acceleration);
      }
      else
      {
         float k = Mathf.Sqrt((Acceleration * Acceleration) - slide.LengthSquared());
         targetAcceleration = (Mathf.Max(k, 1f) * nomalizedTarget) - slide;
      }

      return targetAcceleration;
   }

   private static bool ParallelAreSameDirection(Vector2 a, Vector2 b)
   {
      return a.Dot(b) > 0;
   }
}


// using Godot;
// using System;

// public partial class Cat : CharacterBody2D
// {
// 	[Export] public float Speed = 300f;
// 	[Export] public float Acceleration = 750f;
// 	[Export] public int AngleIncrementCount = 8; // number of rotation steps

// 	private NavigationAgent2D _agent;

// 	public override void _Ready()
// 	{
// 		_agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
// 	}

// 	public override void _Process(double delta)
// 	{
// 		Vector2 mousePos = GetGlobalMousePosition();
// 		GD.Print(mousePos);
// 		_agent.TargetPosition = mousePos;

// 		if (_agent.IsNavigationFinished())
// 			return;

// 		Vector2 next = _agent.GetNextPathPosition();
// 		Vector2 direction = (next - GlobalPosition).Normalized();
// 		Velocity += direction * Acceleration * (float)delta;
// 		if (Velocity.Length() > Speed)
// 			Velocity = Velocity.Normalized() * Speed;
		
// 		Rotation = SnapRotation(direction.Angle());

// 		MoveAndSlide();
// 	}
	
// 	private float SnapRotation(float targetAngle)
// 	{
// 		float slice = Mathf.Tau / AngleIncrementCount;
// 		return Mathf.Round(targetAngle / slice) * slice;
// 	}
// }
