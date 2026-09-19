using Godot;

public partial class NpcController : CharacterBody2D
{
    [Export] public NpcDefinition Definition { get; set; }
    [Export] public NavigationAgent2D NavigationAgent { get; set; }
    private bool _hasMovementTarget;

    public override void _Ready()
    {
        NavigationAgent ??= GetNode<NavigationAgent2D>("NavigationAgent2D");
        NavigationAgent.PathDesiredDistance = 4.0f;
        NavigationAgent.TargetDesiredDistance = 4.0f;
        MotionMode = MotionModeEnum.Floating;
        // Clock (0), runner (1), then movement (2), in the same physics tick.
        ProcessPhysicsPriority = 2;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (DialogueManager.IsDialogueActive)
        {
            StopMoving();
            return;
        }

        if (!_hasMovementTarget)
            return;

        // A newly assigned target needs a path update before checking completion.
        if (NavigationServer2D.MapGetIterationId(NavigationAgent.GetNavigationMap()) == 0)
            return;

        Vector2 next = NavigationAgent.GetNextPathPosition();
        if (NavigationAgent.IsNavigationFinished())
        {
            StopMoving();
            return;
        }

        float speed = Mathf.Max(0, Definition?.DefaultSpeed ?? 80.0f);
        // Avoid overshooting a nearby waypoint at low frame rates.
        if (delta > 0)
            speed = Mathf.Min(speed, GlobalPosition.DistanceTo(next) / (float)delta);
        Velocity = GlobalPosition.DirectionTo(next) * speed;
        MoveAndSlide();
    }

    public void MoveTo(Vector2 destination)
    {
        NavigationAgent.TargetPosition = destination;
        _hasMovementTarget = true;
    }

    public void StopMoving()
    {
        _hasMovementTarget = false;
        Velocity = Vector2.Zero;
    }
}
