using Godot;
using System;

public partial class World : Node {
    public CharacterBody3D player;
    public override void _Ready() {
        player = GetNode<CharacterBody3D>("Player");

        GetTree().CallGroupFlags((long)SceneTree.GroupCallFlags.Deferred, "Enemies", "UpdateTargetLocation", player.GlobalTransform.Origin);
    }
}
