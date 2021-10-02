using Godot;
using System;

public class PIDBalancer : Spatial
{
    private float PastError = 0f;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var cmb = GetParent<Combatant>();
        var body = cmb.FindChildByName<RigidBody>("Body");
        var bodyRotation = body.Rotation.z;
        var bodyRotationRate = body.AngularVelocity.z;

        PastError += bodyRotation * delta;

        var GAIN = -1f;
        var TIME_I = 1f;
        var TIME_D = 1f;

        var control = GAIN * (bodyRotation + (1 / TIME_I) * PastError + TIME_D * bodyRotationRate);

        //Console.WriteLine(control);

        cmb.TargetSpeed = control;
    }
}
