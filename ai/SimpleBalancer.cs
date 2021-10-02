using Godot;
using System;

public class SimpleBalancer : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var cmb = GetParent<Combatant>();



        var body = cmb.FindChildByName<RigidBody>("Body");

        var bodyRotation = body.Rotation.z;

        Console.WriteLine(bodyRotation);

        if (Mathf.Abs(bodyRotation) < 0.4)
        {
            cmb.DesiredArmPos = new Vector3(5, 0, 0);
        }
        else
        {
            if (bodyRotation > 0)
            {
                cmb.DesiredArmPos = new Vector3(5, 0, 0);
            }

            if (bodyRotation < 0)
            {
                cmb.DesiredArmPos = new Vector3(-5, 0, 0);
            }
        }

        if (bodyRotation > 0)
        {
            cmb.MoveLeft = true;
            cmb.MoveRight = false;
        }

        if (bodyRotation < 0)
        {
            cmb.MoveLeft = false;
            cmb.MoveRight = true;
        }
    }
}
