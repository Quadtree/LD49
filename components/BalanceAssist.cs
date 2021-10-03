using Godot;
using System;

public class BalanceAssist : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    float PastError;

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

        PastError += (bodyRotation - 0) * delta;

        var GAIN = 0.5f;
        var TIME_I = 0.25f;
        var TIME_D = 0.25f;

        var control = GAIN * ((bodyRotation - 0) + (1 / TIME_I) * PastError + TIME_D * bodyRotationRate);

        //Console.WriteLine(body.GetGlobalLocation().y);

        body.ApplyTorqueImpulse(new Vector3(0, 0, control * delta));
    }
}
