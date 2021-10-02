using Godot;
using System;

public class AggressivePuncher : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    float Height = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var cmb = GetParent<Combatant>();
        cmb.DesiredArmPos = new Vector3(5, Height, 0);

        if (Util.random() < delta / 4)
        {
            Height = (Util.random() - 0.5f) * 5;
        }

        var enemy = GetTree().CurrentScene.FindChildByName<Combatant>("Player", 0);
        if (enemy.FindChildByName<Spatial>("Body").GetGlobalLocation().DistanceTo(cmb.FindChildByName<Spatial>("Body").GetGlobalLocation()) < 4)
        {
            cmb.Punch();
        }

        //Console.WriteLine(enemy.GetGlobalLocation().DistanceTo(cmb.GetGlobalLocation()));
    }
}
