using Godot;
using System;

public class PIDBalancer : Spatial
{
    private float PastError = 0f;

    [Export]
    public float CenterPoint = 0f;

    [Export]
    public float TimeI = 0.5f;

    [Export]
    public float TimeD = 0.5f;

    [Export]
    public float Gain = -1f;

    [Export]
    public float GiveUpTilt = 2.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var cmb = GetParent<Combatant>();
        var body = cmb.FindChildByName<RigidBody>("Body");

        if (cmb.MoveLeft || cmb.MoveRight || body.GetGlobalLocation().y >= 3)
        {
            PastError = 0;
            return;
        }

        var bodyRotation = body.Rotation.z;

        if (Math.Abs(bodyRotation) > GiveUpTilt && cmb.HitAtLeastOnce)
        {
            cmb.TargetSpeed = 0;
            return;
        }

        var bodyRotationRate = body.AngularVelocity.z;

        PastError += (bodyRotation - CenterPoint) * delta;

        var GAIN = Gain;
        var TIME_I = TimeI;
        var TIME_D = TimeD;

        if (!cmb.HitAtLeastOnce)
        {
            TIME_I = Math.Min(TIME_I, 0.75f);
            TIME_D = Math.Min(TIME_D, 0.75f);
        }

        var control = GAIN * ((bodyRotation - CenterPoint) + (1 / TIME_I) * PastError + TIME_D * bodyRotationRate);

        //Console.WriteLine(body.GetGlobalLocation().y);

        cmb.TargetSpeed = control;
    }
}
