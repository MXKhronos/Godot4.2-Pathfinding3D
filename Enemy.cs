using Godot;
using System;

public partial class Enemy : CharacterBody3D {
    public NavigationAgent3D navAgent;
    public float moveSpeed = 3f;

    public override void _Ready() {
        navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");

        AddToGroup("Enemies");
    }

    public override void _PhysicsProcess(double delta) {
        var currPos = GlobalTransform.Origin;
        var nextPos = navAgent.GetNextPathPosition();

        var newVel = (nextPos-currPos).Normalized() * moveSpeed;

        Velocity = newVel;
        MoveAndSlide();
    }

    public void UpdateTargetLocation(Vector3 targetPos) {
        navAgent.TargetPosition = targetPos;
    }
}
