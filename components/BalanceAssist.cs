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

        if (cmb.MoveRight) bodyRotationRate += 0.4f;
        if (cmb.MoveLeft) bodyRotationRate -= 0.4f;

        PastError += (bodyRotation - 0) * delta;

        var GAIN = -30f;
        var TIME_I = .5f;
        var TIME_D = .5f;

        var control = GAIN * ((bodyRotation - 0) + (1 / TIME_I) * PastError + TIME_D * bodyRotationRate);

        if ((bodyRotation > 0) == (bodyRotationRate > 0))
        {
            //Console.WriteLine("BRAKING");
            body.AngularDamp = 10;
        }
        else
        {
            body.AngularDamp = -1;
        }

        //Console.WriteLine($"{bodyRotation} {bodyRotationRate} {control}");

        //Console.WriteLine(body.GetGlobalLocation().y);

        body.ApplyTorqueImpulse(new Vector3(0, 0, control * delta));
    }
}
