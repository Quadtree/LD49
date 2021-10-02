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
        float targetSpeed = 0f;
        if (MoveLeft && !MoveRight) targetSpeed = -1f;
        if (!MoveLeft && MoveRight) targetSpeed = 1f;

        var wheelJoint = this.FindChildByType<Generic6DOFJoint>();
        wheelJoint.AngularMotorZ__enabled = true;
        wheelJoint.AngularMotorZ__forceLimit = 1000f;
        wheelJoint.AngularMotorZ__targetVelocity = -targetSpeed * 4;

        var cam = GetViewport().GetCamera();

        var raySrc = cam.ProjectRayOrigin(GetViewport().GetMousePosition());
        var rayNorm = cam.ProjectRayNormal(GetViewport().GetMousePosition());
        var rayTo = raySrc + rayNorm * 100;

        var pos = new Vector3();

        var curPos = GetWorld().DirectSpaceState.IntersectRay(raySrc, rayTo);

        if (curPos.Contains("position"))
        {
            pos = (Vector3)curPos["position"];
            Console.WriteLine(pos);

            
        }
        else
        {
            Console.WriteLine("HIT NOTHING");
        }


        /*
        var wheels = this.FindChildByName<RigidBody>("Wheels");
        var body = this.FindChildByName<RigidBody>("Body");

        wheels.ApplyTorqueImpulse(new Vector3(0, 0, targetSpeed * -0.05f));
        body.ApplyImpulse(wheels.GetGlobalLocation() - body.GetGlobalLocation(), new Vector3(targetSpeed * 1f, 0, 0));
        */
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
