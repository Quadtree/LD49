using Godot;
using System;

public class TitleScreen : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CallDeferred(nameof(CreateExitGame));
    }

    private void CreateExitGame()
    {
        GetTree().Root.AddChild(GD.Load<PackedScene>("res://actors/ExitGame.tscn").Instance());
        Console.WriteLine("Exit game created");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsMouseButtonPressed((int)ButtonList.Left))
        {
            GetTree().ChangeScene("res://maps/SetupScreen.tscn");
        }
    }
}
