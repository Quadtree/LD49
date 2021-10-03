using Godot;
using System;

public class BGM : AudioStreamPlayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (!Playing)
        {
            this.Stream = (AudioStream)GD.Load($"res://music/bgm{Util.RandInt(0, 4)}.ogg");
            this.Play();
        }
    }
}
