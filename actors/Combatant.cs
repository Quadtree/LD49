using Godot;
using System;

public class Combatant : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    bool MoveLeft = false;
    bool MoveRight = false;

    [Export]
    public bool IsPlayerControlled;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var wheelJoint = this.FindChildByType<HingeJoint>();

        float targetSpeed = 0f;
        if (MoveLeft && !MoveRight) targetSpeed = -1f;
        if (!MoveLeft && MoveRight) targetSpeed = 1f;

        wheelJoint.Motor__enable = true;
        wheelJoint.Motor__maxImpulse = 100f;
        wheelJoint.Motor__targetVelocity = -targetSpeed * 4;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (IsPlayerControlled)
        {
            if (@event.IsActionPressed("move_left")) MoveLeft = true;
            if (@event.IsActionReleased("move_left")) MoveLeft = false;
            if (@event.IsActionPressed("move_right")) MoveRight = true;
            if (@event.IsActionReleased("move_right")) MoveRight = false;
        }
    }
}
