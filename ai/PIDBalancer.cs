using Godot;
using System;

public class PIDBalancer : Spatial
{
    private float PastError = 0f;

    [Export]
    public float CenterPoint = 0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var cmb = GetParent<Combatant>();

        if (cmb.MoveLeft || cmb.MoveRight)
        {
            PastError = 0;
            return;
        }

        var body = cmb.FindChildByName<RigidBody>("Body");
        var bodyRotation = body.Rotation.z;
        var bodyRotationRate = body.AngularVelocity.z;

        PastError += (bodyRotation - CenterPoint) * delta;

        var GAIN = -1f;
        var TIME_I = 0.5f;
        var TIME_D = 0.5f;

        var control = GAIN * ((bodyRotation - CenterPoint) + (1 / TIME_I) * PastError + TIME_D * bodyRotationRate);

        //Console.WriteLine(control);

        cmb.TargetSpeed = control;
    }
}
