using Godot;
using System;

public partial class Enemy : CharacterBody3D {
    public NavigationAgent3D navAgent;
    public float moveSpeed = 5f;
    private Vector3 gravity;

    public override void _Ready() {
        gravity = (Vector3)ProjectSettings.GetSetting("physics/3d/default_gravity_vector") * (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
        navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");

        AddToGroup("Enemies");
        navAgent.Connect("velocity_computed", new Callable(this, "OnNavAgentVelComputed"));

        moveSpeed = GD.RandRange(6, 11);
    }

    public override void _PhysicsProcess(double delta) {
        var currPos = GlobalTransform.Origin;
        var nextPos = navAgent.GetNextPathPosition();

        var newVel = (nextPos-currPos).Normalized() * moveSpeed;

        navAgent.Velocity = newVel;
        // Velocity = Velocity.MoveToward(newVel, 0.9f);
        // MoveAndSlide();
    }

    public void OnNavAgentVelComputed(Vector3 safeVel) {
        Velocity = Velocity.MoveToward(safeVel, 0.9f);
        Velocity += gravity;

        MoveAndSlide();
    }

    public void UpdateTargetLocation(Vector3 targetPos) {
        navAgent.TargetPosition = targetPos;
    }
}
