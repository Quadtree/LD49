using Godot;
using System;

public class Combatant : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public bool MoveLeft = false;
    public bool MoveRight = false;

    [Export]
    public bool IsPlayerControlled;

    private Vector3 ArmJointRelative = new Vector3(-1000000, 0, 0);

    [Export]
    float MercyStabilityTime = 4f;

    [Export]
    float PunchRange = 2.0f;

    //float ExtraPunchTime = 1f;

    public Vector3 CurArmPos = new Vector3();
    public Vector3 DesiredArmPos = new Vector3();

    [Export]
    float ArmMoveSpeed = 0.2f;

    bool PunchGoingOut = false;
    bool PunchComingBack = false;

    float ExtraPunchRange = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public void SetPunchDestFromGlobal(Vector3 pos)
    {
        var armJoint = this.FindChildByName<Generic6DOFJoint>("ArmJoint");
        var body = this.FindChildByName<RigidBody>("Body");

        var armRootLocation = this.FindChildByName<Spatial>("ArmJointCenter").GlobalTransform;

        //GetTree().CurrentScene.FindChildByName<Spatial>("Debug0").SetGlobalLocation(armRootLocation.origin);
        //GetTree().CurrentScene.FindChildByName<Spatial>("Debug1").SetGlobalLocation(new Vector3(pos.x, pos.y, 0));
        //GetTree().CurrentScene.FindChildByName<Spatial>("Debug2").SetGlobalLocation(pos);

        DesiredArmPos = new Vector3(pos.x, Mathf.Max(pos.y, 0.5f), 0) - new Vector3(armRootLocation.origin.x, armRootLocation.origin.y, 0);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (MercyStabilityTime > 0)
        {
            MercyStabilityTime -= delta;
            //var body = this.FindChildByName<RigidBody>("Body");
            //body.Rotation = new Vector3(0, 0, 0);

            if (MercyStabilityTime <= 0)
            {
                this.FindChildByName<Generic6DOFJoint>("MercyStabilizer").QueueFree();
            }
        }

        float targetSpeed = 0f;
        if (MoveLeft && !MoveRight) targetSpeed = -1f;
        if (!MoveLeft && MoveRight) targetSpeed = 1f;

        var wheelJoint = this.FindChildByName<Generic6DOFJoint>("Generic6DOFJoint");
        wheelJoint.AngularMotorZ__enabled = true;
        wheelJoint.AngularMotorZ__forceLimit = 40;
        wheelJoint.AngularMotorZ__targetVelocity = -targetSpeed * 3;

        if (IsPlayerControlled)
        {
            var cam = GetViewport().GetCamera();

            var raySrc = cam.ProjectRayOrigin(GetViewport().GetMousePosition());
            var rayNorm = cam.ProjectRayNormal(GetViewport().GetMousePosition());
            var rayTo = raySrc + rayNorm * 100;

            var curPos = GetWorld().DirectSpaceState.IntersectRay(raySrc, rayTo, null, 16384);

            if (curPos.Contains("position"))
            {
                var pos = (Vector3)curPos["position"];
                SetPunchDestFromGlobal(pos);
            }
        }

        {
            var armDelta = (DesiredArmPos - CurArmPos).Normalized() * ArmMoveSpeed * delta;
            CurArmPos += armDelta;

            var effectivePunchRange = PunchRange + ExtraPunchRange;

            if (CurArmPos.Length() > PunchRange)
            {
                //if (ExtraPunchTime >= delta)
                //{
                //    ExtraPunchTime -= delta;
                //    effectivePunchRange += 1.5f;
                //}
            }

            if (CurArmPos.Length() > effectivePunchRange)
            {
                CurArmPos = CurArmPos.Normalized() * effectivePunchRange;
            }
        }

        //Console.WriteLine(pos);

        {
            var armJoint = this.FindChildByName<Generic6DOFJoint>("ArmJoint");
            var body = this.FindChildByName<RigidBody>("Body");

            var armRootLocation = this.FindChildByName<Spatial>("ArmJointCenter").GlobalTransform;

            //GetTree().CurrentScene.FindChildByName<Spatial>("Debug0").SetGlobalLocation(armRootLocation.origin);
            //GetTree().CurrentScene.FindChildByName<Spatial>("Debug1").SetGlobalLocation(new Vector3(pos.x, pos.y, 0));
            //GetTree().CurrentScene.FindChildByName<Spatial>("Debug2").SetGlobalLocation(pos);

            var relPos = CurArmPos;

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

        {
            var body = this.FindChildByName<RigidBody>("Body");
            var arm = this.FindChildByName<RigidBody>("Arm");

            body.AddForce(new Vector3(0, -8, 0), arm.GetGlobalLocation() - body.GetGlobalLocation());
        }

        if (PunchGoingOut)
        {
            ExtraPunchRange += delta * 2;
            if (ExtraPunchRange >= 2)
            {
                PunchGoingOut = false;
                PunchComingBack = true;
            }
        }
        else if (PunchComingBack)
        {
            ExtraPunchRange -= delta * 1.5f;
            if (ExtraPunchRange <= -1.5f)
            {
                PunchComingBack = false;
                ExtraPunchRange = 0;
            }
        }

        //ExtraPunchTime += 0.5f * delta;
        //if (ExtraPunchTime > 2f) ExtraPunchTime = 2f;


        /*
        var wheels = this.FindChildByName<RigidBody>("Wheels");
        var body = this.FindChildByName<RigidBody>("Body");

        wheels.ApplyTorqueImpulse(new Vector3(0, 0, targetSpeed * -0.05f));
        body.ApplyImpulse(wheels.GetGlobalLocation() - body.GetGlobalLocation(), new Vector3(targetSpeed * 1f, 0, 0));
        */

        {
            var armRootLocation = this.FindChildByName<Spatial>("ArmJointCenter").GlobalTransform;
            if (armRootLocation.origin.y < 1f)
            {
                GetTree().ChangeScene("res://maps/default.tscn");
            }
        }

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

            if (@event.IsActionReleased("punch") && !PunchGoingOut && !PunchComingBack) PunchGoingOut = true;
        }
    }
}
