/**
* This class defines the interface that a movement controller must implement.
* It takes a moveable agent to perform action on.
*/
using Godot;

public class MovementController(IMovableAgent agent, NavigationAgent2D navigationAgent2D)
{
   IMovableAgent _agent { get; } = agent;
   NavigationAgent2D _navigationAgent2D { get; } = navigationAgent2D;

   public Vector2 Target { set; get; }

   public void Process(double delta)
   {
      if (_navigationAgent2D.IsNavigationFinished())
         return;

      Vector2 next = _navigationAgent2D.GetNextPathPosition();
      Vector2 direction = next - _agent.GlobalPosition;
      if (direction.Length() < 0.1f && direction.Length() > -0.1f)
      {
         return;
      }

      Vector2 accelerationVector = CalculateTargetAccelerationVector(direction.Normalized());
      _agent.Velocity += accelerationVector * (float)delta;
      if (_agent.Velocity.Length() > _agent.MaxSpeed)
         _agent.Velocity = _agent.Velocity.Normalized() * _agent.MaxSpeed;

      _agent.Rotation = accelerationVector.Angle(); //SnapRotation(accelerationVector.Angle());

      _agent.MoveAndSlide();
   }

   private Vector2 CalculateTargetAccelerationVector(Vector2 nomalizedTarget)
   {
      Vector2 slide = _agent.Velocity.Slide(nomalizedTarget);
      Vector2 projection = _agent.Velocity.Project(nomalizedTarget);
      Vector2 alignedProjection = ParallelVectorsAreSameDirection(projection, nomalizedTarget) ? projection : -projection;
      Vector2 targetAcceleration = alignedProjection - slide;

      if (targetAcceleration.Length() >= _agent.MaxSpeed)
      {
         targetAcceleration = targetAcceleration.LimitLength(_agent.Acceleration);
      }
      else
      {
         float k = Mathf.Sqrt((_agent.Acceleration * _agent.Acceleration) - slide.LengthSquared());
         targetAcceleration = (Mathf.Max(k, 1f) * nomalizedTarget) - slide;
      }

      return targetAcceleration;
   }
   
   
   private static bool ParallelVectorsAreSameDirection(Vector2 a, Vector2 b)
   {
      return a.Dot(b) > 0;
   }
}
