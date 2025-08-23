using Godot;
using System;

public partial class AngleVisualizer : Node2D
{
   // float MaxSpeed = 30;
   // float Accelleration = 100;
   // Vector2 Origin = new(0, 0);
   // Vector2 TargetNormalized = new(0, 1);
   // Vector2 Velocity = new(0, 0);

   // Vector2 CorrectedTargetGreen = new(0, 0);
   // Vector2 CorrectedTargetBlue = new(0, 0);
   // Vector2 CorrectedTargetYellow = new(0, 0);
   // Vector2 Flag = new(0, 0);

   /*
   At least one should be normalized
   */
   private bool ParallelAreSameDirection(Vector2 a, Vector2 b)
   {
	  return a.Dot(b) > 0;
   }
   
   public override void _Draw()
   {
	//   Flag = new(0, 0);
	//   Velocity = GetGlobalMousePosition();

	//   //Do stff
	//   CorrectedTargetGreen = Velocity.Slide(TargetNormalized);  // perp
	//   CorrectedTargetBlue = Velocity.Project(TargetNormalized);  // para
	//   Vector2 AlignedBlue = ParallelAreSameDirection(CorrectedTargetBlue, TargetNormalized) ? CorrectedTargetBlue : -CorrectedTargetBlue;
	//   CorrectedTargetYellow = -CorrectedTargetGreen;
	//   CorrectedTargetYellow += AlignedBlue;
	//   if (CorrectedTargetYellow.Length() >= Accelleration)
	//   {
	// 	 Flag = new(200, 200);
	// 	 CorrectedTargetYellow = CorrectedTargetYellow.LimitLength(Accelleration);
	//   }
	//   else
	//   {
	// 	 float k = Mathf.Sqrt((Accelleration*Accelleration) - CorrectedTargetGreen.LengthSquared());
	// 	 CorrectedTargetYellow = (Mathf.Max(k,1f) * TargetNormalized) - CorrectedTargetGreen;
	// 	 GD.Print(k);
	//   }

	//   float margin = 0.1f;
	//   if (CorrectedTargetYellow.Length() < Accelleration - margin || CorrectedTargetYellow.Length() > Accelleration + margin)
	//   {
	// 	 Flag = new(-200, -200);
	//   }


	//   //Display
	//   DrawLine(Origin, TargetNormalized * 100, Colors.White, 1.0f);
	//   DrawLine(Origin, Velocity, Colors.Red, 1.0f);
	//   DrawLine(Origin, CorrectedTargetGreen, Colors.Green, 1.0f);
	//   DrawLine(Origin, CorrectedTargetBlue, Colors.Blue, 1.0f);
	//   DrawLine(Origin, CorrectedTargetYellow, Colors.Yellow, 1.0f);
   }

   Vector2 CalculateTargetAccelerationVector(float maxSpeed, float acceleration, Vector2 nomalizedTarget, Vector2 velocity)
   {
	  Vector2 slide = velocity.Slide(nomalizedTarget);
	  Vector2 projection = velocity.Project(nomalizedTarget);
	  Vector2 alignedProjection = ParallelAreSameDirection(projection, nomalizedTarget) ? projection : -projection;
	  Vector2 targetAcceleration = alignedProjection - slide;

	  if (targetAcceleration.Length() >= maxSpeed)
	  {
		 targetAcceleration = targetAcceleration.LimitLength(acceleration);
	  }
	  else
	  {
		 float k = Mathf.Sqrt((acceleration * acceleration) - slide.LengthSquared());
		 targetAcceleration = (Mathf.Max(k, 1f) * nomalizedTarget) - slide;
	  }

	  return targetAcceleration;
   }

   public override void _Input(InputEvent @event)
   {
	  // if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
	  // {
	  //    GD.Print($"Mouse clicked at: {mouseEvent.Position}");
	  //    TargetNormalized = TargetNormalized.Rotated(Mathf.DegToRad(30));
	  // }
   }

   public override void _Ready()
   {
   }

   public override void _Process(double delta)
   {
	  QueueRedraw();
   }
}
