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

    private Vector3 ArmJointRelative = new Vector3(-1000000, 0, 0);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (ArmJointRelative.x < -10000)
        {
            var armJoint = this.FindChildByName<Generic6DOFJoint>("ArmJoint");
            var body = this.FindChildByName<RigidBody>("Body");

            ArmJointRelative = armJoint.Translation - body.Translation;
        }

        float targetSpeed = 0f;
        if (MoveLeft && !MoveRight) targetSpeed = -1f;
        if (!MoveLeft && MoveRight) targetSpeed = 1f;

        var wheelJoint = this.FindChildByName<Generic6DOFJoint>("Generic6DOFJoint");
        wheelJoint.AngularMotorZ__enabled = true;
        wheelJoint.AngularMotorZ__forceLimit = 1000f;
        wheelJoint.AngularMotorZ__targetVelocity = -targetSpeed * 6;

        var cam = GetViewport().GetCamera();

        var raySrc = cam.ProjectRayOrigin(GetViewport().GetMousePosition());
        var rayNorm = cam.ProjectRayNormal(GetViewport().GetMousePosition());
        var rayTo = raySrc + rayNorm * 100;

        var pos = new Vector3();

        var curPos = GetWorld().DirectSpaceState.IntersectRay(raySrc, rayTo, null, 16384);

        if (curPos.Contains("position"))
        {
            pos = (Vector3)curPos["position"];
            //Console.WriteLine(pos);

            var armJoint = this.FindChildByName<Generic6DOFJoint>("ArmJoint");
            var body = this.FindChildByName<RigidBody>("Body");

            var armRootLocation = this.FindChildByName<Spatial>("ArmJointCenter").GlobalTransform;

            GetTree().CurrentScene.FindChildByName<Spatial>("Debug0").SetGlobalLocation(armRootLocation.origin);
            GetTree().CurrentScene.FindChildByName<Spatial>("Debug1").SetGlobalLocation(new Vector3(pos.x, pos.y, 0));
            GetTree().CurrentScene.FindChildByName<Spatial>("Debug2").SetGlobalLocation(pos);

            var relPos = new Vector3(pos.x, pos.y, 0) - new Vector3(armRootLocation.origin.x, armRootLocation.origin.y, 0);

            if (relPos.Length() > 2) relPos = relPos.Normalized() * 2;

            var rotQuat = new Quat(armRootLocation.Inverse().basis);

            var thing = rotQuat.Xform(relPos);

            //Console.WriteLine(thing);

            armJoint.LinearLimitX__lowerDistance = thing.x;
            armJoint.LinearLimitX__upperDistance = thing.x;
            armJoint.LinearLimitY__lowerDistance = thing.y;
            armJoint.LinearLimitY__upperDistance = thing.y;

            //Console.WriteLine(angle.origin);

            /*var zRot = new Quat(body.GlobalTransform.basis).GetEuler().z;

            var angle = (-Mathf.Atan2(pos.y - armRootLocation.origin.y, pos.x - armRootLocation.origin.x) + zRot) * (180 / Mathf.Pi) + 90;

            var dist = new Vector3(pos.x, pos.y, 0).DistanceTo(new Vector3(armRootLocation.origin.x, armRootLocation.origin.y, 0));

            dist = Mathf.Min(dist, 2);

            var od = dist;

            dist = 1;

            //Console.WriteLine($"armRootLocation={armRootLocation} pos={pos}");
            Console.WriteLine($"angle={angle} dist={dist} od={od} zRot={zRot}");

            armJoint.AngularLimitZ__lowerAngle = angle - 0.05f;
            armJoint.AngularLimitZ__upperAngle = angle + 0.05f;*/

            //armJoint.LinearLimitY__lowerDistance = dist - 0.01f;
            //armJoint.LinearLimitY__upperDistance = dist + 0.01f;
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
