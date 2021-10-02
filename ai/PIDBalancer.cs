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
        var bodyRotationRate = body.AngularVelocity.z;

        PastError += (bodyRotation - CenterPoint) * delta;

        var GAIN = -1f;
        var TIME_I = TimeI;
        var TIME_D = TimeD;

        var control = GAIN * ((bodyRotation - CenterPoint) + (1 / TIME_I) * PastError + TIME_D * bodyRotationRate);

        //Console.WriteLine(body.GetGlobalLocation().y);

        cmb.TargetSpeed = control;
    }
}
