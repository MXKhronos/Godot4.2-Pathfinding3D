using Godot;
using Godot.Collections;
using System;

public partial class PlayerControl : Camera3D {
    public CharacterBody3D player;
    public Rid playerCollisionRid;
    public float rayLength = 1024;
    public float moveSpeed = 14f;

    private Vector3 clampRangeMax = new(float.MaxValue, 0, float.MaxValue);
    private Vector3 clampRangeMin = new(float.MinValue, 0, float.MinValue);
    private Vector3 gravity;

    public override void _Ready() {
        gravity = (Vector3)ProjectSettings.GetSetting("physics/3d/default_gravity_vector") * (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
        player = GetParent().GetNode<CharacterBody3D>("Player");
        playerCollisionRid = player.GetRid();
    }

    public override void _PhysicsProcess(double delta) {
        if (player == null) return;
        
        var mousePos = GetViewport().GetMousePosition();
        var playerPos = player.GlobalTransform.Origin;

        var rayOrigin = ProjectRayOrigin(mousePos);
        var rayTarget = rayOrigin + ProjectRayNormal(mousePos) * rayLength;

        var spaceState = GetWorld3D().DirectSpaceState;
        var rayQuery = PhysicsRayQueryParameters3D.Create(rayOrigin, rayTarget);
        rayQuery.Exclude = new Array<Rid>(){playerCollisionRid};
        var rayResult = spaceState.IntersectRay(rayQuery);

        if (!rayResult.ContainsKey("position")) return;
        var groundPos = (Vector3) rayResult["position"];

        var dirVel = Vector3.Zero;
        if (groundPos.DistanceSquaredTo(playerPos) > 0.01f) {
            dirVel = (groundPos.Clamp(clampRangeMin, clampRangeMax) - playerPos.Clamp(clampRangeMin, clampRangeMax)).Normalized();
        }

        var newVel = dirVel * moveSpeed;
        newVel += gravity;

        player.Velocity = newVel;
        player.MoveAndSlide();
    }
}
