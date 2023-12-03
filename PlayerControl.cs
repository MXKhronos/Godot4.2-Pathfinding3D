using Godot;
using System;

public partial class PlayerControl : Camera3D {
    public CharacterBody3D player;

    public override void _Ready() {
        GetParent().GetNode<CharacterBody3D>("Player");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }
}
